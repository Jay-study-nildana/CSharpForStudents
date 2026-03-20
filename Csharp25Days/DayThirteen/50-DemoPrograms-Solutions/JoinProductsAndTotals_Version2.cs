// JoinProductsAndTotals.cs
// Problem: JoinProductsAndTotals
// Pipeline: products join totals on ProductId -> Select(Name, TotalQty, Revenue)
// Complexity: O(p + t + join) - Join uses hashing internally for LINQ-to-Objects (roughly O(n + m))

using System;
using System.Collections.Generic;
using System.Linq;

record Product(int Id, string Name, decimal Price);
record Total(int ProductId, int TotalQty);

class JoinProductsAndTotals
{
    static IEnumerable<object> ProductTotals(IEnumerable<Product> products, IEnumerable<Total> totals)
    {
        return from p in products
               join t in totals on p.Id equals t.ProductId
               select new { p.Name, t.TotalQty, Revenue = t.TotalQty * p.Price };
    }

    static void Main()
    {
        var products = new[]
        {
            new Product(1, "Shoe", 50m),
            new Product(2, "Hat", 20m)
        };
        var totals = new[]
        {
            new Total(1, 3),
            new Total(2, 5)
        };
        foreach (var pt in ProductTotals(products, totals))
            Console.WriteLine(pt);
        // { Name = Shoe, TotalQty = 3, Revenue = 150.00 }
        // { Name = Hat, TotalQty = 5, Revenue = 100.00 }
    }
}