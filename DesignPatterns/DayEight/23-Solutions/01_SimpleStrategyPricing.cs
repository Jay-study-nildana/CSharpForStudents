// 01_SimpleStrategyPricing.cs
// Strategy pattern for pricing/discounts. Demonstrates swapping strategies at runtime.
// Compile/run in a .NET 6+ console app.

using System;

namespace Day08.Strategy01
{
    // Strategy interface
    public interface IPriceStrategy
    {
        decimal ApplyDiscount(decimal basePrice);
        string Name { get; }
    }

    // Concrete strategies
    public class NoDiscount : IPriceStrategy
    {
        public string Name => "NoDiscount";
        public decimal ApplyDiscount(decimal basePrice) => basePrice;
    }

    public class PercentageDiscount : IPriceStrategy
    {
        public string Name => $"PercentageDiscount(%)";
        private readonly decimal _percent;
        public PercentageDiscount(decimal percent) => _percent = percent;
        public decimal ApplyDiscount(decimal basePrice) => Math.Round(basePrice * (1 - _percent), 2);
    }

    public class TieredDiscount : IPriceStrategy
    {
        public string Name => "TieredDiscount";
        public decimal ApplyDiscount(decimal basePrice) => basePrice > 100 ? basePrice - 20 : basePrice;
    }

    // Client
    public class PricingService
    {
        private IPriceStrategy _strategy;
        public PricingService(IPriceStrategy strategy) => _strategy = strategy;
        public void SetStrategy(IPriceStrategy strategy) => _strategy = strategy;
        public decimal Price(decimal basePrice) => _strategy.ApplyDiscount(basePrice);
    }

    class Program
    {
        static void Main()
        {
            var service = new PricingService(new NoDiscount());
            Console.WriteLine($"NoDiscount for 120 => {service.Price(120):C}");

            service.SetStrategy(new PercentageDiscount(0.15m));
            Console.WriteLine($"15% off for 120 => {service.Price(120):C}");

            service.SetStrategy(new TieredDiscount());
            Console.WriteLine($"Tiered for 120 => {service.Price(120):C}");
        }
    }
}