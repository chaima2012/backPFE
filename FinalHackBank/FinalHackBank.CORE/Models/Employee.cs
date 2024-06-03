using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Employee
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Requestt> Requestts { get; set; } = new List<Requestt>();

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
