// 10_Extend_With_New_Strategy.cs
// Add LoyaltyTierDiscount strategy and show minimal changes needed to use it.
// Demonstrates open/closed principle and easy extension via Strategy.

using System;

namespace Day08.Extend10
{
    public interface IPriceStrategy { decimal ApplyDiscount(decimal p); string Name { get; } }

    public class Percentage : IPriceStrategy { public string Name => "Pct15"; private readonly decimal _p = 0.15m; public decimal ApplyDiscount(decimal p) => Math.Round(p * (1 - _p), 2); }

    // New strategy: Loyalty tier (bronze/silver/gold)
    public class LoyaltyTierDiscount : IPriceStrategy
    {
        public string Name => $"Loyalty({_tier})";
        private readonly string _tier;
        public LoyaltyTierDiscount(string tier) => _tier = tier;
        public decimal ApplyDiscount(decimal p) => _tier.ToLower() switch
        {
            "gold" => Math.Round(p * 0.8m, 2),
            "silver" => Math.Round(p * 0.9m, 2),
            _ => p
        };
    }

    public class PricingService
    {
        private IPriceStrategy _strategy;
        public PricingService(IPriceStrategy strat) => _strategy = strat;
        public void SetStrategy(IPriceStrategy s) => _strategy = s;
        public decimal Price(decimal basePrice) => _strategy.ApplyDiscount(basePrice);
    }

    class Program
    {
        static void Main()
        {
            var svc = new PricingService(new Percentage());
            Console.WriteLine($"Percentage price for 100 => {svc.Price(100):C}");

            // Add new strategy with no changes to PricingService
            svc.SetStrategy(new LoyaltyTierDiscount("gold"));
            Console.WriteLine($"Gold loyalty price for 100 => {svc.Price(100):C}");

            svc.SetStrategy(new LoyaltyTierDiscount("silver"));
            Console.WriteLine($"Silver loyalty price for 100 => {svc.Price(100):C}");
        }
    }
}