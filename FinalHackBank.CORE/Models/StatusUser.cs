using FinalHackBank.CORE.Interfaces;
using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class StatusUser : IEntityWithName
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
