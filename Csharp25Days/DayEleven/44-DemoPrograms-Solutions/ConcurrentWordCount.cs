// Problem: ConcurrentWordCount
// Compute word frequencies in parallel from many lines using ConcurrentDictionary.
// Complexity: O(n) total work; concurrency reduces wall-clock time; memory O(u).

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

class ConcurrentWordCount
{
    static ConcurrentDictionary<string,int> CountWordsParallel(IEnumerable<string> lines)
    {
        var dict = new ConcurrentDictionary<string,int>(StringComparer.OrdinalIgnoreCase);
        Parallel.ForEach(lines, line =>
        {
            var parts = line.Split(new[]{' ', '\t', ',', '.', '!', '?'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
                dict.AddOrUpdate(p, 1, (_, old) => old + 1);
        });
        return dict;
    }

    static void Main()
    {
        var lines = new[]{
            "apple banana apple",
            "banana cherry apple",
            "cherry apple"
        };
        var counts = CountWordsParallel(lines);
        foreach (var kv in counts.OrderByDescending(kv => kv.Value))
            Console.WriteLine($"{kv.Key}: {kv.Value}");
    }
}