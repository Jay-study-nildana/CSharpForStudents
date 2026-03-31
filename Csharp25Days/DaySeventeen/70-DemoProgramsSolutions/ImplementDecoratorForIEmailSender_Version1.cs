// ImplementDecoratorForIEmailSender.cs
using System;
using System.Threading.Tasks;

namespace DI.Exercises
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }

    // Core implementation that sends emails (placeholder)
    public class SmtpEmailSender : IEmailSender
    {
        public Task SendAsync(string to, string subject, string body)
        {
            // send via SMTP...
            Console.WriteLine($"SMTP send to {to}");
            return Task.CompletedTask;
        }
    }

    // Decorator that logs then delegates
    public class LoggingEmailSender : IEmailSender
    {
        private readonly IEmailSender _inner;
        public LoggingEmailSender(IEmailSender inner) => _inner = inner;

        public async Task SendAsync(string to, string subject, string body)
        {
            Console.WriteLine($"Logging: sending to {to} subject:{subject}");
            await _inner.SendAsync(to, subject, body);
        }
    }

    // Registration approach (in composition root):
    // services.AddTransient<SmtpEmailSender>();
    // services.AddTransient<IEmailSender>(sp => new LoggingEmailSender(sp.GetRequiredService<SmtpEmailSender>()));
    //
    // Resolving IEmailSender yields LoggingEmailSender which delegates to SmtpEmailSender.
    //
    // Note: Alternatively use Scrutor's Decorate extension if available to avoid factories.
}