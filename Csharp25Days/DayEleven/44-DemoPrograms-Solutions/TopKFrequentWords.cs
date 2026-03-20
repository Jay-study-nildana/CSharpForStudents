// Problem: TopKFrequentWords
// Return the k most frequent words. Ties broken lexicographically.
// Complexity: O(n + u log u) with LINQ sort; can be improved to O(n log k) with heap.

using System;
using System.Collections.Generic;
using System.Linq;

class TopKFrequentWords
{
    static List<string> TopK(string[] words, int k)
    {
        var counts = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
        foreach (var w in words)
            counts[w] = counts.GetValueOrDefault(w) + 1;

        // Order by frequency desc, then lexicographically asc
        return counts
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .Take(k)
            .Select(kv => kv.Key)
            .ToList();
    }

    static void Main()
    {
        var words = new[] {"apple","banana","apple","cherry","banana","apple","date"};
        var top2 = TopK(words, 2);
        Console.WriteLine(string.Join(", ", top2)); // apple, banana
    }
}