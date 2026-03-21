using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Creational;

// ─── Abstract Factory pattern ────────────────────────────────────────────────
// The factory produces a matched "family" of notification objects.
// Adding a new channel (Slack, SMS) = add one new concrete factory.

/// <summary>Console-based notification sender (no external deps).</summary>
public class ConsoleNotificationSender : INotificationSender
{
    private readonly IAppLogger _logger;
    public ConsoleNotificationSender(IAppLogger logger) => _logger = logger;

    public void Send(string recipient, string subject, string body)
    {
        var msg = $"[CONSOLE NOTIFY] To: {recipient} | {subject} | {body}";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(msg);
        Console.ResetColor();
        _logger.Log(msg);
    }
}

/// <summary>Simulated e-mail sender (writes to log instead of real SMTP).</summary>
public class EmailNotificationSender : INotificationSender
{
    private readonly IAppLogger _logger;
    public EmailNotificationSender(IAppLogger logger) => _logger = logger;

    public void Send(string recipient, string subject, string body)
    {
        var msg = $"[EMAIL] To: {recipient} | Subject: {subject} | Body: {body}";
        _logger.Log(msg);
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"  (simulated email sent to {recipient})");
        Console.ResetColor();
    }
}

/// <summary>Abstract Factory: Console channel.</summary>
public class ConsoleNotificationProviderFactory : INotificationProviderFactory
{
    private readonly IAppLogger _logger;
    public string ProviderName => "Console";
    public ConsoleNotificationProviderFactory(IAppLogger logger) => _logger = logger;
    public INotificationSender CreateSender() => new ConsoleNotificationSender(_logger);
}

/// <summary>Abstract Factory: E-mail channel.</summary>
public class EmailNotificationProviderFactory : INotificationProviderFactory
{
    private readonly IAppLogger _logger;
    public string ProviderName => "Email";
    public EmailNotificationProviderFactory(IAppLogger logger) => _logger = logger;
    public INotificationSender CreateSender() => new EmailNotificationSender(_logger);
}
