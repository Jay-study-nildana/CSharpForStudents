// 09-RefactorLegacyToFactoryMethod.cs
// Refactor example: legacy code creates concrete notifiers directly; refactor to Factory Method.
// Testability: clients become testable by substituting TestCreator/fakes.

using System;

// Legacy (before) - conceptual:
/*
public class LegacySender
{
    public void Send(string msg)
    {
        var email = new EmailNotifier(); // direct concrete instantiation
        email.Notify(msg);
    }
}
*/

// After refactor: Factory Method
public interface INotifier { void Notify(string message); }
public class EmailNotifier : INotifier { public void Notify(string m) => Console.WriteLine($"Email: {m}"); }
public class SmsNotifier : INotifier { public void Notify(string m) => Console.WriteLine($"SMS: {m}"); }

// Creator base class with factory method
public abstract class NotifierCreator
{
    protected abstract INotifier CreateNotifier();
    public void Send(string message)
    {
        var notifier = CreateNotifier();
        notifier.Notify(message);
    }
}

// Concrete creators decide which notifier to instantiate
public class EmailNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

public class SmsNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}

// Client uses a Creator (can be injected)
public class NotificationClient
{
    private readonly NotifierCreator _creator;
    public NotificationClient(NotifierCreator creator) => _creator = creator;
    public void DoSend(string msg) => _creator.Send(msg);
}

/*
Benefits:
- NotificationClient no longer knows concrete types.
- Tests can inject a TestNotifierCreator that returns a fake notifier to assert behavior.
*/