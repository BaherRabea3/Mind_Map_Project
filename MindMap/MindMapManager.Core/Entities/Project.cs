using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Project
{
    public int ProjId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Lid { get; set; }

    public virtual Level? LidNavigation { get; set; }

    public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
}
