

namespace MindMapManager.Core.DTOs
{
    public class TrackResponse
    {
        public int TrackId { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public string TrackDescription { get; set; } = string.Empty;
        public string TrackIcon {  get; set; } = string.Empty;
        public int RoadmapsCount { get; set; }
        public int EnrollmentsCount { get; set; }
    }
}
