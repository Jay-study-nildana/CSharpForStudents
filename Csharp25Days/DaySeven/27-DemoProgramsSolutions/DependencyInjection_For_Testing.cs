using System;

/// <summary>
/// Problem: DependencyInjection_For_Testing
/// EmailSender depends on ISmtpClient; we inject a fake for tests to avoid static usage.
/// </summary>
class DependencyInjection_For_Testing
{
    public interface ISmtpClient { void Send(string to, string body); }

    public class RealSmtpClient : ISmtpClient
    {
        public void Send(string to, string body) => Console.WriteLine($"[REAL] Sending to {to}: {body}");
    }

    public class FakeSmtpClient : ISmtpClient
    {
        public string LastSentTo;
        public string LastBody;
        public void Send(string to, string body) { LastSentTo = to; LastBody = body; Console.WriteLine("[FAKE] Capture send"); }
    }

    public class EmailSender
    {
        private readonly ISmtpClient _smtp;
        public EmailSender(ISmtpClient smtp) => _smtp = smtp;
        public void SendWelcome(string userEmail) => _smtp.Send(userEmail, "Welcome!");
    }

    static void Main()
    {
        // Production
        var sender = new EmailSender(new RealSmtpClient());
        sender.SendWelcome("prod@company.com");

        // Test
        var fake = new FakeSmtpClient();
        var testSender = new EmailSender(fake);
        testSender.SendWelcome("test@fake");
        Console.WriteLine($"Test captured: to={fake.LastSentTo}, body={fake.LastBody}");
        Console.WriteLine("Injecting ISmtpClient makes EmailSender testable without static calls.");
    }
}