# Organizing Code into Projects and Assemblies (C# / .NET)  
Day 16 — Project & solution structure; packages and NuGet

Purpose
- Explain why and how to split a .NET solution into multiple projects/assemblies.
- Show a practical multi-project layout and how projects reference each other and NuGet packages.
- Provide small code snippets illustrating Domain, Infrastructure, and composition root (CLI/API).

Why multiple projects?
- Separation of concerns: keep UI, Domain (core business logic), Infrastructure (DB, file, network) separate so each can evolve independently.
- Explicit dependencies: assemblies make dependencies explicit and prevent accidental coupling.
- Testability: easier to write focused unit tests for core logic.
- Reuse and distribution: share core libraries as NuGet packages or across solutions.

Typical multi-project layout
- Solution: Capstone.sln
- Projects:
  - Capstone.Core (class library) — domain entities, interfaces, pure business logic
  - Capstone.Application (class library) — use-cases, DTOs, application services (depends on Core)
  - Capstone.Infrastructure (class library) — data access, external adapters (depends on Core and Application)
  - Capstone.Cli or Capstone.Api (console app or web app) — composition root and UI
  - Capstone.Tests (test project) — tests referencing Core/Application/Infrastructure as needed

Example directory tree
```text
src/
  Capstone.Core/
  Capstone.Application/
  Capstone.Infrastructure/
  Capstone.Cli/
tests/
  Capstone.Tests/
Capstone.sln
```

Create the solution and projects (dotnet CLI quick recipe)
- Create solution:
  dotnet new sln -n Capstone
- Create projects:
  dotnet new classlib -n Capstone.Core -o src/Capstone.Core
  dotnet new classlib -n Capstone.Application -o src/Capstone.Application
  dotnet new classlib -n Capstone.Infrastructure -o src/Capstone.Infrastructure
  dotnet new console -n Capstone.Cli -o src/Capstone.Cli
  dotnet new xunit -n Capstone.Tests -o tests/Capstone.Tests
- Add projects to solution:
  dotnet sln add src/Capstone.Core/src/Capstone.Core.csproj
  dotnet sln add src/Capstone.Application/src/Capstone.Application.csproj
  dotnet sln add src/Capstone.Infrastructure/src/Capstone.Infrastructure.csproj
  dotnet sln add src/Capstone.Cli/src/Capstone.Cli.csproj
  dotnet sln add tests/Capstone.Tests/tests/Capstone.Tests.csproj

Referencing projects (ProjectReference)
- Use ProjectReference for code within the same solution. This keeps compile-time checks and allows easy debugging across projects.

Example Capstone.Application.csproj (snippet)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Capstone.Core\Capstone.Core.csproj" />
  </ItemGroup>
</Project>
```

Design rules and boundaries
- Capstone.Core: no references to other projects (pure domain). Contains:
  - Entities (POCOs)
  - Value objects
  - Domain exceptions, domain service interfaces
- Capstone.Application: coordinates domain operations and defines application-level DTOs & interfaces. May reference Core.
- Capstone.Infrastructure: concrete implementations (e.g., EF Core DbContext, repositories). References Core and Application only if necessary.
- Capstone.Cli/Api: composition root — wire up DI and choose Infrastructure implementations to run the app.

Small code example: domain interface and implementation
- Domain interface in Capstone.Core:
```csharp
// src/Capstone.Core/ISampleRepository.cs
public interface ISampleRepository
{
    Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
```

- Infrastructure implementation in Capstone.Infrastructure:
```csharp
// src/Capstone.Infrastructure/SampleRepository.cs
public class SampleRepository : ISampleRepository
{
    private readonly DbContext _db;
    public SampleRepository(DbContext db) => _db = db;
    public async Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Set<SampleEntity>().FindAsync(new object?[] { id }, ct);
}
```

Composition root (registering services)
- In the CLI or Web API project, register dependencies once — this is the composition root. Keep this small and centralized.

Example Program.cs (Console app using Microsoft.Extensions.DependencyInjection)
```csharp
// src/Capstone.Cli/Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddScoped<ISampleRepository, SampleRepository>(); // Infrastructure implementation
        services.AddScoped<AppRunner>(); // Application runner that uses application services
    })
    .RunConsoleAsync();
```

NuGet and third-party packages
- Prefer small, well-scoped packages in Infrastructure (e.g., Microsoft.EntityFrameworkCore, Dapper).
- Add packages via CLI:
  dotnet add src/Capstone.Infrastructure/Capstone.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
- For libraries you want to distribute, create a NuGet package (dotnet pack) from the class library and publish to a feed (nuget.org or private feed).

Example csproj with PackageReference
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  </ItemGroup>
</Project>
```

Build and run
- Build entire solution: dotnet build Capstone.sln
- Run console app: dotnet run --project src/Capstone.Cli
- Run tests: dotnet test

Guidelines and best practices
- Keep Core independent: no references to Infrastructure or UI.
- Use interfaces for boundaries: Core defines required abstractions; Infrastructure implements them.
- Limit project count to what helps understandability — avoid an explosion of tiny projects unless warranted.
- Use folders inside projects for smaller logical separation (e.g., Entities/, Services/, Interfaces/).
- Protect internal APIs: prefer internal for things not intended to be public across assemblies, and use InternalsVisibleTo for test projects when necessary.
- Versioning and packages: bump package versions only when public surface changes; keep internal libraries as project references unless reuse outside the solution is needed.

Homework (apply this to your capstone)
- Sketch your solution with at least Core, Application, Infrastructure, and UI projects.
- For each project, list the main classes and interfaces it will contain.
- Describe how you will register dependencies in the composition root.
- Add a short build/run section showing dotnet CLI commands to build and run the solution.

References (quick commands cheat sheet)
- dotnet new sln -n Capstone
- dotnet new classlib -n Capstone.Core -o src/Capstone.Core
- dotnet sln add <path-to-csproj>
- dotnet add <proj> package <PackageId>
- dotnet add <proj> reference <otherproj.csproj>
- dotnet build
- dotnet test
- dotnet pack (create NuGet package)

End note
- The most important part of organizing projects is making dependencies explicit and keeping the domain core free of infrastructure concerns. Aim for clear boundaries: that will make your capstone maintainable, testable, and ready for reuse.