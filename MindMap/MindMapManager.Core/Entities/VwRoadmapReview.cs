using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwRoadmapReview
{
    public int Rid { get; set; }

    public string Roadmap { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int Rate { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
