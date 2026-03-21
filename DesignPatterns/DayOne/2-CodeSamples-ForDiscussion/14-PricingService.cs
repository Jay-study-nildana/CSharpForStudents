// 14-PricingService.cs
// Consumer of IDiscountStrategy demonstrating runtime swapping of behavior via DI (Strategy pattern).

public class PricingService
{
    private readonly IDiscountStrategy _discountStrategy;

    // Strategy injected via constructor; can be swapped for testing or configuration.
    public PricingService(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal GetFinalPrice(decimal listPrice)
    {
        return _discountStrategy.ApplyDiscount(listPrice);
    }
}