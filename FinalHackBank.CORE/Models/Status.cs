using FinalHackBank.CORE.Interfaces;
using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Status : IEntityWithName
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
}
