using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwLeaderboard
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int? Streak { get; set; }

    public int? CompletedRoadmaps { get; set; }

    public decimal? AvgProgress { get; set; }
}
