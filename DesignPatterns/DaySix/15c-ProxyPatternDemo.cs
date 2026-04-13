// ProxyPatternDemo.cs
// Console demo of the Proxy Pattern with lazy initialization and authorization.

using System;
using System.Threading;

// Top-level statements
bool IsAuthorized()
{
    Console.Write("Enter password for access: ");
    var pwd = Console.ReadLine();
    return pwd == "letmein";
}

IHeavyResource resource = new HeavyResourceProxy(IsAuthorized);

while (true)
{
    Console.WriteLine("Type 'compute' to use resource, or 'exit' to quit:");
    var cmd = Console.ReadLine();
    if (cmd == "exit") break;
    if (cmd == "compute")
    {
        try
        {
            var result = resource.Compute();
            Console.WriteLine($"Result: {result}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access denied: {ex.Message}");
        }
    }
}


public interface IHeavyResource
{
    string Compute();
}

// Real expensive resource
public class HeavyResource : IHeavyResource
{
    public HeavyResource()
    {
        // Simulate expensive startup
        Console.WriteLine("[HeavyResource] Initializing (simulated delay)...");
        Thread.Sleep(2000);
        Console.WriteLine("[HeavyResource] Ready.");
    }
    public string Compute() => "Expensive result";
}

// Proxy with lazy init and simple auth check
public class HeavyResourceProxy : IHeavyResource
{
    private readonly Func<bool> _isAuthorized;
    private HeavyResource _real;

    public HeavyResourceProxy(Func<bool> isAuthorized) => _isAuthorized = isAuthorized;

    public string Compute()
    {
        if (!_isAuthorized()) throw new UnauthorizedAccessException("Not allowed");
        // Lazy initialization (virtual proxy)
        return (_real ??= new HeavyResource()).Compute();
    }
}

