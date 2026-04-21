
namespace MindMapManager.Core.DTOs
{
    public class CommunityRequest
    {
        public string Content { get; set; } = string.Empty;
    }

    public class CommunityResponse
    {
        public int ComId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public List<CommunityResponse> Replies { get; set; } = new();
    }
}