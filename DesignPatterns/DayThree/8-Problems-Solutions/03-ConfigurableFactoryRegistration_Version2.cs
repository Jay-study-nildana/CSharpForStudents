// 03-ConfigurableFactoryRegistration.cs
// Demonstrates selecting and registering a concrete factory based on configuration.
// DI/Lifetime: register chosen factory as IProviderFactory; configure at startup.

using System;
using Microsoft.Extensions.DependencyInjection;

public static class FactoryConfigurator
{
    // Conceptual registration: choose factory implementation by config value
    public static void RegisterFactory(IServiceCollection services, string mode)
    {
        if ((mode ?? "sql").Equals("memory", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IProviderFactory, InMemoryProviderFactory>();
        }
        else
        {
            services.AddSingleton<IProviderFactory, SqlProviderFactory>();
        }
    }
}

/*
Consumer receives IProviderFactory via constructor injection:
public class Consumer { public Consumer(IProviderFactory f) { ... } }
*/