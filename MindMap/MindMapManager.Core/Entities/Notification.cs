using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Notification
{
    public int NotId { get; set; }

    public string Message { get; set; } = null!;

    public bool Read { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? RefId { get; set; }

    public string? RefType { get; set; }

    public int? UserId { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
