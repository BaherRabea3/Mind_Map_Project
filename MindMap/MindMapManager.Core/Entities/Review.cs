using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Review
{
    public int RevId { get; set; }

    public string? Content { get; set; }

    public int Rate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UserId { get; set; }

    public int? Rid { get; set; }

    public virtual Roadmap? RidNavigation { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
