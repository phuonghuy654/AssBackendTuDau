using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class ShopItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemType { get; set; }

    public int? PriceXp { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MobDrop> MobDrops { get; set; } = new List<MobDrop>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
