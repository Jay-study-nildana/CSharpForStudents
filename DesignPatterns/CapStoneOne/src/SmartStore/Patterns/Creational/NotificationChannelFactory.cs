namespace SmartStore.Patterns.Creational;

// ================================================================
// FACTORY METHOD PATTERN
// ================================================================
// Defines a factory method (CreateChannel) for creating INotificationChannel
// instances. Each concrete factory subclass decides which channel to make.
//
// Intent   : Define an interface for creating an object, but let subclasses
//            decide which class to instantiate.
// Problem  : The caller needs a notification channel but should not know
//            (or hard-code) the concrete type.
// Solution : The abstract factory class provides the CreateChannel() hook.
//            Subclasses override it to produce different channel types.
// ================================================================
public abstract class NotificationChannelFactory
{
    public abstract INotificationChannel CreateChannel();

    /// <summary>Template-method-style helper: creates then immediately sends.</summary>
    public void SendNotification(string recipient, string subject, string message)
    {
        var channel = CreateChannel();
        channel.Send(recipient, subject, message);
    }
}

// --- Concrete Factories ---

public class ConsoleNotificationFactory : NotificationChannelFactory
{
    public override INotificationChannel CreateChannel() =>
        new ConsoleNotificationChannel();
}

public class EmailNotificationFactory : NotificationChannelFactory
{
    public override INotificationChannel CreateChannel() =>
        new EmailNotificationChannel();
}
