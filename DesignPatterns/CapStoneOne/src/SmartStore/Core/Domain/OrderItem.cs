namespace SmartStore.Core.Domain;

// -------------------------------------------------------
// COMPOSITE PATTERN — Leaf
// -------------------------------------------------------
// OrderItem is a leaf node. It wraps a single Product + Quantity.
// -------------------------------------------------------
public class OrderItem : OrderItemBase
{
    public Product Product { get; init; } = null!;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }

    public override string Name => Product.Name;
    public override decimal GetTotalPrice() => UnitPrice * Quantity;

    public override void Display(string indent = "") =>
        Console.WriteLine($"{indent}- {Product.Name,-20} x{Quantity}  @ ${UnitPrice:F2}  = ${GetTotalPrice():F2}");
}
