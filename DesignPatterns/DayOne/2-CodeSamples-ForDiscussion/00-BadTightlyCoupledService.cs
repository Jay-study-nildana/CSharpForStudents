// 00-BadTightlyCoupledService.cs
// Demonstrates a tightly-coupled class that creates its own concrete dependency (anti-pattern).

using System;

public class EmailSender
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class AlertService_Bad
{
    private readonly EmailSender _sender;

    public AlertService_Bad()
    {
        // Direct instantiation couples AlertService_Bad to EmailSender
        _sender = new EmailSender();
    }

    public void Alert(string text)
    {
        _sender.Send(text);
    }
}