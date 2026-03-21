namespace SmartStore.Patterns.Behavioral.ValidationChain;

// ================================================================
// CHAIN OF RESPONSIBILITY PATTERN
// ================================================================
// Each handler checks one validation rule before passing the request
// to the next handler in the chain. A handler may short-circuit
// (return failure) at any point.
//
// Intent   : Avoid coupling the sender of a request to its receiver
//            by giving more than one object a chance to handle it.
// Problem  : Order validation involves multiple independent rules.
//            Nesting them all in one class creates fragile, untestable code.
// Solution : Each rule is its own handler. Handlers are chained at
//            startup. Any handler can short-circuit or pass on.
// ================================================================

public class ValidationResult
{
    public bool   IsValid      { get; private init; }
    public string ErrorMessage { get; private init; } = string.Empty;

    public static ValidationResult Ok()                   => new() { IsValid = true };
    public static ValidationResult Fail(string message)   => new() { IsValid = false, ErrorMessage = message };
}

public abstract class OrderValidationHandler
{
    protected OrderValidationHandler? _next;

    /// <summary>Links to the next handler and returns it for fluent chaining.</summary>
    public OrderValidationHandler SetNext(OrderValidationHandler next)
    {
        _next = next;
        return next;
    }

    public virtual ValidationResult Handle(Order order)
    {
        // Default: pass to the next handler (or return Ok if at the end)
        return _next is not null ? _next.Handle(order) : ValidationResult.Ok();
    }
}

// ---- Concrete Handlers ----

/// <summary>Rejects orders with an empty cart.</summary>
public class EmptyCartValidationHandler : OrderValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        Console.WriteLine("  [Chain] Rule 1 — Cart not empty...");
        if (order.Items.Count == 0)
            return ValidationResult.Fail("Order has no items in the cart.");
        return base.Handle(order);
    }
}

/// <summary>Rejects orders where requested quantity exceeds available stock.</summary>
public class StockValidationHandler : OrderValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        Console.WriteLine("  [Chain] Rule 2 — Stock availability...");
        foreach (var item in order.Items.OfType<OrderItem>())
        {
            if (item.Product.Stock < item.Quantity)
                return ValidationResult.Fail(
                    $"Insufficient stock for '{item.Product.Name}'. " +
                    $"Requested: {item.Quantity}, Available: {item.Product.Stock}");
        }
        return base.Handle(order);
    }
}

/// <summary>Rejects orders below a minimum order value.</summary>
public class MinimumOrderValueHandler : OrderValidationHandler
{
    private readonly decimal _minimum;

    public MinimumOrderValueHandler(decimal minimum) => _minimum = minimum;

    public override ValidationResult Handle(Order order)
    {
        Console.WriteLine($"  [Chain] Rule 3 — Minimum order value (${_minimum:F2})...");
        if (order.SubTotal < _minimum)
            return ValidationResult.Fail(
                $"Order subtotal ${order.SubTotal:F2} is below the minimum of ${_minimum:F2}.");
        return base.Handle(order);
    }
}
