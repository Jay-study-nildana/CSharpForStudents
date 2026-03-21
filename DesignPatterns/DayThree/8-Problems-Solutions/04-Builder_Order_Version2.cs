// 04-Builder_Order.cs
// Builder for an immutable Order object with fluent API.
// DI/Lifetime: Builders are typically transient (per construction).
// Testability: Use builder in tests to create varied orders easily.

using System;
using System.Collections.Generic;

public class Order
{
    public Guid Id { get; }
    public string Customer { get; }
    public IReadOnlyList<OrderLine> Lines { get; }
    public decimal Discount { get; }

    internal Order(Guid id, string customer, List<OrderLine> lines, decimal discount)
    {
        Id = id; Customer = customer; Lines = lines.AsReadOnly(); Discount = discount;
    }
}

public class OrderLine
{
    public string Sku { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }

    public OrderLine(string sku, int qty, decimal price) { Sku = sku; Quantity = qty; UnitPrice = price; }
}

public class OrderBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _customer;
    private readonly List<OrderLine> _lines = new();
    private decimal _discount = 0m;

    public OrderBuilder ForCustomer(string customer) { _customer = customer; return this; }
    public OrderBuilder AddLine(string sku, int qty, decimal unitPrice) { _lines.Add(new OrderLine(sku, qty, unitPrice)); return this; }
    public OrderBuilder WithDiscount(decimal percent) { _discount = percent; return this; }

    public Order Build()
    {
        // Invariants: Customer required; at least one line
        if (string.IsNullOrWhiteSpace(_customer)) throw new InvalidOperationException("Customer is required.");
        if (_lines.Count == 0) throw new InvalidOperationException("Order must contain at least one line.");

        return new Order(_id, _customer, new List<OrderLine>(_lines), _discount);
    }
}

/*
Build returns an immutable snapshot (Order). Builder is transient and used per construction.
*/