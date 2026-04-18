
namespace MindMapManager.Core.DTOs
{
    public class NotificationResponse
    {
        public int NotId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Read { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }
}
