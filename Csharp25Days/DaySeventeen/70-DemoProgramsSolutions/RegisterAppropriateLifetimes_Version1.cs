// RegisterAppropriateLifetimes.cs
using Microsoft.Extensions.DependencyInjection;

namespace DI.Exercises
{
    // Example components
    public interface IFormatter { string Format(string s); }
    public class Formatter : IFormatter { public string Format(string s) => s.ToUpperInvariant(); }

    public class ApplicationDbContext { } // pretend EF DbContext
    public interface IInMemoryCache { void Set(string k, object v); object Get(string k) => null; }
    public class InMemoryCache : IInMemoryCache { public void Set(string k, object v) { } public object Get(string k) => null; }

    public static class LifetimeRegistration
    {
        public static void Register(IServiceCollection services)
        {
            // Stateless formatting helper — Transient (lightweight, stateless).
            services.AddTransient<IFormatter, Formatter>(); // new instance per resolution

            // Per-request DbContext — Scoped (one instance per request/scope).
            services.AddScoped<ApplicationDbContext>(); // reused within a scope

            // In-memory cache — Singleton (shared across app lifetime).
            services.AddSingleton<IInMemoryCache, InMemoryCache>();
        }
    }

    // Each registration above is chosen to match typical usage:
    // Transient for small stateless helper, Scoped for per-request DbContext lifecycle,
    // Singleton for a shared in-memory cache.
}