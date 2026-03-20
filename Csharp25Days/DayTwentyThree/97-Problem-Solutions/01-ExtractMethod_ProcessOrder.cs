// 01-ExtractMethod_ProcessOrder.cs
// Refactored version: ProcessOrder uses small methods (Validate, CalculateTotals, Persist).
using System;
using System.Linq;
using System.Collections.Generic;

public class Order { public List<OrderItem> Items = new(); public decimal Subtotal; public decimal Tax; public decimal Total; }
public class OrderItem { public decimal Price; public int Quantity; }
public interface IDb { void Add(Order o); void SaveChanges(); }
public class OrderService
{
    private readonly IDb _db;
    public OrderService(IDb db) => _db = db;

    public void ProcessOrder(Order order)
    {
        Validate(order);
        CalculateTotals(order);
        Persist(order);
    }

    private static void Validate(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Items == null || order.Items.Count == 0) throw new InvalidOperationException("Empty order.");
    }

    private static void CalculateTotals(Order order)
    {
        order.Subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        order.Tax = order.Subtotal * 0.08m;
        order.Total = order.Subtotal + order.Tax;
    }

    private void Persist(Order order)
    {
        _db.Add(order);
        _db.SaveChanges();
    }
}