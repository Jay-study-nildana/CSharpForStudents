// 08-FactoryDelegateAndDI.cs
// Factory-delegate approach using Func<string, INotifier> wired via DI.
// DI/Lifetime: Notifier implementations registered as Transient; delegate registered to resolve and return appropriate instance.
// Testability: Provide a delegate that returns fakes in tests.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

public interface INotifier { void Notify(string message); }
public class EmailNotifier : INotifier { public void Notify(string m) => Console.WriteLine($"Email: {m}"); }
public class SmsNotifier : INotifier { public void Notify(string m) => Console.WriteLine($"SMS: {m}"); }

public class NotifierConsumer
{
    private readonly Func<string, INotifier> _factory;
    public NotifierConsumer(Func<string, INotifier> factory) => _factory = factory;

    public void Notify(string kind, string message)
    {
        var notifier = _factory(kind);
        notifier.Notify(message);
    }
}

/*
Conceptual DI registration:

services.AddTransient<EmailNotifier>();
services.AddTransient<SmsNotifier>();
services.AddTransient<Func<string, INotifier>>(sp => key =>
    string.Equals(key, "sms", StringComparison.OrdinalIgnoreCase)
        ? sp.GetRequiredService<SmsNotifier>()
        : sp.GetRequiredService<EmailNotifier>());

Now NotifierConsumer can be registered and injected normally.
*/