using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class Requestt
{
    public int RequestId { get; set; }

    public string Description { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public string Daterequest { get; set; } = null!;

    public int StatusId { get; set; }

    public int DecisionId { get; set; }

    public virtual Decision Decision { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual StatusDemand Status { get; set; } = null!;
}
