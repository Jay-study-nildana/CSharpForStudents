// LoggerServiceSingletonDemo.cs
// Console demo comparing manual singleton and DI-managed singleton for ILoggerService.

using System;
using Microsoft.Extensions.DependencyInjection;

/*
Comparison:
- Manual Singleton: You write the singleton logic, control instantiation, and access via Instance property.
- DI Singleton: The DI container manages the singleton's lifecycle and dependencies. You request the service from the container.
- DI is preferred in modern apps for testability, flexibility, and separation of concerns.
*/

// Top-level statements
var services = new ServiceCollection();
// Register DISingletonLogger as a singleton for ILoggerService
services.AddSingleton<ILoggerService, DISingletonLogger>();
var provider = services.BuildServiceProvider();

Console.WriteLine("Manual Singleton and DI Singleton Demo\n");

while (true)
{
    Console.WriteLine("Choose logger: manual, di, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;
    Console.Write("Enter message: ");
    var msg = Console.ReadLine();

    if (choice == "manual")
    {
        // Manual singleton usage
        // You control the instance lifecycle
        ManualSingletonLogger.Instance.Log(msg);
    }
    else if (choice == "di")
    {
        // DI-managed singleton usage
        // The DI container manages the instance lifecycle
        var logger = provider.GetRequiredService<ILoggerService>();
        logger.Log(msg);
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}




public interface ILoggerService
{
    void Log(string message);
}

// Manual singleton implementation
public class ManualSingletonLogger : ILoggerService
{
    private static ManualSingletonLogger _instance;
    private ManualSingletonLogger() { }
    public static ManualSingletonLogger Instance => _instance ??= new ManualSingletonLogger();
    public void Log(string message) => Console.WriteLine($"[ManualSingleton] {message}");
}

// DI-managed singleton implementation
public class DISingletonLogger : ILoggerService
{
    public void Log(string message) => Console.WriteLine($"[DI Singleton] {message}");
}

