// 09-RefactorLegacyClientToUseFacade.cs
// Intent: Demonstrate refactor from direct subsystem calls to depending on NotificationFacade for cleaner code.
// DI/Lifetime: Client depends on facade via DI; facade lifetime as discussed earlier.
// Testability: Client tests become simpler because only the facade needs to be faked.

using System;

// Before refactor (legacy client - conceptual)
/*
public class LegacyClient
{
    private readonly ILogger _logger;
    private readonly IMetrics _metrics;
    private readonly IEmailSender _email;

    public LegacyClient(ILogger logger, IMetrics metrics, IEmailSender email)
    {
        _logger = logger; _metrics = metrics; _email = email;
    }

    public void DoWork(string email, string msg)
    {
        _logger.Info("Start");
        _metrics.Increment("work.started");
        _email.SendEmail(email, "Subject", msg);
        _metrics.Increment("work.completed");
        _logger.Info("Done");
    }
}
*/

// After refactor: client uses NotificationFacade
public class RefactoredClient
{
    private readonly NotificationFacade _notifications;
    public RefactoredClient(NotificationFacade notifications) => _notifications = notifications;

    public void DoWork(string email, string msg)
    {
        // Single high-level call; internal steps are encapsulated in the facade
        _notifications.SendNotification(email, "Subject", msg);
    }
}

/*
Benefits:
- Client code is shorter and focused on intent (send notification).
- Tests for RefactoredClient need only assert facade was called (inject fake facade), instead of faking logger/email/metrics separately.
- Facade centralizes tracing/metrics, making cross-cutting concerns consistent.
*/