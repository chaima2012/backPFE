using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Documentt
{
    public int DocumentId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Size { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
