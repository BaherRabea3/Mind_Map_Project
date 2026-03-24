using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwUserBookmark
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public int ResId { get; set; }

    public string Resource { get; set; } = null!;

    public string? Type { get; set; }

    public string ResUrl { get; set; } = null!;

    public bool? Paid { get; set; }

    public string Topic { get; set; } = null!;

    public string Level { get; set; } = null!;

    public string Roadmap { get; set; } = null!;
}
