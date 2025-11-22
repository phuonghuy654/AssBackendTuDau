using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Mob
{
    public int MobId { get; set; }

    public string MobName { get; set; } = null!;

    public int? Health { get; set; }

    public int? Damage { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MobDrop> MobDrops { get; set; } = new List<MobDrop>();
}
