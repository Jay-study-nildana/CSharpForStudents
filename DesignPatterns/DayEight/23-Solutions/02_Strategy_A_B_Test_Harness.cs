// 02_Strategy_A_B_Test_Harness.cs
// Simple A/B test harness that routes simulated users/orders to two pricing strategies,
// records pseudo "conversions" depending on price threshold, and compares performance.

using System;

namespace Day08.Strategy02
{
    public interface IPriceStrategy
    {
        decimal ApplyDiscount(decimal basePrice);
        string Name { get; }
    }

    public class PercentageDiscount : IPriceStrategy
    {
        public string Name => "Percentage(10%)";
        private readonly decimal _p;
        public PercentageDiscount(decimal p) => _p = p;
        public decimal ApplyDiscount(decimal basePrice) => Math.Round(basePrice * (1 - _p), 2);
    }

    public class TieredDiscount : IPriceStrategy
    {
        public string Name => "Tiered(-15 over 100)";
        public decimal ApplyDiscount(decimal basePrice) => basePrice > 100 ? basePrice - 15 : basePrice;
    }

    class ABTestHarness
    {
        private readonly IPriceStrategy _a;
        private readonly IPriceStrategy _b;
        private readonly Random _rng;
        public ABTestHarness(IPriceStrategy a, IPriceStrategy b, int seed = 123)
        {
            _a = a; _b = b; _rng = new Random(seed);
        }

        // Simulate N users, split 50/50, conversion if price <= user's willingness-to-pay
        public (int conversionsA, int conversionsB) Run(int trials, decimal basePrice)
        {
            int convA = 0, convB = 0;
            for (int i = 0; i < trials; i++)
            {
                decimal userWtp = (decimal)(_rng.NextDouble() * 200); // willingness to pay 0..200
                if (i % 2 == 0) // A
                {
                    var price = _a.ApplyDiscount(basePrice);
                    if (price <= userWtp) convA++;
                }
                else // B
                {
                    var price = _b.ApplyDiscount(basePrice);
                    if (price <= userWtp) convB++;
                }
            }
            return (convA, convB);
        }
    }

    class Program
    {
        static void Main()
        {
            var stratA = new PercentageDiscount(0.10m);
            var stratB = new TieredDiscount();
            var harness = new ABTestHarness(stratA, stratB, seed: 42);
            var result = harness.Run(10000, basePrice: 120m);
            Console.WriteLine($"A ({stratA.Name}) conversions: {result.conversionsA}");
            Console.WriteLine($"B ({stratB.Name}) conversions: {result.conversionsB}");
            Console.WriteLine(result.conversionsA > result.conversionsB
                ? $"Strategy A wins"
                : result.conversionsB > result.conversionsA ? $"Strategy B wins" : "Tie");
        }
    }
}