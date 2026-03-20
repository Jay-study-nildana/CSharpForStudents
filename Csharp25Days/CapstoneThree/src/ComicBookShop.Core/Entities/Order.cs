using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Entities;

/// <summary>
/// Represents a customer order containing one or more comic book items.
/// Demonstrates relationships between entities, collections in classes (Days 6, 11).
/// </summary>
public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }

    public override string ToString() =>
        $"Order {Id.ToString()[..8]} - {CustomerName} - {Items.Count} item(s) - ${Total:F2} [{Status}]";
}
