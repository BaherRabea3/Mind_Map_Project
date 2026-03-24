using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Resource
{
    public int ResId { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public string? Type { get; set; }

    public string ResUrl { get; set; } = null!;

    public bool? Paid { get; set; }

    public int? TopicId { get; set; }

    public virtual Topic? Topic { get; set; }

    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    public virtual ICollection<ApplicationUser> UsersNavigation { get; set; } = new List<ApplicationUser>();
}
