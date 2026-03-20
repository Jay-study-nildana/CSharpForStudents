// GroupCustomersByCountry.cs
// Problem: GroupCustomersByCountry
// Pipeline: customers -> GroupBy(Country) -> ToDictionary(country, sorted list)
// Complexity: O(n log m) where m is average group size due to sorting within groups.

using System;
using System.Collections.Generic;
using System.Linq;

record Customer(int Id, string Name, string Country);

class GroupCustomersByCountry
{
    static Dictionary<string, List<Customer>> GroupByCountry(IEnumerable<Customer> customers)
    {
        return customers
            .GroupBy(c => c.Country)
            .ToDictionary(g => g.Key,
                          g => g.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList());
    }

    static void Main()
    {
        var customers = new[]
        {
            new Customer(1, "Alice", "USA"),
            new Customer(2, "Bob", "Canada"),
            new Customer(3, "Aaron", "USA"),
            new Customer(4, "Beatrice", "Canada")
        };
        var grouped = GroupByCountry(customers);
        foreach (var kv in grouped)
            Console.WriteLine($"{kv.Key}: {string.Join(", ", kv.Value.Select(c => c.Name))}");
        // Canada: Beatrice, Bob
        // USA: Aaron, Alice
    }
}