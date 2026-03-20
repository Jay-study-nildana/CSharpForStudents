# Dependency Injection and Testability (C# / .NET)

Purpose
- Explain how Dependency Injection (DI) makes code easier to test.
- Show practical patterns: constructor injection, test doubles (fakes, mocks), unit vs integration tests.
- Provide C# snippets students can use in their capstone refactor and tests.

Why DI helps testing
- DI separates object creation from behavior. When classes receive dependencies (interfaces) instead of creating concrete objects, tests can replace real implementations with controlled test doubles.
- Tests become fast, deterministic, and focused: you test logic in isolation without hitting databases, file systems, or external services.

Core pattern: constructor injection
- Constructor injection makes dependencies explicit and immutable. Tests instantiate the class under test with fake implementations.

Domain/service example
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
    private readonly ISampleRepository _repo;
    private readonly ILogger<SampleService> _logger;

    public SampleService(ISampleRepository repo, ILogger<SampleService> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RenameAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var s = await _repo.GetAsync(id, ct) ?? throw new KeyNotFoundException();
        s.Rename(newName);
        await _repo.SaveAsync(s, ct);
        _logger.LogInformation("Renamed {Id} -> {Name}", id, newName);
    }
}
```

Unit testing with a hand-rolled fake
- Hand-written fakes are simple, readable, and great for teaching.

```csharp
// tests/Fakes/FakeSampleRepository.cs
public class FakeSampleRepository : ISampleRepository
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

// tests/SampleServiceTests.cs
[Fact]
public async Task RenameAsync_UpdatesName()
{
    var fakeRepo = new FakeSampleRepository();
    var sample = new Sample("old"); fakeRepo.Seed(sample);
    var logger = NullLogger<SampleService>.Instance;

    var svc = new SampleService(fakeRepo, logger);
    await svc.RenameAsync(sample.Id, "new");

    Assert.Equal("new", sample.Name);
}
```

Unit testing with a mocking framework
- Use Moq (or similar) for behavior verification and more concise tests.

```csharp
var mockRepo = new Mock<ISampleRepository>();
mockRepo.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(sample);
var logger = NullLogger<SampleService>.Instance;

var svc = new SampleService(mockRepo.Object, logger);
await svc.RenameAsync(id, "new");

mockRepo.Verify(r => r.SaveAsync(It.Is<Sample>(s => s.Name == "new"), It.IsAny<CancellationToken>()), Times.Once);
```

When to use fakes vs mocks
- Fakes: simple stateful in-memory implementations — easy to understand and reliable for unit tests.
- Mocks: verify interactions (calls, parameters). Use when you need to assert behavior (e.g., an event was published).
- Prefer fakes for domain logic validation and mocks for messaging/side-effect verification.

Integration testing with DI
- Integration tests exercise multiple layers (e.g., controllers + EF Core). Use the test host (WebApplicationFactory or TestServer) to override registrations.

Example (minimal for ASP.NET Core):
```csharp
var factory = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            // Replace real DB with an in-memory DbContext
            services.RemoveDbContext<AppDbContext>();
            services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TestDb"));
        });
    });

var client = factory.CreateClient();
var resp = await client.PostAsync("/api/samples/...", content);
```
- Replace production services using ConfigureServices in test host to inject test doubles.

Testing scoped/singleton lifetimes
- Avoid writing tests that depend on DI container lifetimes — instantiate class under test directly for unit tests.
- For services that rely on scoped resources inside singleton components (e.g., background workers), test the scope creation logic by mocking IServiceScopeFactory or by creating a real scope in integration tests.

Example: testing background worker that creates scopes
```csharp
var scopeFactoryMock = new Mock<IServiceScopeFactory>();
// setup to return a scope whose ServiceProvider provides required test services...
```

Refactoring checklist for testability
1. Replace direct `new` of collaborators with constructor parameters.
2. Depend on interfaces (abstractions) rather than concrete types.
3. Keep business logic free of I/O; push I/O to repository/adapters.
4. Prefer simple hand-made fakes for unit tests and use mocks only when verifying interactions.
5. Wire concrete implementations only in composition root (Program.cs).
6. For background tasks, use IServiceScopeFactory to access scoped services safely.

Common pitfalls and how DI avoids them
- Hidden dependencies (service locator): constructor injection makes dependencies visible and compile-time checkable.
- Slow/flaky tests: DI enables replacing slow resources (DB, network) with fast in-memory fakes.
- Shared mutable state: wrong service lifetimes cause cross-test leakage — prefer creating fresh fakes per test.

Homework suggestion
- Pick one class in your capstone that creates collaborators with `new`. Refactor it to use constructor injection, add a simple fake, and write one unit test that exercises the core logic without hitting external resources. Submit a 6–10 line note describing changes and test approach.

Summary
- DI shifts responsibility for wiring from classes to the composition root, enabling test doubles and clearer, faster tests.
- Constructor injection + small, focused interfaces = easy, reliable unit tests.
- Use fakes for stateful behavior checks, mocks for interaction verification, and the test host for integration scenarios.
