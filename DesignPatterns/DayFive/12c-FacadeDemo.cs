// NotificationFacadeDemo.cs
// Console demo of the Facade Pattern for notifications.

using System;

// Top-level statements
var logger = new ConsoleLogger();
var metrics = new ConsoleMetrics();
var email = new ConsoleEmailSender();
var facade = new NotificationFacade(logger, metrics, email);

while (true)
{
    Console.WriteLine("Enter recipient email (or 'exit' to quit):");
    var to = Console.ReadLine();
    if (to == "exit") break;
    Console.Write("Subject: ");
    var subject = Console.ReadLine();
    Console.Write("Message: ");
    var body = Console.ReadLine();
    facade.Send(to, subject, body);
    Console.WriteLine();
}


public interface ILogger { void Log(string msg); }
public interface IMetrics { void Increment(string key); }
public interface IEmailSender { void SendEmail(string to, string subject, string body); }

// Simple implementations for demo
public class ConsoleLogger : ILogger
{
    public void Log(string msg) => Console.WriteLine($"[LOG] {msg}");
}

public class ConsoleMetrics : IMetrics
{
    public void Increment(string key) => Console.WriteLine($"[METRICS] Incremented {key}");
}

public class ConsoleEmailSender : IEmailSender
{
    public void SendEmail(string to, string subject, string body)
        => Console.WriteLine($"[EMAIL] To: {to}\nSubject: {subject}\nBody: {body}");
}

public class NotificationFacade
{
    private readonly ILogger _logger;
    private readonly IMetrics _metrics;
    private readonly IEmailSender _email;

    public NotificationFacade(ILogger logger, IMetrics metrics, IEmailSender email)
    {
        _logger = logger; _metrics = metrics; _email = email;
    }

    // Simplified high-level operation
    public void Send(string userEmail, string subject, string message)
    {
        _logger.Log($"Preparing to notify {userEmail}");
        _metrics.Increment("notifications.sent");
        _email.SendEmail(userEmail, subject, message);
        _logger.Log($"Notification sent to {userEmail}");
    }
}

