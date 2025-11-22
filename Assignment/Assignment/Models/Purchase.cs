using System;
using System.Collections.Generic;

namespace Assignment.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int CharacterId { get; set; }

    public int ItemId { get; set; }

    public DateTime? DatePurchased { get; set; }

    public int? PriceAtPurchase { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual ShopItem Item { get; set; } = null!;
}
