using System;
using Microsoft.Extensions.DependencyInjection;

// --- Program (top-level statements) ---
Console.WriteLine("Null Object (NullLogger) demo");
Console.WriteLine();

// Manual composition: using NullLogger (no log output)
Console.WriteLine("Manual composition with NullLogger (no logging expected):");
var workerNoLog = new Worker(new NullLogger());
workerNoLog.DoWork();
Console.WriteLine();

// Manual composition: using ConsoleLogger (logs to console)
Console.WriteLine("Manual composition with ConsoleLogger (logging expected):");
var workerWithLog = new Worker(new ConsoleLogger());
workerWithLog.DoWork();
Console.WriteLine();

// Simple DI example: choose logger at runtime
Console.WriteLine("Dependency Injection example: choose logger at runtime.");
Console.Write("Enable logging? (y/N): ");
var input = Console.ReadLine()?.Trim().ToLowerInvariant();
bool enableLogging = input == "y" || input == "yes";

var services = new ServiceCollection();

// Register ILogger once; pick implementation at registration time so consumers only depend on the abstraction.
services.AddSingleton<ILogger>(sp => enableLogging ? new ConsoleLogger() : new NullLogger());

// Register Worker which depends on ILogger
services.AddTransient<Worker>();

using var provider = services.BuildServiceProvider();

Console.WriteLine();
Console.WriteLine($"Resolving Worker from DI. Logging enabled: {enableLogging}");
var diWorker = provider.GetRequiredService<Worker>();
diWorker.DoWork();

Console.WriteLine();
Console.WriteLine("Demo finished. Press Enter to exit.");
Console.ReadLine();

// Logger abstraction
public interface ILogger
{
    void Log(string message);
}

// Null Object: no-op logger
public class NullLogger : ILogger
{
    public void Log(string _) { /* intentionally no-op */ }
}

// Concrete logger that writes to the console
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

// Example consumer which depends on ILogger
public class Worker
{
    private readonly ILogger _logger;
    public Worker(ILogger logger) => _logger = logger;

    public void DoWork()
    {
        _logger.Log("Starting work.");
        // Simulate work
        for (int i = 1; i <= 3; i++)
        {
            _logger.Log($"Working step {i}...");
            System.Threading.Thread.Sleep(150);
        }
        _logger.Log("Work completed.");
    }
}

