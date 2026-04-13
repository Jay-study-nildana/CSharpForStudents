using System;
using Microsoft.Extensions.DependencyInjection;

// --- Program (top-level statements) ---
Console.WriteLine("LegacyAdapter (Adapter Pattern) demo");
Console.WriteLine();

// Manual composition: create LegacySender and wrap it with LegacyAdapter
Console.WriteLine("Manual composition:");
var legacy = new LegacySender();
var adapter = new LegacyAdapter(legacy);
var manualService = new NotificationService(adapter);
manualService.NotifyUser("alice@example.com", "Hello Alice (manual)!");
Console.WriteLine();

// Dependency Injection composition: register LegacySender and adapt it when resolving IEmailSender
Console.WriteLine("Dependency Injection composition:");
var services = new ServiceCollection();

// Register the legacy concrete so factories can resolve it
services.AddSingleton<LegacySender>();

// Register IEmailSender by creating an adapter that wraps the registered LegacySender
services.AddTransient<IEmailSender>(sp =>
{
    var legacySvc = sp.GetRequiredService<LegacySender>();
    return new LegacyAdapter(legacySvc);
});

// Register the NotificationService
services.AddTransient<NotificationService>();

using var provider = services.BuildServiceProvider();

var diService = provider.GetRequiredService<NotificationService>();
diService.NotifyUser("bob@example.com", "Hello Bob (DI)!");
Console.WriteLine();

// Interactive demo
Console.WriteLine("Interactive: enter recipient email (or empty to exit):");
var email = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(email))
{
    Console.Write("Enter message: ");
    var msg = Console.ReadLine() ?? string.Empty;
    var svc = provider.GetRequiredService<NotificationService>();
    svc.NotifyUser(email, msg);
}
else
{
    Console.WriteLine("No email provided. Exiting interactive demo.");
}

Console.WriteLine();
Console.WriteLine("Demo finished. Press Enter to exit.");
Console.ReadLine();

// The target interface expected by modern code
public interface IEmailSender
{
    void Send(string to, string body);
}

// A legacy third-party/old API we must adapt to
public class LegacySender
{
    // Legacy API uses a single string message in a particular format
    public void SendMsg(string message)
    {
        // Simulate sending via legacy system
        Console.WriteLine($"[LegacySender] Sending via legacy system: {message}");
    }
}

// Your adapter that adapts LegacySender to IEmailSender
public class LegacyAdapter : IEmailSender
{
    private readonly LegacySender _legacy;
    public LegacyAdapter(LegacySender legacy) { _legacy = legacy; }
    public void Send(string to, string body) => _legacy.SendMsg($"{to}:{body}");
}

// A sample modern service that depends on the abstraction IEmailSender
public class NotificationService
{
    private readonly IEmailSender _sender;
    public NotificationService(IEmailSender sender) => _sender = sender;

    public void NotifyUser(string userEmail, string message)
    {
        Console.WriteLine($"NotificationService: Preparing to send to {userEmail}");
        _sender.Send(userEmail, message);
        Console.WriteLine("NotificationService: Send requested.");
    }
}

