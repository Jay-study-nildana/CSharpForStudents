# Service Lifetimes (C# / .NET)

Purpose
- Explain the three main DI lifetimes in .NET (Transient, Scoped, Singleton).
- Show when to use each lifetime, common pitfalls (especially scoped→singleton capture), and practical patterns for background work and testing.
- Provide short code examples illustrating registration and correct fixes for lifetime issues.

Quick summary
- Transient (AddTransient): a new instance every time it’s requested. Good for lightweight, stateless services.
- Scoped (AddScoped): one instance per scope (in web apps, one per HTTP request). Good for request-scoped state like DbContext.
- Singleton (AddSingleton): one instance for the application lifetime. Good for thread-safe, stateless caches or long-lived helpers.

Why lifetimes matter
- Lifetimes define object sharing and disposal semantics. Choosing the wrong lifetime can cause subtle bugs, memory leaks, thread-safety issues, or captured request state in long-lived objects.
- The key rule: never have a Singleton depend on a Scoped service. Doing so can capture request-specific resources for the app lifetime.

Registration examples
```csharp
// Program.cs / composition root
builder.Services.AddTransient<IMath, FastMath>();       // new instance each resolution
builder.Services.AddScoped<IRepository, EfRepository>(); // one per request
builder.Services.AddSingleton<ICache, InMemoryCache>();  // one for app lifetime
```

When to use each lifetime
- Transient
  - Use if the service is cheap to construct and holds no state or disposable resources.
  - Useful for small helper classes, formatters, or strategy objects.
- Scoped
  - Use when service must be unique per logical operation (e.g., per HTTP request).
  - Typical for Entity Framework DbContext or per-request operation contexts.
- Singleton
  - Use for thread-safe caches, configuration wrappers, or services that maintain application-wide state.
  - Ensure singleton dependencies are also singleton or stateless.

Common pitfall: singleton capturing scoped services
- Problem: A singleton’s constructor receives a scoped service (e.g., DbContext). That scoped instance may be created outside a request scope or get reused across requests — producing incorrect behavior or exceptions.
- Example (incorrect):
```csharp
// WRONG: singleton depending on a scoped service
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddSingleton<IMyService, MyService>(); // MyService takes AppDbContext in ctor
```
- Why wrong: MyService is created once at startup (or first resolution) and retains the single AppDbContext instance, which is intended to be short-lived per request.

Correct solutions
1) Make the consumer scoped (preferred if it should follow request lifetime)
```csharp
// Make the dependent service scoped so it matches DbContext
builder.Services.AddScoped<IMyService, MyService>();
```
2) Inject a factory / IServiceScopeFactory for creating scoped instances inside singleton
```csharp
// Use a scope factory inside the singleton to create a proper scope when needed
public class MyBackgroundWorker : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public MyBackgroundWorker(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public async Task DoWorkAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IRepository>();
        await repo.DoSomeDbWorkAsync();
    }
}
```
3) Inject a delegate/factory (preferred for clarity)
```csharp
// Register a factory delegate
builder.Services.AddScoped<IRepository, EfRepository>();
builder.Services.AddSingleton<Func<IRepository>>(sp => () => sp.CreateScope().ServiceProvider.GetRequiredService<IRepository>());
```
Note: prefer explicit factory interfaces or IServiceScopeFactory over frequent use of IServiceProvider inside business code.

Background services pattern
- Background workers often need to access scoped services. Create a scope for each unit of work; do not inject scoped services directly into IHostedService singletons.
```csharp
public class TimedWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public TimedWorker(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRepository>();
            await repo.DoPeriodicWorkAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
```

Disposal and IDisposable services
- Scoped and transient services that implement IDisposable are disposed automatically by the container when their scope ends.
- Singleton disposables are disposed when the ServiceProvider is disposed (app shutdown). Ensure long-lived disposables are safe to live for the app lifetime.

Thread-safety considerations
- Singletons are shared across threads — make them thread-safe or immutable.
- Scoped and transient services are typically not shared across threads; still design for safe concurrent use when necessary.

Testing implications
- Unit tests: avoid relying on DI container lifetimes; instantiate classes directly with fakes or mocks via constructor injection.
- Integration tests: use the host/test server; lifetimes behave the same as runtime. Be explicit about scope when resolving services in test host.
- Example fake injection:
```csharp
var fakeRepo = new FakeRepository();
var svc = new MyService(fakeRepo); // simple constructor injection for unit tests
```

Quick troubleshooting checklist
- If you see shared state leaking between requests, check for singleton holding per-request state.
- If DbContext operations throw “cannot access a disposed object”, ensure the DbContext was not captured beyond its scope.
- If runtime resolution fails for a dependency, verify correct registration order and lifetime mismatches.

Best practices
- Keep composition root (Program.cs) as the only place wiring happens.
- Match lifetimes: prefer Scoped for DbContext and services that wrap request data, Singleton for stateless caches, Transient for cheap, stateless helpers.
- When a singleton needs scoped work, create a scope with IServiceScopeFactory or use a factory interface — don’t inject scoped services into singletons.
- Favor constructor injection to make dependencies explicit and easier to test.

Summary
- Service lifetimes control sharing and disposal. Choose them consciously: Transient for short-lived helpers, Scoped for request-scoped resources, Singleton for app-wide utilities. Avoid lifetime mismatches (especially singleton → scoped), and use factories or scope creation for safe background or long-lived operations.

Homework suggestion
- Inspect one class in your capstone that `new`s a dependency or is registered as Singleton. Explain what lifetime it should have, why, and refactor it to use constructor injection and correct lifetime management (5–10 lines).