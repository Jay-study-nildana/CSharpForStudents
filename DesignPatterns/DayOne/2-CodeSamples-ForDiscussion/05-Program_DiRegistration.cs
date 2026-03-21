// 05-Program_DiRegistration.cs
// Minimal example of registering services and lifetimes using Microsoft.Extensions.DependencyInjection patterns.
// Note: This file shows the conceptual registration; it does not require runtime DI packages to read.

using System;
using Microsoft.Extensions.DependencyInjection;

public static class Program_DiRegistration
{
    public static void Demo()
    {
        var services = new ServiceCollection();

        // Registration examples (conceptual)
        services.AddTransient<IMessageSender, EmailSender>(); // new instance each resolve
        services.AddScoped<IRepository<Customer>, InMemoryRepository<Customer>>(); // per-scope example
        services.AddSingleton<IAppConfig, AppConfig>(); // app-wide singleton

        var provider = services.BuildServiceProvider();

        // Resolve and use
        var sender = provider.GetRequiredService<IMessageSender>();
        var alertService = new AlertService(sender);
        alertService.Alert("Hello from DI demo");
    }
}

// Supporting types for conceptual registration (simple placeholders)

public interface IAppConfig { string AppName { get; } }
public class AppConfig : IAppConfig { public string AppName => "DemoApp"; }

public class Customer { }

public interface IRepository<T>
{
    void Add(T item);
}

public class InMemoryRepository<T> : IRepository<T>
{
    public void Add(T item) { /* store in memory */ }
}