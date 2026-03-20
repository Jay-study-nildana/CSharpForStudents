# Separation of Concerns — UI / Domain / Data / Others (C# / .NET)

Purpose
- Explain the Separation of Concerns (SoC) principle for .NET apps.
- Show practical layer responsibilities (UI, Domain, Infrastructure/Data, Application, Cross-cutting).
- Provide concise C# snippets demonstrating interfaces, implementations, and composition.

Why Separation of Concerns?
- Reduce coupling: each part of the system has one reason to change.
- Improve testability: test domain logic without involving I/O or UI.
- Improve maintainability: replace or upgrade one layer (e.g., DB provider) without touching domain logic.
- Enable reuse: core domain logic can be reused across UIs (CLI, Web API, worker).

Common layers and responsibilities
- Presentation / UI
  - Accepts user input and displays output.
  - Contains controllers, pages, CLI handlers, or view models.
  - Should not contain business rules or direct data-access logic.
- Application (or Service) layer
  - Coordinates use-cases and orchestrates domain operations.
  - Implements application workflows, validation orchestration, transactions.
  - Calls domain services and repositories via interfaces.
- Domain (Core)
  - Pure business logic: entities, value objects, domain services, domain exceptions.
  - No references to Infrastructure or UI.
- Infrastructure / Data
  - Concrete implementations for persistence (EF Core, Dapper), external APIs, file systems.
  - Implements interfaces defined by Domain or Application layers.
- Cross-cutting concerns
  - Logging, caching, configuration, telemetry, authentication.
  - Typically registered centrally (composition root) and applied via middleware or decorators.

Design rules and boundaries
- The Domain project/reference should have zero infrastructure dependencies.
- Define abstractions (interfaces) in the Domain or Application projects; implement them in Infrastructure.
- Keep the composition root (Program.cs / Startup) as the only place that wires concrete implementations.
- Use dependency injection and constructor injection to keep code testable and decoupled.

Example: Domain interface (Capstone.Core)
```csharp
// src/Capstone.Core/ISampleRepository.cs
public interface ISampleRepository
{
    Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task SaveAsync(SampleEntity entity, CancellationToken ct = default);
}
```

Example: Domain entity (pure logic)
```csharp
// src/Capstone.Core/SampleEntity.cs
public class SampleEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public SampleEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required") : name;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) throw new InvalidOperationException("Invalid name");
        Name = newName;
    }
}
```

Example: Infrastructure implementation (Capstone.Infrastructure)
```csharp
// src/Capstone.Infrastructure/SampleRepository.cs
public class SampleRepository : ISampleRepository
{
    private readonly AppDbContext _db;
    public SampleRepository(AppDbContext db) => _db = db;

    public async Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Set<SampleEntity>().FindAsync(new object[] { id }, ct);

    public async Task SaveAsync(SampleEntity entity, CancellationToken ct = default)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync(ct);
    }
}
```

Example: Application service (orchestrating use-case)
```csharp
// src/Capstone.Application/SampleService.cs
public class SampleService
{
    private readonly ISampleRepository _repo;
    public SampleService(ISampleRepository repo) => _repo = repo;

    public async Task RenameSampleAsync(Guid id, string newName, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException();
        entity.Rename(newName);
        await _repo.SaveAsync(entity, ct);
    }
}
```

Example: Presentation (Web API controller or CLI handler)
```csharp
// src/Capstone.Api/Controllers/SamplesController.cs
[ApiController]
[Route("api/[controller]")]
public class SamplesController : ControllerBase
{
    private readonly SampleService _service;
    public SamplesController(SampleService service) => _service = service;

    [HttpPost("{id:guid}/rename")]
    public async Task<IActionResult> Rename(Guid id, [FromBody] RenameDto dto)
    {
        await _service.RenameSampleAsync(id, dto.NewName);
        return NoContent();
    }
}
```

Composition root: registering dependencies (single centralized place)
```csharp
// src/Capstone.Api/Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ISampleRepository, SampleRepository>(); // Infrastructure -> Interface
builder.Services.AddScoped<SampleService>(); // Application service
var app = builder.Build();
app.MapControllers();
app.Run();
```

Cross-cutting concerns and decorators
- For logging/validation/caching, prefer middleware or decorator patterns.
- Keep these concerns out of domain classes; they should be orthogonal and pluggable.

Practical tips
- Put interfaces in the project that most logically owns the abstraction (usually Domain/Core for repositories or Application for use-case contracts).
- Keep small projects to a reasonable number. Group closely related concerns to avoid over-fragmentation.
- Use integration tests for Infrastructure and unit tests for Domain and Application.
- Prefer async Task-based APIs in boundaries that involve I/O.
- Avoid exposing persistence-specific types (e.g., DbContext, IQueryable<T>) from Domain APIs.

When to break a rule
- For performance or architectural reasons you may relax boundaries — do it consciously and document the reason.
- If a domain concept naturally needs a small helper in infrastructure (e.g., encryption key store), prefer an explicit interface and adapter.

Homework prompt (apply SoC to your capstone)
- Draw your solution diagram showing layers and arrows of allowed dependencies.
- For each project list: 5 example classes/interfaces and one external package it may need.
- Write Program.cs showing DI registrations and a simple endpoint or CLI command.

Further reading (short)
- Domain-Driven Design (Evans) — for domain layering and ubiquitous language.
- Microsoft docs: Clean architecture and layering in .NET.
- Patterns: Repository, Unit of Work, Dependency Injection, Decorator.

Summary
- Separation of Concerns is about explicit boundaries: Domain = rules, Infrastructure = implementation, UI = interaction, Application = orchestration.
- Keep dependencies pointing inward (inbound to Domain), wire up concrete implementations only in composition root, and you'll gain testability, maintainability, and clarity.
