// DeferredExecutionPitfall.cs
// Problem: DeferredExecutionPitfall
// Demonstrates deferred execution vs materialization (ToList).
// Complexity: Example demonstrates behavior; pipeline itself is O(n) when enumerated.

using System;
using System.Collections.Generic;
using System.Linq;

class DeferredExecutionPitfall
{
    static IEnumerable<int> FilterByThreshold(IEnumerable<int> source, Func<int> thresholdProvider)
    {
        return source.Where(x => x > thresholdProvider()); // deferred predicate reading external provider
    }

    static void Main()
    {
        var data = Enumerable.Range(1, 5).ToList(); // 1..5
        int threshold = 3;
        // Deferred query
        var deferred = FilterByThreshold(data, () => threshold);
        // Materialized snapshot
        var snapshot = FilterByThreshold(data, () => threshold).ToList();

        Console.WriteLine("Deferred before change: " + string.Join(", ", deferred)); // 4,5
        Console.WriteLine("Snapshot before change: " + string.Join(", ", snapshot)); // 4,5

        threshold = 1; // change external state

        Console.WriteLine("Deferred after change: " + string.Join(", ", deferred)); // 2,3,4,5 (reflects new threshold)
        Console.WriteLine("Snapshot after change: " + string.Join(", ", snapshot)); // 4,5 (unchanged)
    }
}