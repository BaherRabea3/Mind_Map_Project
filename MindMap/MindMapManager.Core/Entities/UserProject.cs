using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class UserProject
{
    public int UserId { get; set; }

    public int ProjId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public virtual Project Proj { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
