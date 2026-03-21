// 13-SeasonalDiscountStrategy.cs
// Concrete Strategy: applies a seasonal percentage discount.

public class SeasonalDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percentage; // 0.15 means 15% off

    public SeasonalDiscountStrategy(decimal percentage)
    {
        _percentage = percentage;
    }

    public decimal ApplyDiscount(decimal originalPrice) => originalPrice * (1 - _percentage);
}