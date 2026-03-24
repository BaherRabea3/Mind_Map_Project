using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class VwUserCertificate
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Roadmap { get; set; } = null!;

    public string Track { get; set; } = null!;

    public string? CertUrl { get; set; }

    public DateTime? IssuedAt { get; set; }
}
