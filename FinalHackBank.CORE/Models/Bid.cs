using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Bid
{
    public int BidId { get; set; }

    public int CompanyId { get; set; }

    public int DocumentId { get; set; }

    public double AmountTtc { get; set; }

    public int? Winner { get; set; }

    public int Idcall { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Documentt Document { get; set; } = null!;

    public virtual Call IdcallNavigation { get; set; } = null!;
}
