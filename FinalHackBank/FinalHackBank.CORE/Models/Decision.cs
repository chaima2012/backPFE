using FinalHackBank.CORE.Interfaces;
using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Decision : IEntityWithName
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Requestt> Requestts { get; set; } = new List<Requestt>();
}
