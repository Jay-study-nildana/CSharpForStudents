// 04_Template_With_StrategyStep.cs
// Template Method that uses a Strategy for the ApplyDiscounts step.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08.TemplateWithStrategy04
{
    public interface IPriceStrategy { decimal ApplyDiscount(decimal subtotal); string Name { get; } }

    public class NoDiscount : IPriceStrategy { public string Name => "No"; public decimal ApplyDiscount(decimal subtotal) => subtotal; }
    public class PercentageDiscount : IPriceStrategy { public string Name => "Pct15"; private readonly decimal _p = 0.15m; public decimal ApplyDiscount(decimal subtotal) => Math.Round(subtotal * (1 - _p), 2); }

    public class Order { public List<(string s, decimal p, int q)> Items { get; } = new(); public decimal Subtotal; public decimal Tax; }

    public abstract class OrderProcessor
    {
        private readonly IPriceStrategy _discountStrategy;
        protected OrderProcessor(IPriceStrategy discountStrategy) => _discountStrategy = discountStrategy;
        public void Process(Order order)
        {
            Validate(order);
            CalculateSubtotal(order);
            ApplyDiscounts(order);
            ApplyTaxes(order);
            Save(order);
        }
        protected abstract void Validate(Order order);
        protected virtual void CalculateSubtotal(Order order) => order.Subtotal = order.Items.Sum(i => i.p * i.q);
        protected virtual void ApplyDiscounts(Order order)
        {
            var before = order.Subtotal;
            order.Subtotal = _discountStrategy.ApplyDiscount(order.Subtotal);
            Console.WriteLine($"Applied strategy {_discountStrategy.Name}: {before:C} -> {order.Subtotal:C}");
        }
        protected abstract void ApplyTaxes(Order order);
        protected virtual void Save(Order order) => Console.WriteLine("Saved.");
    }

    public class DefaultProcessor : OrderProcessor
    {
        public DefaultProcessor(IPriceStrategy s) : base(s) { }
        protected override void Validate(Order order) => Console.WriteLine("Valid");
        protected override void ApplyTaxes(Order order) => order.Tax = Math.Round(order.Subtotal * 0.05m, 2);
    }

    class Program
    {
        static void Main()
        {
            var order = new Order();
            order.Items.Add(("A", 100m, 1));
            order.Items.Add(("B", 30m, 2));

            var noDisc = new DefaultProcessor(new NoDiscount());
            noDisc.Process(order);

            var pct = new DefaultProcessor(new PercentageDiscount());
            pct.Process(order);
        }
    }
}