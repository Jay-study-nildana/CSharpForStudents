// 03-Facade_NotificationFacade.cs
// Intent: Facade that unifies logging, metrics, and email sending behind a single API.
// DI/Lifetime: Facade can be Transient/Scoped; if it holds only thread-safe dependencies it may be Singleton.
// Testability: Tests can inject a fake facade into clients or test the facade by injecting fake subsystems.

using System;

public interface IMetrics { void Increment(string key); }
public interface IEmailSender { void SendEmail(string to, string subject, string body); }

public class NotificationFacade
{
    private readonly ILogger _logger;
    private readonly IMetrics _metrics;
    private readonly IEmailSender _email;

    public NotificationFacade(ILogger logger, IMetrics metrics, IEmailSender email)
    {
        _logger = logger;
        _metrics = metrics;
        _email = email;
    }

    // High-level unified operation
    public void SendNotification(string userEmail, string subject, string message)
    {
        _logger.Info($"Preparing notification for {userEmail}");
        _metrics.Increment("notifications.prepared");
        _email.SendEmail(userEmail, subject, message);
        _metrics.Increment("notifications.sent");
        _logger.Info($"Notification sent to {userEmail}");
    }
}

// Example usage in a client:
public class AlertController
{
    private readonly NotificationFacade _notifications;
    public AlertController(NotificationFacade notifications) => _notifications = notifications;

    public void TriggerAlert(string userEmail, string details)
    {
        _notifications.SendNotification(userEmail, "Alert", details);
    }
}