// CompositionRootSetup.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DI.Exercises
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    // DbContext (scoped)
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("ConnectionStringHere"));

                    // Repositories & application services
                    services.AddScoped<IBackgroundRepository, BackgroundRepository>();
                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<ScopedService>();

                    // Email sender + decorator
                    services.AddTransient<SmtpEmailSender>();
                    services.AddTransient<IEmailSender>(sp => new LoggingEmailSender(sp.GetRequiredService<SmtpEmailSender>()));

                    // Background worker (hosted) - uses IServiceScopeFactory inside
                    services.AddHostedService<PeriodicCleanupService>();

                    // In-memory cache singleton example
                    services.AddSingleton<IInMemoryCache, InMemoryCache>();

                    // Factory delegate for report generation
                    services.AddTransient<Func<string, IReportGenerator>>(sp => template => new ReportGenerator(template));

                    // Logging already provided by Host
                });

        // One-line rationale: Composition root centralizes 'new' and registrations so application wiring is explicit and testable.
    }
}