using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Call
{
    public int CallId { get; set; }

    public string Descriptions { get; set; } = null!;

    public int StatusId { get; set; }

    public int? Budget { get; set; }

    public string Remark { get; set; } = null!;

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Status Status { get; set; } = null!;
}
