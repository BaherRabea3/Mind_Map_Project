
namespace MindMapManager.Core.DTOs
{
    public class CertificateResponse
    {
        public int CertId { get; set; }
        public string RoadmapName { get; set; } = string.Empty;
        public string CertUrl { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
    }
}