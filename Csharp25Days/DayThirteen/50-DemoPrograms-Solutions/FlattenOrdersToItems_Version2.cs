// FlattenOrdersToItems.cs
// Problem: FlattenOrdersToItems
// Pipeline: orders -> SelectMany(Items) -> Select(OrderId, ProductId, Quantity)
// Complexity: O(totalItems)

using System;
using System.Collections.Generic;
using System.Linq;

record OrderItem(int ProductId, int Quantity);
record Order(int Id, int CustomerId, DateTime Date, List<OrderItem> Items);

class FlattenOrdersToItems
{
    static IEnumerable<(int OrderId, int ProductId, int Quantity)> Flatten(IEnumerable<Order> orders)
    {
        return orders.SelectMany(o => o.Items.Select(it => (o.Id, it.ProductId, it.Quantity)));
    }

    static void Main()
    {
        var orders = new[]
        {
            new Order(1, 1, DateTime.UtcNow, new List<OrderItem>{ new OrderItem(1,2), new OrderItem(2,1) }),
            new Order(2, 2, DateTime.UtcNow, new List<OrderItem>{ new OrderItem(1,1) })
        };
        foreach (var entry in Flatten(orders))
            Console.WriteLine($"Order {entry.OrderId} - Product {entry.ProductId}: {entry.Quantity}");
    }
}