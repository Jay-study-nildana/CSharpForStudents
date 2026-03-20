// ConcurrentFrequencyCountGeneric.cs
// Problem: ConcurrentFrequencyCountGeneric
// Count frequencies in parallel using ConcurrentDictionary<T,int> and AddOrUpdate.
// Complexity: O(n) work; parallel reduces wall-clock time. No explicit locks needed.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class ConcurrentFrequencyCountGeneric
{
    public static ConcurrentDictionary<T, int> ConcurrentFrequencyCount<T>(IEnumerable<T> items, IEqualityComparer<T>? comparer = null)
    {
        var dict = new ConcurrentDictionary<T, int>(comparer);
        Parallel.ForEach(items, item =>
        {
            dict.AddOrUpdate(item!, 1, (_, old) => old + 1);
        });
        return dict;
    }

    static void Main()
    {
        var words = new[] { "a", "b", "a", "c", "b", "a", "d" };
        var counts = ConcurrentFrequencyCount(words, StringComparer.Ordinal);
        foreach (var kv in counts.OrderByDescending(kv => kv.Value))
            Console.WriteLine($"{kv.Key}: {kv.Value}");
    }
}