namespace MindMapManager.Core.DTOs
{
    public class CertificateVerificationResponse
    {
        public bool IsValid { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string RoadmapName { get; set; } = string.Empty;
        public DateTime? IssuedAt { get; set; }

        public string Issuer { get; set; } = "MindRoad";
    }

}
