using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class UserComment
{
    public int ComId { get; set; }

    public int? UserId { get; set; }

    public virtual Comment Com { get; set; } = null!;

    public virtual ApplicationUser? User { get; set; }
}
