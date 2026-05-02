
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ICertificateService
    {
        public List<CertificateResponse> GetMyCertificates(int userId);
        public CertificateResponse GetById(int certId, int userId);
        public Task AutoIssue(int userId, int roadmapId);
        public DownloadCertificateResponse DownloadCertificate(int certId, int userId);
        public CertificateVerificationResponse Verify(string code);
    }
}