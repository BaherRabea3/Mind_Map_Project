
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ICertificateService
    {
        public List<CertificateResponse> GetMyCertificates(int userId);
        public CertificateResponse GetById(int certId, int userId);
        public CertificateResponse AutoIssue(int userId, int roadmapId);
    }
}