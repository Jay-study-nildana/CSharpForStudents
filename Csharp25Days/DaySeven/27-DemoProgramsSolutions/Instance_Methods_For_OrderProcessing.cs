using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Problem: Instance_Methods_For_OrderProcessing
/// Order holds items as instance state and exposes instance methods to operate on that state.
/// </summary>
class Instance_Methods_For_OrderProcessing
{
    public class OrderItem { public string Name; public decimal Price; public int Qty; }

    public class Order
    {
        private readonly List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        public void AddItem(string name, decimal price, int qty = 1)
        {
            if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
            _items.Add(new OrderItem { Name = name, Price = price, Qty = qty });
        }

        public decimal Total(decimal taxRate = 0m)
        {
            var subtotal = _items.Sum(i => i.Price * i.Qty);
            return subtotal + subtotal * taxRate;
        }
    }

    static void Main()
    {
        var order = new Order();
        order.AddItem("Pen", 1.2m, 3);
        order.AddItem("Book", 12.5m);
        Console.WriteLine($"Items: {order.Items.Count}, Total with 10% tax: {order.Total(0.10m):C}");
        Console.WriteLine("Instance methods operate on per-order state and are easy to test per instance.");
    }
}