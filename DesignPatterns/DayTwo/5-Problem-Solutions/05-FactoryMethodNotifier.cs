// 05-FactoryMethodNotifier.cs
// Problem: Implement Factory Method for notification system.

using System;

public interface INotifier
{
    void Notify(string message);
}

public class EmailNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine($"[Email] {message}");
}

public class SmsNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine($"[SMS] {message}");
}

// Creator defines the Factory Method
public abstract class NotifierCreator
{
    // Factory method to be overridden by concrete creators
    protected abstract INotifier CreateNotifier();

    // Business method uses the product without knowing concrete type
    public void Send(string message)
    {
        var notifier = CreateNotifier();
        notifier.Notify(message);
    }
}

// Concrete creators decide which concrete product to instantiate
public class EmailNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

public class SmsNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}

/*
When to prefer Factory Method:
- When subclassing should determine which product is created.
- When creation logic must be encapsulated in subclasses so clients remain unaware of concrete products.
*/