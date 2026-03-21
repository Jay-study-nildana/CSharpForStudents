// 11-IDiscountStrategy.cs
// Interface used in the Strategy pattern example to show interchangeable algorithms.

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
}