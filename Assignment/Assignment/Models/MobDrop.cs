using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class MobDrop
{
    public int DropId { get; set; }

    public int MobId { get; set; }

    public int ItemId { get; set; }

    public virtual ShopItem Item { get; set; } = null!;

    public virtual Mob Mob { get; set; } = null!;
}
