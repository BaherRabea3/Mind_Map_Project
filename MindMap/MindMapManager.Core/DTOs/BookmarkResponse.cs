
namespace MindMapManager.Core.DTOs
{
    public class BookmarkResponse
    {
        public int ResId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string ResUrl { get; set; } = string.Empty;
        public bool? Paid { get; set; }
    }
}
