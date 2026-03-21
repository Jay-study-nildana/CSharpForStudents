// 03_TemplateMethod_OrderProcessing.cs
// Template Method for order processing. Concrete processors override steps.
// Demonstrates invariant algorithm skeleton and overridable steps.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08.Template03
{
    public class Order
    {
        public List<(string sku, decimal Price, int Qty)> Items { get; } = new();
        public string CustomerEmail { get; set; } = "";
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
    }

    public abstract class OrderProcessor
    {
        // Template method - sealed to preserve sequence
        public void ProcessOrder(Order order)
        {
            Validate(order);
            CalculateSubtotal(order);
            ApplyDiscounts(order);
            ApplyTaxes(order);
            Save(order);
            NotifyCustomer(order);
        }

        protected abstract void Validate(Order order);
        protected virtual void CalculateSubtotal(Order order)
        {
            order.Subtotal = order.Items.Sum(i => i.Price * i.Qty);
            Console.WriteLine($"Subtotal calculated: {order.Subtotal:C}");
        }
        protected virtual void ApplyDiscounts(Order order) { } // hook
        protected abstract void ApplyTaxes(Order order);
        protected virtual void Save(Order order) => Console.WriteLine("Order saved (default).");
        protected virtual void NotifyCustomer(Order order) => Console.WriteLine($"Notify {order.CustomerEmail}");
    }

    public class DomesticOrderProcessor : OrderProcessor
    {
        protected override void Validate(Order order)
        {
            if (string.IsNullOrEmpty(order.CustomerEmail)) throw new InvalidOperationException("Missing email");
            Console.WriteLine("Domestic validation passed.");
        }
        protected override void ApplyTaxes(Order order)
        {
            order.Tax = Math.Round(order.Subtotal * 0.06m, 2);
            Console.WriteLine($"Domestic tax applied: {order.Tax:C}");
        }
        protected override void ApplyDiscounts(Order order)
        {
            if (order.Subtotal > 200) { order.Subtotal -= 20; Console.WriteLine("Applied domestic discount -20"); }
        }
    }

    public class InternationalOrderProcessor : OrderProcessor
    {
        protected override void Validate(Order order) { Console.WriteLine("International validation passed."); }
        protected override void ApplyTaxes(Order order)
        {
            order.Tax = Math.Round(order.Subtotal * 0.0m, 2); // VAT handled differently
            Console.WriteLine("International taxes handled separately.");
        }
        protected override void Save(Order order) => Console.WriteLine("Saved to international orders DB.");
    }

    class Program
    {
        static void Main()
        {
            var order = new Order { CustomerEmail = "cust@example.com" };
            order.Items.Add(("SKU1", 50m, 2));
            order.Items.Add(("SKU2", 30m, 1));

            Console.WriteLine("Processing domestic:");
            var domestic = new DomesticOrderProcessor();
            domestic.ProcessOrder(order);

            Console.WriteLine("\nProcessing international:");
            var intl = new InternationalOrderProcessor();
            intl.ProcessOrder(order);
        }
    }
}