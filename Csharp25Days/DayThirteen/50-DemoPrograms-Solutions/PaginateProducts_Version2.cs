// PaginateProducts.cs
// Problem: PaginateProducts
// Pipeline: products -> OrderBy(Id) -> Skip((page-1)*pageSize) -> Take(pageSize) -> ToList + Count()
// Complexity: O(n) for counting; page extraction is O(n log n) for sorting then O(pageSize) for page.

using System;
using System.Collections.Generic;
using System.Linq;

record Product(int Id, string Name, decimal Price);

class PaginateProducts
{
    static (List<Product> Page, int TotalCount) GetPage(IEnumerable<Product> products, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
        var ordered = products.OrderBy(p => p.Id);
        var total = ordered.Count(); // materialize count (enumeration)
        var page = ordered.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return (page, total);
    }

    static void Main()
    {
        var products = Enumerable.Range(1, 23).Select(i => new Product(i, $"P{i}", i)).ToList();
        var (page3, total) = GetPage(products, 3, 5);
        Console.WriteLine($"Total: {total}, Page 3: {string.Join(", ", page3.Select(p => p.Id))}");
        // Total: 23, Page 3: 11, 12, 13, 14, 15
    }
}