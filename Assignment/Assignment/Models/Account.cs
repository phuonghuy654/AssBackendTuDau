using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? DateCreate { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
