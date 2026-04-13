using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

// --- Program (top-level statements) ---
Console.WriteLine("CompositeNotifier demo");
Console.WriteLine();

// Manual composition example
Console.WriteLine("Manual composition:");
var notificationsFilePath = Path.Combine(Environment.CurrentDirectory, "notifications.txt");
var manualCompositeNotifier = new CompositeNotifier(new INotifier[]
{
    new ConsoleNotifier(),
    new FileNotifier(notificationsFilePath)
});
manualCompositeNotifier.Notify("Manual: Hello subscribers!");
Console.WriteLine($"Wrote to console and appended to file: {notificationsFilePath}");
Console.WriteLine();

// Dependency Injection example
Console.WriteLine("DI composition using IServiceCollection:");
var services = new ServiceCollection();

// Register multiple INotifier implementations.
// Note: we register implementations as INotifier so the container will provide them in IEnumerable<INotifier>.
services.AddTransient<INotifier, ConsoleNotifier>();
services.AddTransient<INotifier>(sp => new FileNotifier(notificationsFilePath));

// Register CompositeNotifier as a concrete type that depends on IEnumerable<INotifier>
services.AddTransient<CompositeNotifier>();

// Register a service that depends on an INotifier; resolve it with the composite manually
services.AddTransient<NotifyingService>();

using var provider = services.BuildServiceProvider();

// Resolve the CompositeNotifier and call it
var diCompositeNotifier = provider.GetRequiredService<CompositeNotifier>();
diCompositeNotifier.Notify("DI: Hello subscribers!");

// Alternatively, resolve NotifyingService and pass the composite explicitly
var notifyingService = new NotifyingService(diCompositeNotifier);
notifyingService.DoSomethingAndNotify("work completed via DI");

Console.WriteLine($"DI: Also appended to file: {notificationsFilePath}");
Console.WriteLine();

// Interactive usage
Console.WriteLine("Interactive: enter a message to notify (empty to skip):");
var message = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(message))
{
    // Use DI-resolved composite
    var service = provider.GetRequiredService<NotifyingService>();
    service.DoSomethingAndNotify(message);
    Console.WriteLine("Notification sent.");
}
else
{
    Console.WriteLine("No message entered. Skipping.");
}

Console.WriteLine();
Console.WriteLine("Demo finished. Press Enter to exit.");
Console.ReadLine();

// Notification contract
public interface INotifier
{
    void Notify(string message);
}

// Concrete notifier that writes to the console
public class ConsoleNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine($"[Console] {message}");
}

// Concrete notifier that appends messages to a file
public class FileNotifier : INotifier
{
    private readonly string _path;
    public FileNotifier(string path) => _path = path;
    public void Notify(string message)
    {
        var line = $"[File {DateTime.UtcNow:O}] {message}{Environment.NewLine}";
        File.AppendAllText(_path, line);
    }
}

// Your CompositeNotifier (focus of the example)
public class CompositeNotifier : INotifier
{
    private readonly IEnumerable<INotifier> _children;
    public CompositeNotifier(IEnumerable<INotifier> children) => _children = children;
    public void Notify(string m)
    {
        foreach (var c in _children)
            c.Notify(m);
    }
}

// A tiny sample service that uses INotifier
public class NotifyingService
{
    private readonly INotifier _notifier;
    public NotifyingService(INotifier notifier) => _notifier = notifier;
    public void DoSomethingAndNotify(string info)
    {
        // do some work...
        _notifier.Notify($"Event occurred: {info}");
    }
}

