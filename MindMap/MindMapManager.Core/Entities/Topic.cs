using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Topic
{
    public int TopicId { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public int? Lid { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Level? LidNavigation { get; set; }

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
