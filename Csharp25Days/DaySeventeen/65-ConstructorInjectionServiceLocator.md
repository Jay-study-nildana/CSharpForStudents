# Constructor Injection and Service Locators (C# / .NET)

Purpose
- Explain constructor injection (the recommended DI pattern) and contrast it with the service locator anti-pattern.
- Show practical examples, common lifetime pitfalls, how constructor injection improves testability, and a short refactoring checklist.

Quick summary
- Constructor injection: dependencies are required and passed into a class through its constructor. Explicit, testable, and easy to reason about.
- Service locator: classes ask a global registry or service provider for dependencies. Hidden, hard to test, and promotes runtime failure modes.

Why constructor injection?
- Makes dependencies explicit in the API of a class.
- Encourages coding to abstractions (interfaces).
- Simplifies unit testing by allowing test doubles to be passed in.
- Encourages a single composition root where wiring happens (Program.cs / Startup).

Example: interface and constructor injection
```csharp
// src/Core/ISampleRepository.cs
public interface ISampleRepository
{
    Task<Sample?> GetAsync(Guid id, CancellationToken ct = default);
    Task SaveAsync(Sample entity, CancellationToken ct = default);
}

// src/App/SampleService.cs
public class SampleService
{
    private readonly ISampleRepository _repository;
    private readonly ILogger<SampleService> _logger;

    public SampleService(ISampleRepository repository, ILogger<SampleService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RenameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var sample = await _repository.GetAsync(id, ct) ?? throw new KeyNotFoundException();
        sample.Rename(newName);              // domain logic inside Sample
        await _repository.SaveAsync(sample, ct);
        _logger.LogInformation("Renamed {Id} -> {Name}", id, newName);
    }
}
```

Composition root (where constructor injection is wired)
```csharp
// src/Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISampleRepository, SampleRepository>(); // Infrastructure implementation
builder.Services.AddScoped<SampleService>();                        // Application service
builder.Services.AddLogging();

var app = builder.Build();
app.MapControllers();
app.Run();
```

What is the service locator pattern?
- A service locator exposes a global API (ServiceLocator.Current, or IServiceProvider resolved inside classes) that lets any code fetch dependencies at runtime.
- Example: retrieving services inside business logic by calling a static locator or a service provider passed around.

Service locator (anti-pattern) example
```csharp
public class BadService
{
    public void DoWork()
    {
        var repo = ServiceLocator.Current.GetService<ISampleRepository>(); // hidden dependency
        var logger = ServiceLocator.Current.GetService<ILogger<BadService>>();
        // business logic that now depends on a global registry
    }
}
```

Why service locator is problematic
- Hidden dependencies: reading a class signature no longer tells you what it needs.
- Harder to test: tests must configure the global locator or replace it, increasing setup complexity and coupling tests to implementation details.
- Runtime failures: missing registrations fail at runtime rather than compile-time (less safe).
- Encourages misuse of the DI container as a global factory inside application code.

Testing and constructor injection
- With constructor injection you can pass fakes or mocks easily; tests are simple and fast.

Simple test (hand-rolled fake)
```csharp
public class FakeRepo : ISampleRepository
{
    private readonly Dictionary<Guid, Sample> _store = new();

    public Task<Sample?> GetAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.TryGetValue(id, out var s) ? s : null);

    public Task SaveAsync(Sample entity, CancellationToken ct = default)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public void Seed(Sample sample) => _store[sample.Id] = sample;
}

[Fact]
public async Task RenameAsync_UpdatesName()
{
    var fake = new FakeRepo();
    var sample = new Sample("old"); fake.Seed(sample);
    var logger = NullLogger<SampleService>.Instance;

    var svc = new SampleService(fake, logger);
    await svc.RenameAsync(sample.Id, "new");

    Assert.Equal("new", sample.Name);
}
```

Service lifetimes and a common pitfall
- Transient: AddTransient — new instance every resolution.
- Scoped: AddScoped — one instance per scope (e.g., HTTP request).
- Singleton: AddSingleton — one instance for app lifetime.
Key rule: do not have singletons depend on scoped services. If a singleton captures a scoped service, you may capture request-specific state incorrectly or throw runtime exceptions when the scope is not present.

Example pitfall
```csharp
// WRONG: Singleton depending on scoped DbContext
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddSingleton<IMyCache, MyCache>(); // MyCache should not accept AppDbContext in constructor
```
If MyCache captures AppDbContext at construction, the scoped DbContext is resolved outside an HTTP request scope — this leads to incorrect behavior or exceptions.

When a locator might appear necessary
- Factories or late-bound services that require runtime parameters can be implemented using factory delegates or IServiceProvider factory methods — keep the usage localized to factories in the composition root.
- Use Func<T> or factory interfaces instead of asking for IServiceProvider inside application code.

Factory registration example
```csharp
builder.Services.AddScoped<ISampleRepository, SampleRepository>();
builder.Services.AddScoped<Func<ISampleRepository>>(sp => () => sp.GetRequiredService<ISampleRepository>());
```
Prefer explicit factories or delegate factories over generic service provider usage inside domain code.

Decorator pattern (composition via DI)
- Use DI to decorate services (e.g., logging, caching) without service locators.

Example registration (decorator)
```csharp
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.Decorate<IEmailSender, LoggingEmailSender>(); // requires a DI helper or manual setup
```

Refactoring checklist (homework)
1. Find classes that `new` dependencies or call ServiceLocator/IServiceProvider.
2. Extract an interface for the dependency if one doesn’t exist.
3. Add constructor parameters for each dependency and store as readonly fields.
4. Replace `new` calls with injected dependencies.
5. Register concrete implementations in Program.cs with appropriate lifetimes.
6. Add unit tests using fakes or mocks to show the class can be tested without real infrastructure.

Best practices summary
- Prefer constructor injection for clarity and testability.
- Limit use of the DI container to the composition root; avoid service locator in business logic.
- Choose lifetimes carefully; never inject scoped services into singletons.
- Use explicit factories/delegates when runtime parameters are required.
- Keep dependencies small and oriented to interfaces to make unit tests trivial.

Further reading
- “Dependency Injection Principles, Practices, and Patterns” (Seemann & van Deursen)
- Microsoft docs: Dependency injection in .NET
- Articles on Composition Root and anti-patterns (Service Locator)