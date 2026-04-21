namespace MindMapManager.Core.DTOs
{
    public class TrackProgressResponse
    {
        public int trackId { get; set; }
        public string trackName { get; set; }
        public IEnumerable<RoadmapProgressResponse> Roadmaps { get; set; }
    }
}
