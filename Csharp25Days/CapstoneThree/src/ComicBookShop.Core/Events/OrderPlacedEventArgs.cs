namespace ComicBookShop.Core.Events;

/// <summary>
/// Event data raised when a new order is placed.
/// Demonstrates delegates and event-driven design (Day 19).
/// </summary>
public class OrderPlacedEventArgs : EventArgs
{
    public Guid OrderId { get; }
    public string CustomerName { get; }
    public decimal Total { get; }
    public int ItemCount { get; }

    public OrderPlacedEventArgs(Guid orderId, string customerName, decimal total, int itemCount)
    {
        OrderId = orderId;
        CustomerName = customerName;
        Total = total;
        ItemCount = itemCount;
    }
}
