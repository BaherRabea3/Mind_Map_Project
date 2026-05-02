
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certRepo;
        private readonly IProgressRepository _progressRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly IRoadmapRepository _roadmapRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CertificateService(ICertificateRepository certRepo, IProgressRepository progressRepo, INotificationRepository notificationRepo, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager, IRoadmapRepository roadmapRepo)
        {
            _certRepo = certRepo;
            _progressRepo = progressRepo;
            _notificationRepo = notificationRepo;
            _environment = environment;
            _roadmapRepo = roadmapRepo;
            _userManager = userManager;
        }

        public List<CertificateResponse> GetMyCertificates(int userId)
        {
            var certs = _certRepo.GetByUserIdWithRoadmaps(userId);
            return certs.Select(MapToResponse).ToList();
        }

        public CertificateResponse GetById(int certId, int userId)
        {
            var cert = _certRepo.GetById(certId , userId);

            if (cert == null)
                throw new NotFoundException("certificate not found");
            if (cert.UserId != userId)
                throw new ForbiddenException("complete the roadmap first");

            return MapToResponse(cert);
        }

        public async Task AutoIssue(int userId, int roadmapId)
        {
            var exists = _certRepo.IsAlreadyExisted(userId, roadmapId);

            if (exists)
                return;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roadmap = _roadmapRepo.GetById(roadmapId);

            if (user == null || roadmap == null)
                throw new BadRequestException("Invalid certificate data");

            var code = Guid.NewGuid().ToString("N");
            var fileName = $"{code}.pdf";
            var DirectoryPath = Path.Combine(_environment.WebRootPath, "certificates");

            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            var fullPath = Path.Combine(DirectoryPath, fileName);

            CertificatePdfGenerator.Generate(fullPath, user.FullName, roadmap.Name, DateTime.UtcNow, code);

            var cert = new Certificate
            {
                UserId = userId,
                Rid = roadmapId,
                CertUrl = $"certificates/{fileName}",
                IssuedAt = DateTime.UtcNow,
                CertificateCode = code
            };

            _certRepo.Add(cert);
            _certRepo.Save();

            _notificationRepo.Add(new Notification()
            {
                Message = "🎉 Congratulations! You’ve earned a new certificate!",
                CreatedAt = DateTime.UtcNow,
                Read = false,
                UserId = userId,
            });
            _notificationRepo.Save();
        }

        private static CertificateResponse MapToResponse(Certificate cert) => new()
        {
            CertId = cert.CertId,
            CertUrl = cert.CertUrl ?? string.Empty,
            IssuedAt = cert.IssuedAt ?? DateTime.UtcNow,
            RoadmapName =cert.RidNavigation.Name
        };

        public DownloadCertificateResponse DownloadCertificate(int certId, int userId)
        {
            var certificate = _certRepo.GetById(certId, userId);

            if (certificate == null)
                throw new NotFoundException("Certificate not found");

            var FullPath = Path.Combine(
                _environment.WebRootPath,
                certificate.CertUrl.TrimStart('/'));

            if (!File.Exists(FullPath))
                throw new NotFoundException();

            return new DownloadCertificateResponse()
            {
                fileName = Path.GetFileName(FullPath),
                contentType = "application/pdf",
                physicalPath = FullPath,
            };
        }

        public CertificateVerificationResponse Verify(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
            {
                return new CertificateVerificationResponse()
                {
                    IsValid = false
                };
            }

            var certificate = _certRepo.GetByCodeWithUserAndRoadmap(code);

            if (certificate == null)
            {
                return new CertificateVerificationResponse()
                {
                    IsValid = false
                };
            }

            return new CertificateVerificationResponse()
            {
                IsValid = true,
                UserName = certificate.User.UserName,
                RoadmapName = certificate.RidNavigation.Name,
                IssuedAt = certificate.IssuedAt ?? DateTime.UtcNow
            };
        }
    }
}