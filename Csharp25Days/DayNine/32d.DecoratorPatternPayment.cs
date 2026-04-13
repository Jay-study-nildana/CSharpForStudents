using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;


// --- Program starts here (top-level statements) ---
Console.WriteLine("Payment decorator (LoggingPayment) demo");
Console.WriteLine();

// Manual composition example
decimal sampleAmount = 42.50m;
Console.WriteLine("Manual composition:");
var manualPayment = new LoggingPayment(new RealPayment(), new ConsoleLogger());
manualPayment.Pay(sampleAmount);
Console.WriteLine();

// Dependency Injection example using Microsoft.Extensions.DependencyInjection
Console.WriteLine("Dependency Injection composition:");
var services = new ServiceCollection();

// Register the logger
services.AddSingleton<ILogger, ConsoleLogger>();

// Register the concrete RealPayment as itself so the factory can resolve it
services.AddTransient<RealPayment>();

// Register IPayment as a decorated instance: resolve RealPayment and ILogger and create LoggingPayment
services.AddTransient<IPayment>(sp =>
{
    var concretepayment = sp.GetRequiredService<RealPayment>(); // inner implementation
    var logger = sp.GetRequiredService<ILogger>();
    return new LoggingPayment(concretepayment, logger);
});

using var provider = services.BuildServiceProvider();
var diPayment = provider.GetRequiredService<IPayment>();
diPayment.Pay(123.45m);
Console.WriteLine();

// Interactive demo: choose manual or DI and enter an amount
Console.WriteLine("Interactive demo:");
Console.WriteLine("  1 = Use DI-registered payment");
Console.WriteLine("  2 = Use manual LoggingPayment");
Console.Write("Selection (1/2): ");
var sel = Console.ReadLine()?.Trim();

Console.Write("Enter amount to pay (e.g. 99.99): ");
var input = Console.ReadLine();
if (!decimal.TryParse(input, out var amount))
{
    Console.WriteLine("Invalid amount. Exiting.");
}
else
{
    IPayment chosen;
    if (sel == "1")
    {
        chosen = provider.GetRequiredService<IPayment>();
    }
    else
    {
        chosen = new LoggingPayment(new RealPayment(), new ConsoleLogger());
    }

    chosen.Pay(amount);
}

Console.WriteLine();
Console.WriteLine("Demo finished.");

// Simple payment interface
public interface IPayment
{
    void Pay(decimal amount);
}

// Simple logger interface (separate from Microsoft.Extensions.Logging for clarity)
public interface ILogger
{
    void Log(string message);
}

// A concrete logger that writes to the console
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

// A concrete payment implementation that performs the actual payment work
public class RealPayment : IPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"RealPayment: Processing payment of {amount:C}...");
        // Simulate some work
        Thread.Sleep(300);
        Console.WriteLine("RealPayment: Payment processed.");
    }
}

// The decorator you provided: logs before and after delegating to the inner IPayment
public class LoggingPayment : IPayment
{
    private readonly IPayment _inner;
    private readonly ILogger _log;
    public LoggingPayment(IPayment inner, ILogger log) 
    { 
        _inner = inner; 
        _log = log; 
    }
    public void Pay(decimal a) 
    { 
        _log.Log($"Paying {a:C}");
        _inner.Pay(a);
        _log.Log("Paid"); 
    }
}

