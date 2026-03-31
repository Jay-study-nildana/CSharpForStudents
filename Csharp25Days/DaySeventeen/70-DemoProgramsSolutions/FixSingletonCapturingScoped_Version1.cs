// FixSingletonCapturingScoped.cs
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DI.Exercises
{
    // BAD: singleton capturing a scoped DbContext (anti-pattern)
    public interface IAppDbContext { }
    public class AppDbContext : IAppDbContext { }

    // Bad: singleton that receives scoped service via ctor -> captive dependency
    public class BadSingletonService
    {
        private readonly IAppDbContext _db;
        public BadSingletonService(IAppDbContext db) => _db = db; // wrong if registered scoped
    }

    // FIX 1: Make the dependent service scoped instead of singleton
    public class ScopedService
    {
        private readonly IAppDbContext _db;
        public ScopedService(IAppDbContext db) => _db = db;
    }

    // FIX 2: Keep service singleton but create scopes when needed
    public class SingletonWithScope
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public SingletonWithScope(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public void DoWork()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            // use db within this scoped boundary
        }
    }

    // Registration hints:
    // services.AddScoped<IAppDbContext, AppDbContext>();
    // // Either:
    // services.AddScoped<ScopedService>();
    // // Or:
    // services.AddSingleton<SingletonWithScope>();

    // Recommendation: Prefer making the service scoped if its natural lifetime matches a scope;
    // use IServiceScopeFactory when the service must remain singleton but needs scoped work.
}