namespace SmartStore.Patterns.Behavioral;

// ================================================================
// TEMPLATE METHOD PATTERN
// ================================================================
// Defines the invariant skeleton of the order-processing algorithm.
// Concrete subclasses override specific steps without altering the
// overall sequence.
//
// Intent   : Define the skeleton of an algorithm in an operation,
//            deferring some steps to subclasses.
// Problem  : Both Standard and Express order processing share the same
//            high-level steps (validate → reserve → business rules →
//            finalize) but differ in their details.
// Solution : OrderProcessingTemplate declares the final Process() method
//            and calls abstract/virtual hook methods that subclasses fill.
// ================================================================
public abstract class OrderProcessingTemplate
{
    /// <summary>The invariant algorithm — not overridable by subclasses.</summary>
    public void Process(Order order)
    {
        Console.WriteLine($"\n  [Template] Processing Order #{order.Id} ({GetType().Name})...");
        ValidateOrder(order);
        ReserveStock(order);
        ApplyBusinessRules(order);   // virtual hook — subclasses may override
        FinalizeOrder(order);
        Console.WriteLine($"  [Template] Order #{order.Id} processing complete. Status: {order.Status}");
    }

    protected abstract void ValidateOrder(Order order);
    protected abstract void ReserveStock(Order order);

    /// <summary>Optional hook. Default is a no-op.</summary>
    protected virtual void ApplyBusinessRules(Order order) { }

    private static void FinalizeOrder(Order order)
    {
        order.Status = OrderStatus.Confirmed;
        Console.WriteLine($"    [Template:Finalize] Status set to {order.Status}.");
    }
}

// ================================================================
// Standard Order Processor — concrete class
// ================================================================
public class StandardOrderProcessor : OrderProcessingTemplate
{
    protected override void ValidateOrder(Order order)
    {
        Console.WriteLine("    [Standard] Validating order integrity...");
        if (order.Items.Count == 0)
            throw new InvalidOperationException("Cannot process an order with no items.");
    }

    protected override void ReserveStock(Order order)
    {
        Console.WriteLine("    [Standard] Reserving stock for all leaf items...");
        foreach (var item in order.Items.OfType<OrderItem>())
            item.Product.Stock -= item.Quantity;
    }

    protected override void ApplyBusinessRules(Order order)
    {
        Console.WriteLine("    [Standard] Applying loyalty bonus for VIP customers...");
        if (order.Customer.Type == CustomerType.Vip)
            order.Notes = (order.Notes is null ? "" : order.Notes + " | ") + "VIP fast-track enabled";
    }
}

// ================================================================
// Express Order Processor — concrete class
// ================================================================
public class ExpressOrderProcessor : OrderProcessingTemplate
{
    protected override void ValidateOrder(Order order)
    {
        Console.WriteLine("    [Express] Quick-validating order...");
    }

    protected override void ReserveStock(Order order)
    {
        Console.WriteLine("    [Express] Priority stock lock applied.");
        foreach (var item in order.Items.OfType<OrderItem>())
            item.Product.Stock -= item.Quantity;
    }

    protected override void ApplyBusinessRules(Order order)
    {
        Console.WriteLine("    [Express] Flagging for same-day dispatch.");
        order.Notes = (order.Notes is null ? "" : order.Notes + " | ") + "EXPRESS DISPATCH";
    }
}
