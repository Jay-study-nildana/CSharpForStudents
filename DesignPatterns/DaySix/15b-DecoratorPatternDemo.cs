// DecoratorPatternDemo.cs
// Console demo of the Decorator Pattern for services.

using System;
using System.Collections.Generic;

// Top-level statements
IService service = new RealService();
service = new LoggingDecorator(service);
service = new CachingDecorator(service);

// REPL: support both getting and setting data by id
while (true)
{
    Console.WriteLine("Enter command (get/set/exit):");
    var cmd = Console.ReadLine();
    if (cmd == "exit") break;

    if (cmd == "get")
    {
        Console.Write("Enter integer id: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var id))
        {
            Console.WriteLine("Invalid id.");
            continue;
        }
        var data = service.GetData(id);
        Console.WriteLine($"Returned: {data}\n");
    }
    else if (cmd == "set")
    {
        Console.Write("Enter integer id: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var id))
        {
            Console.WriteLine("Invalid id.");
            continue;
        }
        Console.Write("Enter data to set: ");
        var value = Console.ReadLine();
        service.SetData(id, value);
        Console.WriteLine("Value set.\n");
    }
    else
    {
        Console.WriteLine("Unknown command.");
    }
}


public interface IService
{
    string GetData(int id);
    void SetData(int id, string data);
}

// Concrete component
public class RealService : IService
{
    // Simple in-memory backing store for demo purposes.
    private readonly Dictionary<int, string> _store = new();

    public string GetData(int id)
    {
        return _store.TryGetValue(id, out var v) ? v : null;
    }

    public void SetData(int id, string data)
    {
        _store[id] = data;
    }
}

// Base decorator
public abstract class ServiceDecorator : IService
{
    protected readonly IService _inner;
    protected ServiceDecorator(IService inner) => _inner = inner;
    public abstract string GetData(int id);
    public abstract void SetData(int id, string data);
}

// Logging decorator
public class LoggingDecorator : ServiceDecorator
{
    public LoggingDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        Console.WriteLine($"[Log] Calling GetData({id})");
        var result = _inner.GetData(id);
        Console.WriteLine($"[Log] Result: {result}");
        return result;
    }

    public override void SetData(int id, string data)
    {
        Console.WriteLine($"[Log] Calling SetData({id}, {data})");
        _inner.SetData(id, data);
        Console.WriteLine($"[Log] SetData completed for {id}");
    }
}

// Caching decorator
public class CachingDecorator : ServiceDecorator
{
    private readonly Dictionary<int, string> _cache = new();
    public CachingDecorator(IService inner) : base(inner) { }
    public override string GetData(int id)
    {
        if (_cache.TryGetValue(id, out var cached)) return cached;
        var result = _inner.GetData(id);
        _cache[id] = result;
        return result;
    }

    public override void SetData(int id, string data)
    {
        // Update the cache immediately so subsequent gets return the new value
        _cache[id] = data;
        // Delegate to inner to ensure the underlying service stores the value
        _inner.SetData(id, data);
    }
}

