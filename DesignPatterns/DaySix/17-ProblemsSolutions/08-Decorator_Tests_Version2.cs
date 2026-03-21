// 08-Decorator_Tests.cs
// Intent: Conceptual unit tests verifying LoggingDecorator, CachingDecorator, and ValidationDecorator behavior using fakes/spies.
// DI/Lifetime: In tests construct decorator chain manually and inject fakes; no DI required for simple unit tests.
// Testability: Assertions are in comments; replace with test framework asserts (xUnit/NUnit) as needed.

using System;
using System.Collections.Generic;

public static class DecoratorTests
{
    public static void Test_LoggingDecorator_RecordsMessages()
    {
        // Arrange
        var fake = new FakeService("hello");
        var logs = new List<string>();
        var logger = new TestLogger(msg => logs.Add(msg));
        var loggingDecorator = new LoggingDecorator(fake); // LoggingDecorator writes to Console in example; adapt to TestLogger in real code

        // Act
        var result = loggingDecorator.GetData(1);

        // Assert (conceptual)
        // Check that result equals expected and that logs contain Enter/Exit messages.
        // e.g., Assert.Contains("[Log] Enter GetData(1)", logs);
    }

    public static void Test_CachingDecorator_AvoidsRepeatedCalls()
    {
        var counting = new CountingService();
        var cache = new CachingDecoratorSafe(counting);

        var a = cache.GetData(2); // counting.Calls == 1
        var b = cache.GetData(2); // counting.Calls == 1

        // Assert: counting.Calls == 1
    }

    public static void Test_ValidationDecorator_BlocksInvalidInput()
    {
        var called = false;
        var inner = new FakeService("x") { OnCall = () => called = true };
        var validate = new ValidationDecoratorSimple(inner);

        try
        {
            validate.GetData(0);
            // Fail: expected exception
        }
        catch (ArgumentException)
        {
            // Expected
        }

        // Assert: called == false (inner was not invoked)
    }
}

// Supporting fakes (simplified)
public class FakeService : IService
{
    private readonly string _value;
    public Action OnCall;
    public FakeService(string value) => _value = value;
    public string GetData(int id) { OnCall?.Invoke(); return _value; }
}

public class TestLogger
{
    private readonly Action<string> _sink;
    public TestLogger(Action<string> sink) => _sink = sink;
    public void Write(string msg) => _sink(msg);
}