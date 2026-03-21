namespace SmartStore.Core.Domain;

/// <summary>
/// Aggregate root for an order. Holds the line items (leaf or composite),
/// status, discount, and computed totals.
/// </summary>
public class Order
{
    public int Id { get; init; }
    public Customer Customer { get; init; } = null!;
    public List<OrderItemBase> Items { get; init; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Draft;
    public decimal Discount { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public string? Notes { get; set; }

    // Computed
    public decimal SubTotal => Items.Sum(i => i.GetTotalPrice());
    public decimal Total => SubTotal - Discount;

    public void Display()
    {
        Console.WriteLine($"  Order #{Id}  |  {Customer}  |  Status: {Status}");
        Console.WriteLine($"  Created : {CreatedAt:yyyy-MM-dd HH:mm}");
        foreach (var item in Items)
            item.Display("    ");
        if (Discount > 0)
            Console.WriteLine($"    {'─',20}  Discount  : -${Discount:F2}");
        Console.WriteLine($"    {'─',20}  TOTAL     :  ${Total:F2}");
        if (!string.IsNullOrEmpty(Notes))
            Console.WriteLine($"    Notes: {Notes}");
    }
}
