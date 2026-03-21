// 05-Proxy_LazyInitialization.cs
// Intent: Proxy that lazily initializes a heavy resource (virtual proxy scenario).
// DI/Lifetime: Proxy can be Transient/Scoped; heavy resource created on demand. Avoid Singleton if heavy resource holds per-request state.
// Testability: Provide a factory or delegate for resource creation to inject test stubs and avoid heavy construction.

using System;
using System.Threading;

// Subject interface
public interface IHeavyResource
{
    string Compute();
}

// Real heavy resource
public class HeavyResource : IHeavyResource
{
    public HeavyResource()
    {
        // Simulate expensive start-up
        Thread.Sleep(1000);
        Console.WriteLine("HeavyResource constructed");
    }

    public string Compute() => "Expensive result";
}

// Lazy proxy
public class HeavyResourceProxy : IHeavyResource
{
    private readonly Func<IHeavyResource> _factory;
    private IHeavyResource _real;

    public HeavyResourceProxy(Func<IHeavyResource> factory) => _factory = factory;

    public string Compute()
    {
        // Lazy init
        _real ??= _factory();
        return _real.Compute();
    }
}

/*
Usage:
var proxy = new HeavyResourceProxy(() => new HeavyResource());
// no heavy construction yet
var result = proxy.Compute(); // constructs HeavyResource here
*/