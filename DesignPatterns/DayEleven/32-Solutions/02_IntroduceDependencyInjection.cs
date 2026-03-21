using System;

//
// Problem: Introduce Dependency Injection
// Test plan: swap repository implementation via composition root; demo shows mock used.
// Demonstrates: constructor injection and a simple composition root.
//

namespace Day11.RefactorLab
{
    public interface IMessageSender { void Send(string to, string body); }

    public class SmtpSender : IMessageSender
    {
        public void Send(string to, string body) => Console.WriteLine($"SMTP: Sent to {to}: {body}");
    }

    public class MockSender : IMessageSender
    {
        public void Send(string to, string body) => Console.WriteLine($"MOCK: Would send to {to}: {body}");
    }

    public class NotificationService
    {
        private readonly IMessageSender _sender;
        public NotificationService(IMessageSender sender) => _sender = sender;
        public void Notify(string to, string message) => _sender.Send(to, message);
    }

    class Program
    {
        static void Main()
        {
            // Composition root: swap implementation for testing or production
            IMessageSender sender = new MockSender(); // replace with new SmtpSender() in prod
            var svc = new NotificationService(sender);

            svc.Notify("alice@example.com", "Your order shipped.");
        }
    }
}