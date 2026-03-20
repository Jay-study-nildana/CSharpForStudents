// CountDistinctGeneric.cs
// Problem: CountDistinctGeneric
// Generic method CountDistinct<T>(IEnumerable<T>, IEqualityComparer<T>?) -> int
// Complexity: O(n) time, O(u) space where u = unique items. No boxing for value types.

using System;
using System.Collections.Generic;

class CountDistinctGeneric
{
    public static int CountDistinct<T>(IEnumerable<T> items, IEqualityComparer<T>? comparer = null)
    {
        var set = new HashSet<T>(comparer);
        foreach (var item in items)
        {
            // HashSet<T> uses equality comparer; value-types are not boxed
            set.Add(item!);
        }
        return set.Count;
    }

    static void Main()
    {
        var nums = new int[] { 1, 2, 2, 3, 1 };
        Console.WriteLine(CountDistinct(nums)); // 3

        var words = new[] { "Apple", "apple", "Banana" };
        Console.WriteLine(CountDistinct(words, StringComparer.OrdinalIgnoreCase)); // 2
    }
}