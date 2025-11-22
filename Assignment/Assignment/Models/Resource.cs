using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public string? ResourceName { get; set; }

    public string? ResourceType { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
