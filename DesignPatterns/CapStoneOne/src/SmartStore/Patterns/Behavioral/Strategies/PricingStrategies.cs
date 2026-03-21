namespace SmartStore.Patterns.Behavioral.Strategies;

// ================================================================
// STRATEGY PATTERN
// ================================================================
// Defines a family of pricing algorithms, encapsulates each one,
// and makes them interchangeable at runtime.
//
// Intent   : Define a family of algorithms, put each in its own class,
//            and make their objects interchangeable.
// Problem  : Pricing rules differ by customer tier and promotion.
//            Embedding all rules in one class creates a tangled mess.
// Solution : Each rule is an IPricingStrategy. Client code receives the
//            correct strategy (injected / selected via Factory or DI).
//            Swapping strategies requires no change to checkout logic.
// ================================================================

/// <summary>Regular customers receive no discount.</summary>
public class RegularPricingStrategy : IPricingStrategy
{
    public string Name => "Regular (no discount)";
    public decimal CalculateDiscount(Order order) => 0m;
}

/// <summary>Seasonal sale: a fixed percentage off the subtotal.</summary>
public class DiscountPricingStrategy : IPricingStrategy
{
    private readonly decimal _percentOff;

    public DiscountPricingStrategy(decimal percentOff) => _percentOff = percentOff;

    public string Name => $"Seasonal {_percentOff}% Off";
    public decimal CalculateDiscount(Order order) =>
        Math.Round(order.SubTotal * (_percentOff / 100m), 2);
}

/// <summary>VIP customers receive 15% off orders exceeding $100.</summary>
public class VipPricingStrategy : IPricingStrategy
{
    public string Name => "VIP 15% (orders > $100)";

    public decimal CalculateDiscount(Order order)
    {
        if (order.Customer.Type == CustomerType.Vip && order.SubTotal > 100m)
            return Math.Round(order.SubTotal * 0.15m, 2);
        return 0m;
    }
}
