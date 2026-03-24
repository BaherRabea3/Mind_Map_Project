using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwUserDashboard
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int? Streak { get; set; }

    public DateTime? LastActDate { get; set; }

    public string Plan { get; set; } = null!;

    public string Track { get; set; } = null!;

    public string Roadmap { get; set; } = null!;

    public string Level { get; set; } = null!;

    public decimal CompPerc { get; set; }
}
