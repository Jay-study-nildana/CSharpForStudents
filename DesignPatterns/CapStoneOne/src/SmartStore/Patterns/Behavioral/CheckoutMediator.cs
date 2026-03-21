namespace SmartStore.Patterns.Behavioral;

// ================================================================
// MEDIATOR PATTERN
// ================================================================
// ConcreteCheckoutMediator centralises communication between
// CartComponent, PaymentComponent, and SummaryComponent.
// No component holds a direct reference to any other component.
//
// Intent   : Define an object that encapsulates how a set of objects
//            interact, promoting loose coupling.
// Problem  : Checkout UI has Cart, Payment, and Summary panels that
//            must react to each other's events. Direct coupling creates
//            a mesh of cross-references.
// Solution : Each component notifies only the mediator. The mediator
//            decides what should happen next and whom to forward to.
// ================================================================

public interface ICheckoutMediator
{
    void Notify(object sender, string eventName);
}

// ---- Abstract component base ----
public abstract class CheckoutComponent
{
    protected ICheckoutMediator Mediator;
    protected CheckoutComponent(ICheckoutMediator mediator) => Mediator = mediator;
}

// ---- Concrete Components ----

public class CartComponent : CheckoutComponent
{
    public Order? CurrentOrder { get; private set; }

    public CartComponent(ICheckoutMediator mediator) : base(mediator) { }

    public void LoadOrder(Order order)
    {
        CurrentOrder = order;
        Console.WriteLine($"  [Cart]    Order #{order.Id} loaded — SubTotal: ${order.SubTotal:F2}");
        Mediator.Notify(this, "OrderLoaded");
    }
}

public class PaymentComponent : CheckoutComponent
{
    public bool IsApproved { get; private set; }

    public PaymentComponent(ICheckoutMediator mediator) : base(mediator) { }

    public void ApprovePayment()
    {
        IsApproved = true;
        Console.WriteLine("  [Payment] Payment token approved.");
        Mediator.Notify(this, "PaymentApproved");
    }
}

public class SummaryComponent : CheckoutComponent
{
    public SummaryComponent(ICheckoutMediator mediator) : base(mediator) { }

    public void ShowSummary(Order order)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [Summary] ✔ Order #{order.Id} | {order.Customer.Name} | Total: ${order.Total:F2} | Status: {order.Status}");
        Console.ResetColor();
    }
}

// ---- Concrete Mediator ----
public class ConcreteCheckoutMediator : ICheckoutMediator
{
    public CartComponent    Cart    { get; set; } = null!;
    public PaymentComponent Payment { get; set; } = null!;
    public SummaryComponent Summary { get; set; } = null!;

    public void Notify(object sender, string eventName)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"  [Mediator] Event '{eventName}' from {sender.GetType().Name}");
        Console.ResetColor();

        switch (eventName)
        {
            case "OrderLoaded":
                Console.WriteLine("  [Mediator] → Triggering PaymentComponent to collect payment...");
                Payment.ApprovePayment();
                break;

            case "PaymentApproved" when Cart.CurrentOrder is not null:
                Console.WriteLine("  [Mediator] → Triggering SummaryComponent to show confirmation...");
                Summary.ShowSummary(Cart.CurrentOrder);
                break;
        }
    }
}
