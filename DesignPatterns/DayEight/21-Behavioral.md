# Day 8 — Behavioral Patterns: Strategy & Template Method

## Objectives
- Learn the Strategy pattern to encapsulate interchangeable algorithms and swap them at runtime.
- Learn the Template Method pattern to define an invariant algorithm skeleton with overridable steps in subclasses.
- Compare when to choose Strategy (favor composition and runtime flexibility) vs Template Method (favor inheritance and controlled extension).
- Demo: pricing/discount strategies (Strategy) and a multi-step order processing pipeline (Template Method).
- Lab & Homework: design strategies for A/B testing and a template-method pipeline for a domain process.

---

## 1. High-level concepts
- Strategy: encapsulate a family of algorithms behind a common interface. Clients hold a reference to the strategy and can swap algorithms at runtime. Use when you want to vary behavior without modifying clients or creating conditional logic.
- Template Method: define the skeleton of an algorithm in a base class and defer some steps to subclasses. Use when an algorithm's structure is fixed but certain steps vary by subclass.

Tradeoffs:
- Strategy uses composition and favors runtime flexibility; more objects but looser coupling.
- Template Method uses inheritance and enforces a sequence; less object proliferation but less runtime flexibility.

---

## 2. Strategy — Pricing/Discount example (C#)
Participants: IPriceStrategy (strategy interface), concrete strategies (NoDiscount, PercentageDiscount, TieredDiscount), PricingService (client).

Note : This was previously already discussed. Check Day One discussions folder. 

```csharp
// Strategy interface
public interface IPriceStrategy
{
    decimal ApplyDiscount(decimal basePrice);
}

// Concrete strategies
public class NoDiscount : IPriceStrategy
{
    public decimal ApplyDiscount(decimal basePrice) => basePrice;
}

public class PercentageDiscount : IPriceStrategy
{
    private readonly decimal _percent; // e.g., 0.15m for 15%
    public PercentageDiscount(decimal percent) => _percent = percent;
    public decimal ApplyDiscount(decimal basePrice) => basePrice * (1 - _percent);
}

public class TieredDiscount : IPriceStrategy
{
    public decimal ApplyDiscount(decimal basePrice) =>
        basePrice > 100 ? basePrice - 20 : basePrice;
}

// Client that composes a strategy
public class PricingService
{
    private IPriceStrategy _strategy;
    public PricingService(IPriceStrategy strategy) => _strategy = strategy;
    public void SetStrategy(IPriceStrategy strategy) => _strategy = strategy;
    public decimal Price(decimal basePrice) => _strategy.ApplyDiscount(basePrice);
}

// Usage
var service = new PricingService(new NoDiscount());
Console.WriteLine(service.Price(120)); // 120
service.SetStrategy(new PercentageDiscount(0.15m));
Console.WriteLine(service.Price(120)); // 102
```

Notes:
- Strategies are small, testable, and interchangeable.
- Strategy enables A/B testing: select different strategies for different user segments at runtime (feature-flag driven, config-driven, or experiment service).

---

## 3. Template Method — Order processing pipeline (C#)
Participants: abstract `OrderProcessor` defines `ProcessOrder()` (template), and concrete processors override steps.

```csharp
public abstract class OrderProcessor
{
    // Template method — sealed to prevent altering the algorithm sequence
    public void ProcessOrder(Order order)
    {
        Validate(order);
        CalculateSubtotal(order);
        ApplyDiscounts(order);       // customizable
        ApplyTaxes(order);           // customizable
        Save(order);
        NotifyCustomer(order);
    }

    protected abstract void Validate(Order order);
    protected virtual void CalculateSubtotal(Order order)
    {
        order.Subtotal = order.Items.Sum(i => i.Price * i.Quantity);
    }
    protected virtual void ApplyDiscounts(Order order) { } // hook
    protected abstract void ApplyTaxes(Order order);
    protected virtual void Save(Order order)
    {
        // default persistence (could be overridden)
        Console.WriteLine("Saved order");
    }
    protected virtual void NotifyCustomer(Order order)
    {
        Console.WriteLine($"Email sent to {order.CustomerEmail}");
    }
}

// Concrete subclass: US order processor
public class UsOrderProcessor : OrderProcessor
{
    protected override void Validate(Order order)
    {
        if (string.IsNullOrEmpty(order.CustomerEmail)) throw new InvalidOperationException("Missing email");
    }
    protected override void ApplyTaxes(Order order) => order.Tax = order.Subtotal * 0.07m;
    protected override void ApplyDiscounts(Order order)
    {
        // subclass-specific discount logic
        if (order.Subtotal > 200) order.Subtotal -= 20;
    }
}
```

Notes:
- Template Method enforces the sequence: clients call `ProcessOrder()` and cannot alter the order of steps.
- Hooks (virtual methods with default implementations) let subclasses optionally override behavior.

---

## 4. Combining Strategy + Template Method
Template Method can use Strategy objects to delegate variable steps—gives both structural control and runtime flexibility.

```csharp
public class StrategyOrderProcessor : OrderProcessor
{
    private readonly IPriceStrategy _discountStrategy;
    public StrategyOrderProcessor(IPriceStrategy discountStrategy) => _discountStrategy = discountStrategy;
    protected override void ApplyDiscounts(Order order)
    {
        order.Subtotal = _discountStrategy.ApplyDiscount(order.Subtotal);
    }
    protected override void Validate(Order order) { /* ... */ }
    protected override void ApplyTaxes(Order order) => order.Tax = order.Subtotal * 0.05m;
}
```

Use case: keep the processing pipeline fixed while swapping discount strategies per experiment or tenant.

---

## 5. Demo idea
- Implement a PricingService with multiple strategies and a web endpoint that selects a strategy by user segment or a query parameter, showing A/B testing.
- Implement an OrderProcessor pipeline where the discount step uses a pluggable strategy; demonstrate changing strategies without changing pipeline code.

---

## 6. Lab & Homework
Lab:
- Implement three discount strategies: NoDiscount, PercentageDiscount, and ContextualDiscount (e.g., loyalty-based).
- Implement an OrderProcessor template with at least five steps (validate, subtotal, discount hook, tax, persistence). Provide two concrete processors (domestic vs international).
- Create a small harness that runs orders through different processors and strategies and records results.

Homework:
- Propose an A/B test where Strategy enables experimentation: design how you would route 50% of traffic to PercentageDiscount vs 50% to TieredDiscount, collect conversion/ARPU metrics, and roll forward the winning strategy.
- Draw UML diagrams for Strategy and Template Method showing participants and relationships.

---

## 7. When to pick which pattern
- Choose Strategy when:
  - You need to switch algorithms at runtime.
  - You want to test multiple implementations (A/B testing).
  - You prefer composition over inheritance.
- Choose Template Method when:
  - The algorithm’s structure is fixed and only steps change.
  - You centralize shared logic and reduce duplicated sequence code across subclasses.

---

## 8. Practical tips for students
- Keep strategy interfaces small and focused (Single Responsibility).
- Favor composition (Strategy) to make code more testable and decoupled.
- In Template Method, seal the template method to prevent accidental change to the algorithm ordering.
- Use dependency injection for strategies to enable configuration-driven experiments.

End of Day 8 summary — implement both patterns and try combining them: a Template Method pipeline that uses Strategies for selected steps is a robust, extensible approach.