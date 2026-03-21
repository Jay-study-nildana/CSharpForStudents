// 07-FactoryDelegateWithDI.cs
// Problem: Implement a factory-delegate approach (Func<string, INotifier>) and demonstrate usage.
// Also show a commented example of how to register such a factory in IServiceCollection.

using System;
using System.Collections.Generic;

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

// A lightweight factory provider that behaves like a delegate: Func<string, INotifier>
public class NotifierFactory
{
    private readonly IDictionary<string, Func<INotifier>> _map;

    public NotifierFactory()
    {
        // In a DI app you would inject factories/instances instead of newing here.
        _map = new Dictionary<string, Func<INotifier>>(StringComparer.OrdinalIgnoreCase)
        {
            ["email"] = () => new EmailNotifier(),
            ["sms"] = () => new SmsNotifier()
        };
    }

    public INotifier Create(string key)
    {
        if (_map.TryGetValue(key, out var factory)) return factory();
        return _map["email"]();
    }
}

// Consumer that depends on a factory delegate
public class NotificationService
{
    private readonly Func<string, INotifier> _notifierFactory;

    public NotificationService(Func<string, INotifier> notifierFactory)
    {
        _notifierFactory = notifierFactory;
    }

    public void Notify(string kind, string message)
    {
        var notifier = _notifierFactory(kind);
        notifier.Notify(message);
    }
}

/*
Example DI registration (conceptual):
services.AddTransient<EmailNotifier>();
services.AddTransient<SmsNotifier>();
services.AddTransient<Func<string, INotifier>>(sp => key =>
    key == "sms" ? sp.GetRequiredService<SmsNotifier>() : sp.GetRequiredService<EmailNotifier>()
);

Advantages:
- Allows runtime selection without complex inheritance hierarchies.
- Plays well with DI containers and test replacement.
*/