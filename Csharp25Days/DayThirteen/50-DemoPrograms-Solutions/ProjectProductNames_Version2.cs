// ProjectProductNames.cs
// Problem: ProjectProductNames
// Pipeline: products -> Select names -> OrderBy -> ToList
// Complexity: O(n log n) due to sorting.

using System;
using System.Collections.Generic;
using System.Linq;

record Product(int Id, string Name, decimal Price);

class ProjectProductNames
{
    static List<string> GetProductNamesSorted(IEnumerable<Product> products)
    {
        return products
            .Select(p => p.Name)       // projection
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase) // ordering
            .ToList();                 // materialize
    }

    static void Main()
    {
        var products = new[]
        {
            new Product(1, "Shoe", 49.99m),
            new Product(2, "Hat", 19.50m),
            new Product(3, "apple", 1.25m)
        };
        var names = GetProductNamesSorted(products);
        Console.WriteLine(string.Join(", ", names)); // apple, Hat, Shoe
    }
}