

// 06-BadCacheSingleton.cs
// This file intentionally documents a bad usage pattern: registering a mutable
// object as a singleton in the DI container. The comments below explain, line
// by line, why this is problematic in real applications.

using System; // Console I/O
using System.Collections.Generic; // Dictionary for in-memory store
using Microsoft.Extensions.DependencyInjection; // DI container APIs

// Top-level statements (program entry point) ---------------------------------

// Create a new service collection (the DI container builder).
// Comment: Creating the container here is normal, but what we register matters.
var services = new ServiceCollection();

// Register BadCache as a singleton.
// Comment: This instructs the DI container to create exactly one instance of
// `BadCache` for the entire application lifetime and return the same instance
// on every resolve. That's fine for stateless services, but `BadCache` is
// mutable which makes this registration dangerous.
services.AddSingleton<BadCache>(); // BAD: Singleton with mutable state

// Build the service provider from the configured services. From this point
// forward the container can provide instances according to the lifetimes we
// registered.
var provider = services.BuildServiceProvider();

// Resolve the singleton instance once and reuse it locally in this sample.
// Comment: Any code that resolves `BadCache` (in any part of the app) will get
// the same instance. That creates implicit shared state across unrelated
// components, increasing coupling and making behavior hard to reason about.
var cache = provider.GetRequiredService<BadCache>();

// Simple REPL to interact with the cache ------------------------------------
while (true)
{
    // Prompt the user - left as-is for the demo.
    Console.WriteLine("Enter command (set/get/exit):");

    // Read a command from the console.
    // Comment: User input drives the mutable cache; this demonstrates the
    // stateful nature of the singleton instance.
    var cmd = Console.ReadLine();
    if (cmd == "exit") break; // Exit condition

    if (cmd == "set")
    {
        Console.Write("Key: ");
        var key = Console.ReadLine();
        Console.Write("Value: ");
        var value = Console.ReadLine();

        // Update the shared singleton's state.
        // Comment: Because `BadCache` is a singleton, this mutation is visible
        // to any other consumer that holds a reference to the singleton.
        // That can lead to surprising cross-component interactions.
        cache.Put(key, value);
        Console.WriteLine($"Set {key} = {value}");
    }
    else if (cmd == "get")
    {
        Console.Write("Key: ");
        var key = Console.ReadLine();

        // Read from the shared state.
        // Comment: Retrieval will return values set by any other consumer of
        // the singleton. There's no encapsulation or access control here.
        var value = cache.Get(key);
        Console.WriteLine(value != null ? $"Value: {value}" : "Not found");
    }
}

// BadCache: a small in-memory key/value store -------------------------------
// Comment: The class itself is simple, but because it is mutable and
// registered as a singleton it demonstrates multiple issues described below.
public class BadCache
{
    // Internal mutable state shared for the lifetime of the app.
    // Comment: Exposing mutable state via methods on a singleton invites:
    //  - Race conditions in multi-threaded scenarios (no synchronization here),
    //  - Hidden dependencies because callers implicitly rely on global state,
    //  - Hard-to-write deterministic unit tests, and
    //  - Difficult lifecycle management (when should the cache be cleared?).
    private readonly Dictionary<string, string> _store = new();

    // Put mutates the singleton state.
    // Comment: Mutating a shared object means callers must reason about the
    // global state at any time. If multiple threads call `Put` concurrently,
    // the Dictionary will suffer race conditions and may throw exceptions or
    // become corrupted because it is not thread-safe.
    public void Put(string key, string value) => _store[key] = value;

    // Get reads from the singleton state.
    // Comment: Because there is no locking, concurrent reads and writes can
    // interleave and return inconsistent results. Also, callers cannot assume
    // a clean state between tests or different parts of the app.
    public string Get(string key) => _store.TryGetValue(key, out var v) ? v : null;
}
