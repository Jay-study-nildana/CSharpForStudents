// 06-GoodCacheScoped.cs
// Demonstrates a recommended cache pattern using DI with a scoped lifetime.
// The comments below explain, line by line, why a scoped lifetime is often the
// better choice for mutable per-operation state compared to a singleton.

using System; // Console I/O
using System.Collections.Generic; // Dictionary for in-memory per-scope storage
using Microsoft.Extensions.DependencyInjection; // DI container APIs

// Top-level statements (program entry) -------------------------------------

// Create a service collection to register application services.
// Comment: The registration choice determines service lifetime semantics,
// which affect sharing, concurrency, and lifetime management.
var services = new ServiceCollection();

// Register GoodCache as scoped.
// Comment: `AddScoped<T>()` creates one instance of `GoodCache` per DI scope.
// This avoids a single global instance while still allowing efficient reuse
// within a logical operation (for example, a web request or a unit of work).
services.AddScoped<GoodCache>(); // GOOD: Scoped cache, not shared across all consumers

// Build the service provider from the configured services.
// Comment: After building the provider, you can create scopes which define
// the lifetime boundaries for scoped services.
var provider = services.BuildServiceProvider();

// Application loop demonstrating a scope-per-interaction model -----------
while (true)
{
    // Create a new scope for each iteration/operation.
    // Comment: Scopes delimit service lifetimes. Any scoped service resolved
    // from `scope.ServiceProvider` will be a fresh instance for that scope.
    // This gives isolation between operations and prevents accidental
    // sharing of mutable state across unrelated flows.
    using var scope = provider.CreateScope();

    // Resolve the GoodCache from the current scope.
    // Comment: Because the cache is scoped, each scope gets its own cache
    // instance. That makes behavior predictable and reduces coupling between
    // different parts of the app.
    var cache = scope.ServiceProvider.GetRequiredService<GoodCache>();

    Console.WriteLine("Enter command (set/get/exit):");
    var cmd = Console.ReadLine();
    if (cmd == "exit") break;

    if (cmd == "set")
    {
        Console.Write("Key: ");
        var key = Console.ReadLine();
        Console.Write("Value: ");
        var value = Console.ReadLine();

        // Mutate the cache instance for this scope only.
        // Comment: Since the instance is scoped, this mutation is local to the
        // current operation. Other operations will not see these changes,
        // which makes reasoning about state easier and avoids global side
        // effects that singletons introduce.
        cache.Put(key, value);
        Console.WriteLine($"Set {key} = {value}");
    }
    else if (cmd == "get")
    {
        Console.Write("Key: ");
        var key = Console.ReadLine();

        // Read from the per-scope cache.
        // Comment: Results are deterministic within the scope. Tests and
        // calling code can set up a scope, populate the cache, and assert
        // behavior without worrying about leftover data from other scopes.
        var value = cache.Get(key);
        Console.WriteLine(value != null ? $"Value: {value}" : "Not found");
    }
}

// GoodCache: an in-memory cache that is intended to be scoped per operation.
// Comment: The implementation is the same as a simple cache, but the lifetime
// provided by DI determines whether it behaves as a safe per-operation store
// (scoped) or as a dangerous shared global (singleton).
public class GoodCache
{
    // Internal mutable state, but life is bound to the scope in which the
    // instance was created. This reduces the surface area of shared mutable
    // state compared to a singleton.
    private readonly Dictionary<string, string> _store = new();

    // Put mutates state for this scope only.
    // Comment: If concurrent access within the same scope is possible, you
    // still need synchronization. Scoped lifetime only guarantees instance
    // isolation between scopes, not thread-safety inside a scope.
    public void Put(string key, string value) => _store[key] = value;

    // Get reads state from this scope's cache.
    // Comment: Because the dictionary is private to the scoped instance, test
    // setup and cleanup is simpler and there are fewer surprises from other
    // parts of the application mutating the same store.
    public string Get(string key) => _store.TryGetValue(key, out var v) ? v : null;
}
