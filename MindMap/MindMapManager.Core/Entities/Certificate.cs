using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Certificate
{
    public int CertId { get; set; }

    public string? CertUrl { get; set; }

    public int? UserId { get; set; }

    public int? Rid { get; set; }

    public DateTime? IssuedAt { get; set; }

    public virtual Roadmap? RidNavigation { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
