// FactoryPatternDemo.cs
// Console demo of the Factory Design Pattern with interactive selection.

using System;

// Top-level statements
NotifierCreator creator = null;

while (true)
{
    Console.WriteLine("Choose notifier: email, sms, or exit");
    var choice = Console.ReadLine();
    if (choice == "exit") break;
    if (choice == "email")
        creator = new EmailNotifierCreator();
    else if (choice == "sms")
        creator = new SmsNotifierCreator();
    else
    {
        Console.WriteLine("Invalid choice.");
        continue;
    }
    Console.Write("Enter message: ");
    var msg = Console.ReadLine();
    creator.Send(msg);
}

public interface INotifier { void Notify(string message); }

public class EmailNotifier : INotifier
{
    public void Notify(string m) { Console.WriteLine($"[Email] {m}"); }
}

public class SmsNotifier : INotifier
{
    public void Notify(string m) { Console.WriteLine($"[SMS] {m}"); }
}

public abstract class NotifierCreator
{
    // Factory method
    protected abstract INotifier CreateNotifier();
    // Business method uses the product
    public void Send(string message)
    {
        var notifier = CreateNotifier();
        notifier.Notify(message);
    }
}

public class EmailNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

public class SmsNotifierCreator : NotifierCreator
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}