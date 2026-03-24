using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Plan
{
    public int Pid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}
