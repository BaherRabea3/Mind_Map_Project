using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwRoadmapDetail
{
    public int Rid { get; set; }

    public string Roadmap { get; set; } = null!;

    public string? Description { get; set; }

    public string Track { get; set; } = null!;

    public int? TotalLevels { get; set; }

    public int? TotalTopics { get; set; }

    public int? TotalResources { get; set; }

    public decimal? AvgRating { get; set; }

    public int? TotalReviews { get; set; }
}
