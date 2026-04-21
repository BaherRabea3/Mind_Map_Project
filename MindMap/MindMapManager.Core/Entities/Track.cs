using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Track
{
    public int TrackId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public virtual ICollection<Roadmap> Roadmaps { get; set; } = new List<Roadmap>();

    public virtual ICollection<UserTrack> UserTracks { get; set; } = new List<UserTrack>();
}
