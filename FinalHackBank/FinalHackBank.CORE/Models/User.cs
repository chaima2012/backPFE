using System;
using System.Collections.Generic;

namespace FinalHackBank.CORE.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? StatusUserId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual StatusUser? StatusUser { get; set; }
}
