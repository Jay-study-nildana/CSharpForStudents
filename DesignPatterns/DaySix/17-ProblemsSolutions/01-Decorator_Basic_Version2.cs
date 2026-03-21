// 01-Decorator_Basic.cs
// Intent: Show a basic Decorator that adds logging to an IService implementation.
// DI/Lifetime: RealService can be Transient/Scoped; LoggingDecorator can be Transient/Scoped. Avoid Singleton if decorators keep mutable state.
// Testability: Inject a fake inner service to verify decorator behavior in unit tests.

using System;

public interface IService
{
    string GetData(int id);
}

public class RealService : IService
{
    public string GetData(int id) => $"Real data for {id}";
}

// Base decorator
public abstract class ServiceDecoratorBase : IService
{
    protected readonly IService _inner;
    protected ServiceDecoratorBase(IService inner) => _inner = inner;
    public abstract string GetData(int id);
}

// Logging decorator
public class LoggingDecorator : ServiceDecoratorBase
{
    public LoggingDecorator(IService inner) : base(inner) { }

    public override string GetData(int id)
    {
        Console.WriteLine($"[Log] Enter GetData({id})");
        var result = _inner.GetData(id);
        Console.WriteLine($"[Log] Exit GetData({id}) => {result}");
        return result;
    }
}

// Example client
public class Consumer
{
    private readonly IService _service;
    public Consumer(IService service) => _service = service;
    public void Run()
    {
        var v = _service.GetData(42);
        Console.WriteLine($"Consumed: {v}");
    }
}

/*
Usage example:
var service = new LoggingDecorator(new RealService());
var c = new Consumer(service);
c.Run();
*/