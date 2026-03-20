using System;
using System.Collections.Generic;

class DiscountPolicy_Hierarchy
{
    public abstract class DiscountPolicy
    {
        public abstract decimal Apply(decimal total);
    }

    public class NoDiscount : DiscountPolicy
    {
        public override decimal Apply(decimal total) => total;
    }

    public class PercentageDiscount : DiscountPolicy
    {
        private readonly decimal _percent; // e.g., 0.10m for 10%
        public PercentageDiscount(decimal percent) => _percent = percent;
        public override decimal Apply(decimal total) => total * (1 - _percent);
    }

    public class ThresholdDiscount : DiscountPolicy
    {
        private readonly decimal _percent;
        private readonly decimal _threshold;
        public ThresholdDiscount(decimal threshold, decimal percent) { _threshold = threshold; _percent = percent; }
        public override decimal Apply(decimal total) => total >= _threshold ? total * (1 - _percent) : total;
    }

    static void Main()
    {
        var policies = new List<DiscountPolicy>
        {
            new NoDiscount(),
            new PercentageDiscount(0.10m),
            new ThresholdDiscount(100m, 0.2m)
        };

        decimal amount = 150m;
        foreach (var p in policies)
        {
            Console.WriteLine($"{p.GetType().Name} => {p.Apply(amount):C}");
        }

        // Replaces if/switch on policy type with polymorphic behavior on DiscountPolicy.
    }
}