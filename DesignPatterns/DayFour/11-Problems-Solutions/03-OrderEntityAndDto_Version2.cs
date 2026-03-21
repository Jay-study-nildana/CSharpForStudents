// 03-OrderEntityAndDto.cs
// Purpose: Define Order, OrderLine domain classes and OrderDto read-model with a simple mapper.
// DI/Lifetime: Domain types are plain POCOs. Mapping logic can be injected as a service if needed.
// Testability note: DTO mapping is trivial here; in tests you can assert DTO values.

using System;
using System.Collections.Generic;
using System.Linq;

public class Order
{
    public int Id { get; set; } // used by InMemoryRepository for identity
    public int CustomerId { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
    public string Status { get; set; } = "New";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount() => Lines.Sum(l => l.Quantity * l.UnitPrice);
}

public class OrderLine
{
    public string Sku { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}

// Simple mapper helper
public static class OrderMapper
{
    public static OrderDto ToDto(Order order) => new OrderDto
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        TotalAmount = order.TotalAmount(),
        Status = order.Status
    };
}