// 06_Strategy_Shipping.cs
// Shipping cost strategy examples and a CheckoutService that uses them.

using System;

namespace Day08.Shipping06
{
    public interface IShippingStrategy
    {
        decimal Calculate(decimal weightKg, decimal orderTotal);
        string Name { get; }
    }

    public class FlatRateShipping : IShippingStrategy
    {
        public string Name => "FlatRate";
        public decimal Calculate(decimal weightKg, decimal orderTotal) => 10m;
    }

    public class WeightBasedShipping : IShippingStrategy
    {
        public string Name => "WeightBased";
        public decimal Calculate(decimal weightKg, decimal orderTotal) => Math.Round(2.5m * weightKg, 2);
    }

    public class FreeOverThreshold : IShippingStrategy
    {
        public string Name => "FreeOver150";
        public decimal Calculate(decimal weightKg, decimal orderTotal) => orderTotal >= 150m ? 0m : 12m;
    }

    public class CheckoutService
    {
        private IShippingStrategy _shipping;
        public CheckoutService(IShippingStrategy shipping) => _shipping = shipping;
        public void SetShipping(IShippingStrategy shipping) => _shipping = shipping;
        public decimal CalculateShipping(decimal weightKg, decimal orderTotal) => _shipping.Calculate(weightKg, orderTotal);
    }

    class Program
    {
        static void Main()
        {
            var checkout = new CheckoutService(new FlatRateShipping());
            Console.WriteLine($"FlatRate: {checkout.CalculateShipping(5, 100):C}");

            checkout.SetShipping(new WeightBasedShipping());
            Console.WriteLine($"WeightBased (5kg): {checkout.CalculateShipping(5, 100):C}");

            checkout.SetShipping(new FreeOverThreshold());
            Console.WriteLine($"FreeOverThreshold (order 200): {checkout.CalculateShipping(5, 200):C}");
        }
    }
}