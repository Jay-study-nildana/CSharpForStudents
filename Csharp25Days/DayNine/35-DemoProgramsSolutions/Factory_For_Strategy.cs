using System;

class Factory_For_Strategy
{
    // Problem: DiscountFactory producing IDiscountPolicy implementations
    public interface IDiscountPolicy { decimal Apply(decimal total); }

    public class NoDiscount : IDiscountPolicy { public decimal Apply(decimal total) => total; }
    public class PercentageDiscount : IDiscountPolicy
    {
        private readonly decimal _percent;
        public PercentageDiscount(decimal p) => _percent = p;
        public decimal Apply(decimal total) => total * (1 - _percent);
    }

    public static class DiscountFactory
    {
        public static IDiscountPolicy Create(string kind)
        {
            return kind switch
            {
                "none" => new NoDiscount(),
                "10pct" => new PercentageDiscount(0.10m),
                _ => new NoDiscount()
            };
        }
    }

    static void Main()
    {
        var policy = DiscountFactory.Create("10pct");
        Console.WriteLine($"Final: {policy.Apply(200m):C}");

        // Factory centralizes selection logic and returns abstraction for testability.
    }
}