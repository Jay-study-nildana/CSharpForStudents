// 09-FactoryMethodTestability.cs
// Problem: Demonstrate how Factory Method improves testability with a TestNotifierCreator.

using System;

public interface INotifier
{
    void Notify(string message);
}

public class EmailNotifier : INotifier
{
    public void Notify(string message) => Console.WriteLine($"[Email] {message}");
}

// Production creator uses factory method to create EmailNotifier
public abstract class NotifierCreator
{
    protected abstract INotifier CreateNotifier();
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

// Test double (fake notifier) and test creator
public class FakeNotifier : INotifier
{
    public string LastMessage { get; private set; }
    public void Notify(string message) => LastMessage = message;
}

public class TestNotifierCreator : NotifierCreator
{
    private readonly FakeNotifier _fake;
    public TestNotifierCreator(FakeNotifier fake) => _fake = fake;
    protected override INotifier CreateNotifier() => _fake;
}

/*
Example test (conceptual):
var fake = new FakeNotifier();
var creator = new TestNotifierCreator(fake);
creator.Send("hello");
Assert.Equal("hello", fake.LastMessage);

Factory Method allows injecting a test creator that returns a fake product, improving test isolation.
*/