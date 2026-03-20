namespace ComicBookShop.Core.Enums;

/// <summary>Lifecycle status of a customer order.</summary>
public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
