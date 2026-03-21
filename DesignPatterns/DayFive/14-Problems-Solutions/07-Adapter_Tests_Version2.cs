// 07-Adapter_Tests.cs
// Intent: Conceptual unit tests for LegacyPaymentAdapter and NotificationFacade using fakes/spies.
// DI/Lifetime: In tests register fakes in DI or construct objects directly.
// Testability: Show assertions to verify adapter mapping and facade orchestration.

using System;
using System.Collections.Generic;

// Conceptual test code (not tied to a test framework; replace with xUnit/NUnit syntax in real tests)
public static class AdapterFacadeTests
{
    public static void Test_LegacyPaymentAdapter_ChargesCorrectly()
    {
        // Arrange: create a fake legacy processor that records call parameters
        var fakeProcessor = new FakeLegacyProcessor();
        var adapter = new LegacyPaymentAdapter(fakeProcessor);

        // Act
        var success = adapter.Charge(12.34m, "USD");

        // Assert
        if (!success) throw new Exception("Expected success for valid amount");
        if (fakeProcessor.LastCents != 1234) throw new Exception("Adapter converted amount incorrectly");
        if (fakeProcessor.LastCurrency != "USD") throw new Exception("Adapter passed wrong currency");
    }

    public static void Test_NotificationFacade_Orchestrates_Subsystems()
    {
        // Arrange: create fakes for logger, metrics, email
        var fakeLogger = new FakeLogger();
        var fakeMetrics = new FakeMetrics();
        var fakeEmail = new FakeEmailSender();
        var facade = new NotificationFacade(fakeLogger, fakeMetrics, fakeEmail);

        // Act
        facade.SendNotification("a@b.com", "Hi", "Welcome");

        // Assert (conceptual):
        if (fakeMetrics.IncrementedKeys.Count != 2) throw new Exception("Metrics not updated as expected");
        if (fakeEmail.SentTo != "a@b.com") throw new Exception("Email not sent to expected recipient");
        if (!fakeLogger.Messages.Exists(m => m.Contains("Notification sent"))) throw new Exception("Logger not called as expected");
    }
}

// Simple test doubles
public class FakeLegacyProcessor : LegacyPaymentProcessor
{
    public int LastCents;
    public string LastCurrency;
    public override string ProcessPayment(int cents, string currencyCode)
    {
        LastCents = cents; LastCurrency = currencyCode;
        return cents > 0 ? "OK" : "ERR";
    }
}

public class FakeLogger : ILogger
{
    public List<string> Messages = new();
    public void Info(string message) => Messages.Add(message);
    public void Error(string message) => Messages.Add("ERROR: " + message);
}

public class FakeMetrics : IMetrics
{
    public List<string> IncrementedKeys = new();
    public void Increment(string key) => IncrementedKeys.Add(key);
}

public class FakeEmailSender : IEmailSender
{
    public string SentTo;
    public void SendEmail(string to, string subject, string body) => SentTo = to;
}