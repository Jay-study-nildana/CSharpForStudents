namespace SmartStore.Core.Domain;

public enum OrderStatus
{
    Draft,
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
