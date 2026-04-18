
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certRepo;
        private readonly IProgressRepository _progressRepo;

        public CertificateService(ICertificateRepository certRepo, IProgressRepository progressRepo)
        {
            _certRepo = certRepo;
            _progressRepo = progressRepo;
        }

        public List<CertificateResponse> GetMyCertificates(int userId)
        {
            var certs = _certRepo.GetByUserId(userId);
            return certs.Select(MapToResponse).ToList();
        }

        public CertificateResponse GetById(int certId, int userId)
        {
            var cert = _certRepo.GetById(certId);
            if (cert == null)
                throw new Exception("not found");
            if (cert.UserId != userId)
                throw new Exception("forbidden");

            return MapToResponse(cert);
        }

        public CertificateResponse AutoIssue(int userId, int roadmapId)
        {
            var existing = _certRepo.GetByUserAndRoadmap(userId, roadmapId);
            if (existing != null)
                return MapToResponse(existing);

            bool isCompleted = _progressRepo.IsRoadmapCompleted(userId, roadmapId);
            if (!isCompleted)
                throw new Exception("roadmap not completed yet");

            var cert = new Certificate
            {
                UserId = userId,
                Rid = roadmapId,
                CertUrl = $"https://mindmap.io/certificates/{Guid.NewGuid()}",
                IssuedAt = DateTime.UtcNow
            };

            _certRepo.Add(cert);
            try
            {
                _certRepo.Save();
                return MapToResponse(cert);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        private static CertificateResponse MapToResponse(Certificate cert) => new()
        {
            CertId = cert.CertId,
            CertUrl = cert.CertUrl ?? string.Empty,
            IssuedAt = cert.IssuedAt ?? DateTime.UtcNow,
            RoadmapId = cert.Rid ?? 0
        };
    }
}