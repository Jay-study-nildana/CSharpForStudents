# Dependency Injection & Inversion of Control (C# / .NET)

Purpose
- Explain the concepts of Dependency Injection (DI) and Inversion of Control (IoC).
- Show why DI improves modularity and testability.
- Demonstrate constructor injection, service lifetimes, basic registration patterns, common pitfalls, and a small unit test example.

Why DI and IoC?
- Inversion of Control (IoC) means a class does not create its own dependencies; a framework or caller supplies them.
- Dependency Injection is a technique to implement IoC by passing dependencies (usually via constructors).
- Benefits: decoupling, easier unit testing, clearer responsibilities, and simpler composition of application behavior.

Core concepts
- Dependency: any object a class needs to do its job (e.g., repository, logger).
- Abstraction: depend on interfaces or abstract types (not concrete classes).
- Composition Root: the single place where concrete implementations are wired up (e.g., Program.cs).
- DI Container / Service Provider: resolves and supplies dependencies at runtime.

Constructor injection (recommended)
- Prefer constructor injection: it's explicit, makes dependencies visible, and prevents hidden runtime errors.

Example: domain/service + repository interface
```csharp
// src/Capstone.Core/ISampleRepository.cs
public interface ISampleRepository
{
    Task<Sample?> GetAsync(Guid id, CancellationToken ct = default);
}

// src/Capstone.Application/SampleService.cs
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
        sample.Rename(newName);
        _logger.LogInformation("Renamed sample {Id} to {Name}", id, newName);
        // persist via repository (not shown)
    }
}
```

Registering services in the composition root
```csharp
// src/Capstone.Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISampleRepository, SampleRepository>(); // Infrastructure
builder.Services.AddScoped<SampleService>(); // Application
builder.Services.AddLogging();

var app = builder.Build();
app.MapControllers();
app.Run();
```

Service lifetimes (conceptual)
- Transient: new instance each resolution (AddTransient). Use for lightweight, stateless services.
- Scoped: one instance per scope, commonly one HTTP request (AddScoped). Use for per-request state like DbContext.
- Singleton: single instance for app lifetime (AddSingleton). Use for stateless, thread-safe services or caches.
Key rule: avoid injecting scoped services into singletons — it causes captured scoped state lifetime issues.

Factory and delegate registrations
```csharp
// register with factory when construction needs runtime data
builder.Services.AddSingleton<IClock>(_ => new SystemClock(TimeZoneInfo.Utc));
```

Anti-pattern: Service Locator
- Calling serviceProvider.GetService<T>() deep inside business logic hides dependencies and makes testing hard.
- Prefer constructor injection; the composition root should own serviceProvider interactions.

Before (service locator anti-pattern)
```csharp
public class BadService
{
    public void DoWork()
    {
        var repo = ServiceLocator.Current.Get<ISampleRepository>(); // hidden dependency
        // ...
    }
}
```

After (constructor injection)
```csharp
public class GoodService
{
    private readonly ISampleRepository _repo;
    public GoodService(ISampleRepository repo) => _repo = repo;
    public void DoWork() { /* use _repo */ }
}
```

Testing with DI
- Since classes depend on interfaces, replace implementations with test doubles (mocks/fakes) easily.

Unit test example (xUnit + Moq-style pseudo)
```csharp
// Example using a simple hand-written fake for clarity
public class FakeRepository : ISampleRepository
{
    private readonly Dictionary<Guid, Sample> _store = new();
    public Task<Sample?> GetAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.TryGetValue(id, out var s) ? s : null);
    public void Add(Sample s) => _store[s.Id] = s;
}

[Fact]
public async Task RenameAsync_ChangesName()
{
    var fakeRepo = new FakeRepository();
    var sample = new Sample("old");
    fakeRepo.Add(sample);

    var logger = NullLogger<SampleService>.Instance;
    var svc = new SampleService(fakeRepo, logger);

    await svc.RenameAsync(sample.Id, "new");

    Assert.Equal("new", sample.Name);
}
```
- Using test doubles removes the need for a real DB or external dependencies and keeps unit tests fast and reliable.

Advanced patterns and tips
- Decorator: compose behavior (e.g., logging, caching) around an interface. Register the decorated service in DI to wrap the original.
- Open generics: register open generic types (e.g., IRepository<T>) for generic repositories or handlers.
- Options pattern: use IOptions<T> to configure services from configuration.
- Dispose carefully: singletons that hold disposable resources should implement IDisposable and be registered appropriately.

Common pitfalls
- Capturing scoped services inside singleton constructors -> runtime exceptions or leaked scoped state.
- Long-lived singletons depending on short-lived resources (DbContext, HttpContext) -> subtle bugs.
- Overusing ServiceProvider inside application code (service locator).
- Too many small services for no benefit — prefer logical grouping.

Refactoring checklist for constructor injection (homework)
1. Identify concrete dependencies instantiated with `new` inside the class.
2. Extract interfaces for those dependencies if they don't exist.
3. Add constructor parameters for the interfaces and store to readonly fields.
4. Register concrete implementations in the composition root (Program.cs) with appropriate lifetimes.
5. Replace `new` usages with injected fields.
6. Add unit tests replacing implementations with fakes/mocks.

Summary
- DI/IoC decouples components and moves object assembly to a centralized composition root.
- Use constructor injection and prefer abstractions; register services with correct lifetimes.
- Avoid service locators and be cautious about lifetimes and disposal.
- DI makes unit testing straightforward — replace dependencies with fakes/mocks to isolate logic.

Homework suggestion
- Pick one class from your capstone that `new`s dependencies. Refactor it to use constructor injection and write a short note (5–10 lines) describing the changes and why DI improved the class testability.

Further reading
- Microsoft docs: Dependency injection in .NET
- Patterns: Composition Root, Decorator, Factory, Options
- Books: "Dependency Injection Principles, Practices, and Patterns" (Steven van Deursen & Mark Seemann)