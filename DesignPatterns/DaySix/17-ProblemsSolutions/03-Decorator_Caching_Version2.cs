// 03-Decorator_Caching.cs
// Intent: Implement a caching decorator that stores results of GetData(int) to avoid repeated inner calls.
// DI/Lifetime: Caching decorator may be Scoped or Singleton depending on cache scope and thread-safety. Use ConcurrentDictionary for thread-safe caches.
// Testability: Inject fake inner that counts calls to assert cache effectiveness.

using System;
using System.Collections.Concurrent;

// IService and RealService assumed from previous files

public class CachingDecoratorSafe : ServiceDecoratorBase
{
    private readonly ConcurrentDictionary<int, string> _cache = new();
    public CachingDecoratorSafe(IService inner) : base(inner) { }

    public override string GetData(int id)
    {
        return _cache.GetOrAdd(id, _ => _inner.GetData(id));
    }
}

// Fake inner for test demonstration
public class CountingService : IService
{
    public int Calls;
    public string GetData(int id) { Calls++; return $"Computed {id}"; }
}

/*
Conceptual test (pseudo-code):
var counting = new CountingService();
var caching = new CachingDecoratorSafe(counting);
var a = caching.GetData(5); // counting.Calls == 1
var b = caching.GetData(5); // counting.Calls == 1 (cached)
*/