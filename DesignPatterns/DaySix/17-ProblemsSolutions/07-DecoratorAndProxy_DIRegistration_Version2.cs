// 07-DecoratorAndProxy_DIRegistration.cs
// Intent: Show DI registration patterns for composing decorators and registering proxies.
// DI/Lifetime: Recommend aligning lifetimes: scoped with scoped dependencies, transient for stateless decorators; proxies depend on resource lifetimes.
// Testability: Register fakes in test Startup or construct compositions directly in tests.

using Microsoft.Extensions.DependencyInjection;

// Conceptual registration snippets (comments)
public static class DiSamples
{
    public static void Register(IServiceCollection services)
    {
        // Register core service
        // services.AddTransient<RealService>();
        // Manual decorator composition:
        // services.AddTransient<IService>(sp => new LoggingDecorator(
        //     new CachingDecoratorSafe(sp.GetRequiredService<RealService>())));

        // If using a DI decorator helper (Scrutor) hypothetical usage:
        // services.AddTransient<RealService>();
        // services.AddTransient<IService>(sp => sp.GetRequiredService<RealService>());
        // services.Decorate<IService, CachingDecoratorSafe>();
        // services.Decorate<IService, ValidationDecorator>(); // order of Decorate calls matters

        // Proxy registration example:
        // services.AddTransient<Func<IHeavyResource>>(sp => () => new HeavyResource());
        // services.AddTransient<IHeavyResource>(sp => new HeavyResourceProxy(sp.GetRequiredService<Func<IHeavyResource>>()));
    }
}