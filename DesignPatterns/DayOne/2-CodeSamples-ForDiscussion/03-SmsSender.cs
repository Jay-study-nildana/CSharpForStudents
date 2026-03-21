// 03-SmsSender.cs
// Alternative implementation of IMessageSender (shows substitutability).

using System;

public class SmsSender : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"[SmsSender] {message}");
    }
}