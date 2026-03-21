// 04-SingletonWithMutableState.cs
// Problem: Show a singleton cache with mutable state, then refactor to a safe alternative.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#region Naive (unsafe) singleton cache
public class NaiveCache
{
    private static NaiveCache _instance;
    private readonly Dictionary<string, string> _store = new();

    private NaiveCache() { }

    public static NaiveCache Instance => _instance ??= new NaiveCache();

    public void Put(string key, string value) => _store[key] = value;
    public string Get(string key) => _store.TryGetValue(key, out var v) ? v : null;
}
/*
Problems:
- Not thread-safe (Dictionary + lazy singleton).
- Mutable shared state can leak between consumers and users.
*/
#endregion

#region Thread-safe alternative
public class SafeCache
{
    // This class can be registered as DI Singleton safely because it uses thread-safe internals.
    private readonly ConcurrentDictionary<string, string> _store = new();

    public void Put(string key, string value) => _store[key] = value;
    public string Get(string key) => _store.TryGetValue(key, out var v) ? v : null;
}
#endregion

/*
Recommendation:
- Prefer using a thread-safe structure (ConcurrentDictionary) and register SafeCache as a DI Singleton
  only if the cache semantics are truly application-wide and thread-safe.
- For per-request or per-user state, avoid singletons and prefer Scoped or Transient services.
*/