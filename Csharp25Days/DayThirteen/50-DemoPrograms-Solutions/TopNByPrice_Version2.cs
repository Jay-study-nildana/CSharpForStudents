// TopNByPrice.cs
// Problem: TopNByPrice
// Pipeline: products -> OrderByDescending(price) -> ThenBy(name) -> Take(N) -> Select(name,price)
// Complexity: O(n log n) for full sort; for small N you might use a heap for O(n log k).

using System;
using System.Collections.Generic;
using System.Linq;

record Product(int Id, string Name, decimal Price);

class TopNByPrice
{
    static List<(string Name, decimal Price)> TopN(IEnumerable<Product> products, int n)
    {
        return products
            .OrderByDescending(p => p.Price)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .Take(n)
            .Select(p => (p.Name, p.Price))
            .ToList();
    }

    static void Main()
    {
        var products = new[]
        {
            new Product(1, "Shoe", 99.99m),
            new Product(2, "Hat", 19.5m),
            new Product(3, "Coat", 99.99m),
            new Product(4, "Gloves", 25m)
        };
        var top2 = TopN(products, 2);
        foreach (var t in top2) Console.WriteLine($"{t.Name} - {t.Price}");
        // Output: Coat - 99.99 then Shoe - 99.99 (ordered by name for tie)
    }
}