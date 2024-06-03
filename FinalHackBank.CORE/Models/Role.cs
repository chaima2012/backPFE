using FinalHackBank.CORE.Interfaces;
using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Role : IEntityWithName
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
