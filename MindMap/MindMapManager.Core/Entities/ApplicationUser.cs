using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class ApplicationUser : IdentityUser<int>
{
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public string Status { get; set; } = null!;

    public int? Streak { get; set; }

    public DateTime? LastActDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? Pid { get; set; }

    public int? OtpCode { get; set; }

    public DateTime? OtpExpire { get; set; }

    public bool? IsVerified { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Plan? PidNavigation { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();

    public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

    public virtual ICollection<Resource> Res { get; set; } = new List<Resource>();

    public virtual ICollection<Resource> ResNavigation { get; set; } = new List<Resource>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
