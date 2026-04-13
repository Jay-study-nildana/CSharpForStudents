// TemplatePatternDemo.cs
// Console demo of the Template Method Pattern for order processing.

using System;
using System.Linq;
using System.Collections.Generic;

// Top-level statements
while (true)
{
    Console.WriteLine("Enter customer email (or 'exit' to quit):");
    var email = Console.ReadLine();
    if (email == "exit") break;
    var order = new Order { CustomerEmail = email };
    while (true)
    {
        Console.WriteLine("Add item? (y/n)");
        var ans = Console.ReadLine();
        if (ans != "y") break;
        Console.Write("Item name: ");
        var name = Console.ReadLine();
        Console.Write("Price: ");
        decimal price = decimal.Parse(Console.ReadLine());
        Console.Write("Quantity: ");
        int qty = int.Parse(Console.ReadLine());
        order.Items.Add(new OrderItem { Name = name, Price = price, Quantity = qty });
    }
    var processor = new UsOrderProcessor();
    try
    {
        processor.ProcessOrder(order);
        Console.WriteLine($"Subtotal: {order.Subtotal:C}, Tax: {order.Tax:C}, Total: {order.Total:C}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Order failed: {ex.Message}\n");
    }
}


public class Order
{
    public string CustomerEmail { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total => Subtotal + Tax;
}

public class OrderItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public abstract class OrderProcessor
{
    // Template method — sealed to prevent altering the algorithm sequence
    public void ProcessOrder(Order order)
    {
        Validate(order);
        CalculateSubtotal(order);
        ApplyDiscounts(order);       // customizable
        ApplyTaxes(order);           // customizable
        Save(order);
        NotifyCustomer(order);
    }

    protected abstract void Validate(Order order);
    protected virtual void CalculateSubtotal(Order order)
    {
        order.Subtotal = order.Items.Sum(i => i.Price * i.Quantity);
    }
    protected virtual void ApplyDiscounts(Order order) { } // hook
    protected abstract void ApplyTaxes(Order order);
    protected virtual void Save(Order order)
    {
        // default persistence (could be overridden)
        Console.WriteLine("Saved order");
    }
    protected virtual void NotifyCustomer(Order order)
    {
        Console.WriteLine($"Email sent to {order.CustomerEmail}");
    }
}

// Concrete subclass: US order processor
public class UsOrderProcessor : OrderProcessor
{
    protected override void Validate(Order order)
    {
        if (string.IsNullOrEmpty(order.CustomerEmail)) throw new InvalidOperationException("Missing email");
    }
    protected override void ApplyTaxes(Order order) => order.Tax = order.Subtotal * 0.07m;
    protected override void ApplyDiscounts(Order order)
    {
        // subclass-specific discount logic
        if (order.Subtotal > 200) order.Subtotal -= 20;
    }
}

