namespace SmartStore.Core.Domain;

// -------------------------------------------------------
// COMPOSITE PATTERN — Component
// -------------------------------------------------------
// OrderItemBase is the abstract component.
// Both leaf (OrderItem) and composite (BundleOrderItem) extend it.
// Client code in Order treats all items uniformly via this type.
// -------------------------------------------------------
public abstract class OrderItemBase
{
    public abstract string Name { get; }
    public abstract decimal GetTotalPrice();
    public abstract void Display(string indent = "");
}
