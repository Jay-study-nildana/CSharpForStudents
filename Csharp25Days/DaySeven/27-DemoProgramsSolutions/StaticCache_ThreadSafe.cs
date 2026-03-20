using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Problem: StaticCache_ThreadSafe
/// Demonstrates a static ConcurrentDictionary cache and an instance cache with lock.
/// </summary>
class StaticCache_ThreadSafe
{
    // Static thread-safe cache
    public static class GlobalCache
    {
        private static ConcurrentDictionary<string, string> _cache = new();
        public static string GetOrAdd(string key, Func<string> factory) =>
            _cache.GetOrAdd(key, _ => factory());
    }

    // Instance cache with lock
    public class InstanceCache
    {
        private readonly Dictionary<string, string> _cache = new();
        private readonly object _lock = new();
        public string GetOrAdd(string key, Func<string> factory)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var v)) return v;
                v = factory();
                _cache[key] = v;
                return v;
            }
        }
    }

    static void Main()
    {
        // Concurrent demo for static cache
        Parallel.For(0, 10, i =>
        {
            var key = "k";
            var val = GlobalCache.GetOrAdd(key, () => "value-" + Guid.NewGuid());
            Console.WriteLine($"Static cache thread {Task.CurrentId}: {val}");
        });

        // Instance cache demo
        var inst = new InstanceCache();
        Console.WriteLine("Instance cache value: " + inst.GetOrAdd("x", () => "inst-" + Guid.NewGuid()));

        Console.WriteLine("Tradeoffs: static cache is shared globally; instance cache is per-object (better isolation).");
    }
}