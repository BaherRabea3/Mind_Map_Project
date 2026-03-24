using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwCommentThread
{
    public int ComId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? TopicId { get; set; }

    public string Topic { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int? ParentComId { get; set; }

    public string Type { get; set; } = null!;
}
