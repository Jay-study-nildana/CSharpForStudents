using System;
using System.Collections.Generic;

class Fake_For_UnitTest
{
    // Problem: IEmailSender and NotificationService; show FakeEmailSender for unit testing
    public interface IEmailSender { void Send(string to, string body); }

    public class NotificationService
    {
        private readonly IEmailSender _email;
        public NotificationService(IEmailSender email) => _email = email;
        public void NotifyUser(string email) => _email.Send(email, "Welcome!");
    }

    // Fake used in tests to capture calls
    public class FakeEmailSender : IEmailSender
    {
        public List<(string to, string body)> Sent = new();
        public void Send(string to, string body) => Sent.Add((to, body));
    }

    static void Main()
    {
        var fake = new FakeEmailSender();
        var svc = new NotificationService(fake);
        svc.NotifyUser("user@example.com");

        Console.WriteLine($"Fake recorded {fake.Sent.Count} send(s). First: to={fake.Sent[0].to}, body={fake.Sent[0].body}");
        // In unit tests we assert fake.Sent contains expected entries.
    }
}