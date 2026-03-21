namespace SmartStore.Patterns.Structural;

// ================================================================
// FACADE PATTERN
// ================================================================
// CheckoutFacade provides one simple method (Checkout) that hides the
// complexity of six collaborating subsystems.
//
// Intent   : Provide a unified, simplified interface to a set of
//            interfaces in a subsystem.
// Problem  : Checkout involves validation, pricing, payment, persistence,
//            observation, and business rules — spread across many types.
//            Callers should not need to orchestrate all of that.
// Solution : The facade knows the correct sequence and delegates to each
//            subsystem. Callers interact only with Checkout().
//
// Subsystems used:
//   1. Chain of Responsibility  — order validation
//   2. Strategy                 — pricing / discount calculation
//   3. Adapter                  — legacy payment processing
//   4. Repository + Unit of Work — persistence
//   5. Observer (EventManager)  — post-checkout notifications
// ================================================================
public class CheckoutFacade
{
    private readonly IUnitOfWork _uow;
    private readonly IPaymentGateway _payment;
    private readonly IPricingStrategy _pricing;
    private readonly OrderEventManager _eventManager;
    private readonly OrderValidationHandler _validationChain;

    public CheckoutFacade(
        IUnitOfWork uow,
        IPaymentGateway payment,
        IPricingStrategy pricing,
        OrderEventManager eventManager,
        OrderValidationHandler validationChain)
    {
        _uow           = uow;
        _payment       = payment;
        _pricing       = pricing;
        _eventManager  = eventManager;
        _validationChain = validationChain;
    }

    /// <summary>
    /// Runs the full checkout pipeline. Returns true on success.
    /// </summary>
    public bool Checkout(Order order)
    {
        Console.WriteLine("\n  [Facade] ── Starting checkout pipeline ──");

        // Step 1 — Validate (Chain of Responsibility)
        var validation = _validationChain.Handle(order);
        if (!validation.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [Facade] Validation failed: {validation.ErrorMessage}");
            Console.ResetColor();
            return false;
        }

        // Step 2 — Apply pricing strategy (Strategy)
        order.Discount = _pricing.CalculateDiscount(order);
        Console.WriteLine($"  [Facade] Pricing '{_pricing.Name}' — Discount: ${order.Discount:F2}");

        // Step 3 — Process payment (Adapter wraps legacy gateway)
        var paid = _payment.ProcessPayment(order.Customer.Id.ToString(), order.Total);
        if (!paid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  [Facade] Payment declined — checkout aborted.");
            Console.ResetColor();
            return false;
        }
        Console.WriteLine("  [Facade] Payment approved.");

        // Step 4 — Persist (Repository + Unit of Work)
        order.Status = OrderStatus.Confirmed;
        _uow.Orders.Update(order);
        _uow.Commit();

        // Step 5 — Notify observers (Observer)
        _eventManager.Notify(order, "OrderConfirmed");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  [Facade] ── Checkout complete ──");
        Console.ResetColor();
        return true;
    }
}
