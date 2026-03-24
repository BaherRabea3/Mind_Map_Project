using System;
using System.Collections.Generic;

namespace MindMapManager.Core.Entities;

public partial class Progress
{
    public int ProgId { get; set; }

    public decimal CompPerc { get; set; }

    public int? UserId { get; set; }

    public int? Lid { get; set; }

    public virtual Level? LidNavigation { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
