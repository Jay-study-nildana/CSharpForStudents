namespace ComicBookShop.Core.Models;

/// <summary>
/// Immutable receipt returned after an order is placed.
/// Demonstrates records with nested data (Day 10).
/// </summary>
public record OrderReceipt(
    Guid OrderId,
    string CustomerName,
    DateTime OrderDate,
    IReadOnlyList<OrderLineItem> Lines,
    decimal Subtotal,
    decimal Discount,
    decimal Total);

/// <summary>Single line on a receipt.</summary>
public record OrderLineItem(
    string ComicTitle,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
