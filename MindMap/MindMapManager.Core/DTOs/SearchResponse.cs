
namespace MindMapManager.Core.DTOs
{
    public class SearchResponse
    {
        public List<TrackSearchResult> Tracks { get; set; } = new();
        public List<RoadmapSearchResult> Roadmaps { get; set; } = new();
        public List<TopicSearchResult> Topics { get; set; } = new();
    }

    public class TrackSearchResult
    {
        public int TrackId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class RoadmapSearchResult
    {
        public int Rid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class TopicSearchResult
    {
        public int TopicId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
