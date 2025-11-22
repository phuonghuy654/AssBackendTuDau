using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Character
{
    public int CharacterId { get; set; }

    public int AccountId { get; set; }

    public string? Mode { get; set; }

    public int? Health { get; set; }

    public int? Hunger { get; set; }

    public int? LevelXp { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CharacterTask> CharacterTasks { get; set; } = new List<CharacterTask>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
