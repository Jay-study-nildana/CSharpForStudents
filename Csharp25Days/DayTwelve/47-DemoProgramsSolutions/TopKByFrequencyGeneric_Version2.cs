// TopKByFrequencyGeneric.cs
// Problem: TopKByFrequencyGeneric
// Return top-k most frequent items. Uses Dictionary<T,int> + min-heap PriorityQueue.
// Complexity: O(n + u log k) time, O(u) space where u = distinct items.

using System;
using System.Collections.Generic;
using System.Linq;

class TopKByFrequencyGeneric
{
    public static List<T> TopKByFrequency<T>(IEnumerable<T> items, int k, IEqualityComparer<T>? comparer = null)
    {
        if (k <= 0) return new List<T>();
        var counts = new Dictionary<T, int>(comparer);
        foreach (var item in items)
            counts[item!] = counts.GetValueOrDefault(item!) + 1;

        // Use min-heap of size k (PriorityQueue available in .NET 6+)
        var pq = new PriorityQueue<T, int>(); // min-heap by priority
        foreach (var kv in counts)
        {
            pq.Enqueue(kv.Key, kv.Value);
            if (pq.Count > k) pq.Dequeue();
        }

        var result = new List<T>();
        while (pq.Count > 0) result.Add(pq.Dequeue());
        result.Reverse(); // highest freq first
        return result;
    }

    static void Main()
    {
        var words = new[] { "a", "b", "a", "c", "b", "a", "d" };
        var top2 = TopKByFrequency(words, 2, StringComparer.Ordinal);
        Console.WriteLine(string.Join(", ", top2)); // a, b
    }
}