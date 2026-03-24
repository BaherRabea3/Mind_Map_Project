using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Level
{
    public int Lid { get; set; }

    public string Name { get; set; } = null!;

    public int? Rid { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Roadmap? RidNavigation { get; set; }

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
