// 00-BadTightlyCoupledService-Fixed.cs
// Demonstrates proper dependency injection to decouple AlertService from EmailSender.

using System;

public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class AlertService
{
    private readonly IMessageSender _sender;

    // Dependency is injected via constructor
    public AlertService(IMessageSender sender)
    {
        _sender = sender;
    }

    public void Alert(string text)
    {
        _sender.Send(text);
    }
}

// Example usage:
// var service = new AlertService(new EmailSender());
// service.Alert("SOLID principles rock!");
