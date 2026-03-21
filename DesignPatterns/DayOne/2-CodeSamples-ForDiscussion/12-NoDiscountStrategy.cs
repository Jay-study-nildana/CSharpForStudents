// 12-NoDiscountStrategy.cs
// Concrete Strategy: no discount.

public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice) => originalPrice;
}