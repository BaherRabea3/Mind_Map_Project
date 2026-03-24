using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwRoadmapStat
{
    public int Rid { get; set; }

    public string Roadmap { get; set; } = null!;

    public string Track { get; set; } = null!;

    public int? TotalEnrollments { get; set; }

    public int? TotalCompletions { get; set; }

    public decimal? AvgRating { get; set; }
}
