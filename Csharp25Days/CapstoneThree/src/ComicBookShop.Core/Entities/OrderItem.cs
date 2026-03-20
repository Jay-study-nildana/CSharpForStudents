using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Entities;

/// <summary>
/// Represents a single line item in an order.
/// Demonstrates computed properties and value-object-like design (Day 10).
/// </summary>
public class OrderItem
{
    public Guid ComicBookId { get; set; }
    public string ComicTitle { get; set; } = string.Empty;
    public Genre Genre { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    /// <summary>Computed line total — Quantity × UnitPrice.</summary>
    public decimal LineTotal => Quantity * UnitPrice;
}
