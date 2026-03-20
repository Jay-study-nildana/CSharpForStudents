// TotalQuantityPerProduct.cs
// Problem: TotalQuantityPerProduct
// Pipeline: items -> GroupBy(ProductId) -> Select(ProductId, Sum(Quantity))
// Complexity: O(n)

using System;
using System.Collections.Generic;
using System.Linq;

record OrderItem(int ProductId, int Quantity);

class TotalQuantityPerProduct
{
    static List<(int ProductId, int TotalQty)> Totals(IEnumerable<OrderItem> items)
    {
        return items
            .GroupBy(it => it.ProductId)
            .Select(g => (ProductId: g.Key, TotalQty: g.Sum(it => it.Quantity)))
            .ToList();
    }

    static void Main()
    {
        var items = new[]
        {
            new OrderItem(1, 2),
            new OrderItem(2, 3),
            new OrderItem(1, 1),
            new OrderItem(2, 2)
        };
        var totals = Totals(items);
        foreach (var t in totals) Console.WriteLine($"Product {t.ProductId}: {t.TotalQty}");
        // Product 1: 3
        // Product 2: 5
    }
}