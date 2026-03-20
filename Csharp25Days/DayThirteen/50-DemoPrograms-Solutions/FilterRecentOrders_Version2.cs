// FilterRecentOrders.cs
// Problem: FilterRecentOrders
// Pipeline: orders -> Where(date within last 30 days) -> ToList
// Complexity: O(n)

using System;
using System.Collections.Generic;
using System.Linq;

record Order(int Id, int CustomerId, DateTime Date);

class FilterRecentOrders
{
    static IEnumerable<Order> RecentOrders(IEnumerable<Order> orders, DateTime now)
    {
        var cutoff = now.AddDays(-30);
        return orders.Where(o => o.Date >= cutoff); // deferred
    }

    static void Main()
    {
        var now = DateTime.UtcNow;
        var orders = new[]
        {
            new Order(1, 1, now.AddDays(-10)),
            new Order(2, 2, now.AddDays(-40)),
            new Order(3, 3, now.AddDays(-5))
        };
        var recent = RecentOrders(orders, now).ToList();
        Console.WriteLine(string.Join(", ", recent.Select(o => o.Id))); // 1, 3
    }
}