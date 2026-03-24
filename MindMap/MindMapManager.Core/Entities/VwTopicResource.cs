using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwTopicResource
{
    public int Rid { get; set; }

    public string Roadmap { get; set; } = null!;

    public int Lid { get; set; }

    public string Level { get; set; } = null!;

    public int TopicId { get; set; }

    public string Topic { get; set; } = null!;

    public int TopicOrder { get; set; }

    public int ResId { get; set; }

    public string Resource { get; set; } = null!;

    public int ResourceOrder { get; set; }

    public string? Type { get; set; }

    public string ResUrl { get; set; } = null!;

    public bool? Paid { get; set; }
}
