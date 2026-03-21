// 06-AdapterAndFacade_DIRegistration.cs
// Intent: Show conceptual IServiceCollection registrations for adapters and facades with rationale for lifetimes.
// DI/Lifetime: Adapters often Transient; facades Transient/Scoped; if facade and its dependencies are thread-safe, Singleton ok.
// Testability: Register interfaces to allow swapping fakes/mocks during tests.

using Microsoft.Extensions.DependencyInjection;

public static class DiRegistrationSamples
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Legacy dependency instances (adaptees) may be created per use or injected if stateful.
        // Example: registering legacy processor as Transient
        // services.AddTransient<LegacyPaymentProcessor>();

        // Adapter registration: depends on LegacyPaymentProcessor
        // services.AddTransient<IPaymentGateway, LegacyPaymentAdapter>();

        // Legacy logger
        // services.AddSingleton<LegacyLogger>();
        // services.AddTransient<ILogger, LegacyLoggingAdapter>(); // adapter maps legacy API to ILogger

        // Facade registration
        // If NotificationFacade depends on ILogger, IMetrics, IEmailSender:
        // services.AddTransient<NotificationFacade>();

        // Rationale:
        // - Transient for adapters: keep them lightweight and avoid holding on to unexpected state.
        // - Facade transient/scoped: if it composes scoped dependencies (DbContext, UoW) prefer Scoped.
        // - Singleton only if all dependencies are Singletons and thread-safe.
    }
}