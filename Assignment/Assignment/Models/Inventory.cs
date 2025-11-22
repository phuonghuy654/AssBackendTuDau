using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Inventory
{
    public int CharacterId { get; set; }

    public int ResourceId { get; set; }

    public int? Quantity { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;
}
