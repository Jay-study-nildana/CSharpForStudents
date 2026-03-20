// DelegateBasedStrategyInjection.cs
// Solution: service that accepts a Func<Order,decimal> strategy and demonstrates swapping strategies.

using System;

namespace Day19.Solutions
{
    public record Order(Guid Id, decimal Subtotal, int ItemCount);

    public class PricingService
    {
        private readonly Func<Order, decimal> _priceCalculator;
        public PricingService(Func<Order, decimal> priceCalculator) => _priceCalculator = priceCalculator;

        public decimal ComputeTotal(Order order) => _priceCalculator(order);
    }

    public static class DelegateBasedStrategyInjection
    {
        public static void Run()
        {
            var order = new Order(Guid.NewGuid(), 100m, 3);

            // Standard strategy
            Func<Order, decimal> standard = o => o.Subtotal;

            // Discount strategy
            Func<Order, decimal> discount = o => o.Subtotal * 0.9m;

            var svcStandard = new PricingService(standard);
            var svcDiscount = new PricingService(discount);

            Console.WriteLine($"Standard total: {svcStandard.ComputeTotal(order)}"); // 100
            Console.WriteLine($"Discount total: {svcDiscount.ComputeTotal(order)}"); // 90
        }
    }
}