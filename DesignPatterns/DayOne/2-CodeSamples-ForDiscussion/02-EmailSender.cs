// 02-EmailSender.cs
// Concrete implementation of IMessageSender.

using System;

public class EmailSender : IMessageSender
{
    public void Send(string message)
    {
        // Real code would send an email; we keep it simple for the example
        Console.WriteLine($"[EmailSender] {message}");
    }
}