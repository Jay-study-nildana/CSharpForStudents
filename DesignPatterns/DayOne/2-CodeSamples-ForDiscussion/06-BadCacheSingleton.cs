// 06-BadCacheSingleton.cs
// Demonstrates a singleton with mutable shared state (cautionary example).

using System.Collections.Generic;

public class BadCache
{
    private readonly Dictionary<string, string> _store = new();

    // This class might be registered as Singleton, but mutable per-request state is unsafe here.
    public void Put(string key, string value) => _store[key] = value;
    public string Get(string key) => _store.TryGetValue(key, out var v) ? v : null;
}