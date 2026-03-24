using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwUserProgress
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int Rid { get; set; }

    public string Roadmap { get; set; } = null!;

    public int Lid { get; set; }

    public string Level { get; set; } = null!;

    public decimal CompPerc { get; set; }

    public string ProjectStatus { get; set; } = null!;
}
