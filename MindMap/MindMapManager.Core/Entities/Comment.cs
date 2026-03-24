using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Comment
{
    public int ComId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? TopicId { get; set; }

    public int? ParentComId { get; set; }

    public virtual ICollection<Comment> InverseParentCom { get; set; } = new List<Comment>();

    public virtual Comment? ParentCom { get; set; }

    public virtual Topic? Topic { get; set; }

    public virtual UserComment? UserComment { get; set; }
}
