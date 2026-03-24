using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Roadmap
{
    public int Rid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? TrackId { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Level> Levels { get; set; } = new List<Level>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Track? Track { get; set; }
}
