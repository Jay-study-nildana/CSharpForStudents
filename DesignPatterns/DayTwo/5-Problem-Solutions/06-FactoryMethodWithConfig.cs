// 06-FactoryMethodWithConfig.cs
// Problem: Factory Method where concrete product selection depends on runtime config.

using System;

public class ConfigurableNotifierCreator : NotifierCreatorBase
{
    private readonly string _mode; // e.g., "email" or "sms"
    public ConfigurableNotifierCreator(string mode) => _mode = mode;

    protected override INotifier CreateNotifier()
    {
        return _mode?.ToLowerInvariant() switch
        {
            "sms" => new SmsNotifier(),
            _ => new EmailNotifier(),
        };
    }
}

// Reuse simple notifier implementations and define a base abstract creator
public abstract class NotifierCreatorBase
{
    protected abstract INotifier CreateNotifier();
    public void Send(string message)
    {
        var notifier = CreateNotifier();
        notifier.Notify(message);
    }
}

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

/*
Note:
- Clients call new ConfigurableNotifierCreator(mode).Send(msg) without knowing notifier types.
- This keeps creation encapsulated; it's useful when creation depends on configuration or environment.
*/