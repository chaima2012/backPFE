using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Company
{
    public int UserId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual User User { get; set; } = null!;
}
