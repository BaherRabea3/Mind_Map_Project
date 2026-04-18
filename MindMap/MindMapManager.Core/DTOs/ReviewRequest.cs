
namespace MindMapManager.Core.DTOs
{
    public class ReviewRequest
    {
        public string Content { get; set; } = string.Empty;
        public int Rate { get; set; }
    }

    public class ReviewResponse
    {
        public int RevId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int RoadmapId { get; set; }
    }
}