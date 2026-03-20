# NuGet and Package Management (C# / .NET)

Purpose
- A concise, practical guide to NuGet and package management for .NET projects.
- Covers adding/removing packages, project vs package references, central package versions, creating and publishing packages, and best practices.

What is NuGet?
- NuGet is the package manager for .NET. Packages distribute reusable code (libraries, tools) and are consumed by adding PackageReference entries to your project or by installing via the dotnet CLI or Visual Studio.

Key concepts
- Package: a .nupkg file that contains assemblies, metadata, and optionally content and tools.
- Feed (source): where packages are stored — public (nuget.org) or private (Azure Artifacts, local folder, other registries).
- PackageReference: the modern way to reference a NuGet package from an SDK-style project (csproj).
- Transitive dependencies: packages you depend on may bring their own dependencies automatically.

Add / remove / list packages (dotnet CLI)
- Add a package:
  dotnet add src/MyApp/MyApp.csproj package Newtonsoft.Json
- Remove a package:
  dotnet remove src/MyApp/MyApp.csproj package Newtonsoft.Json
- List installed packages:
  dotnet list src/MyApp/MyApp.csproj package
- Restore packages (usually automatic on build):
  dotnet restore

Project file with PackageReference (example)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />
  </ItemGroup>
</Project>
```

ProjectReference vs PackageReference
- ProjectReference: references another project in the same solution (source-level, used during development).
  dotnet add src/MyApp/MyApp.csproj reference ../MyLib/MyLib.csproj
- PackageReference: references a compiled NuGet package (useful for versioned libraries or when sharing across solutions).
- Use ProjectReference during development; use PackageReference when consuming published libraries or when sharing across teams.

Centralizing versions: Directory.Packages.props (Central Package Management)
- To manage consistent package versions across many projects, use a central props file at repository root.

Example Directory.Packages.props:
```xml
<Project>
  <ItemGroup>
    <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageVersion Include="Polly" Version="7.2.3" />
  </ItemGroup>
</Project>
```
- Then in csproj files:
```xml
<PackageReference Include="Newtonsoft.Json" />
```
- This prevents version drift and simplifies upgrades.

NuGet.Config — custom feeds and settings
- Configure additional feeds or credentials via a NuGet.Config file (repo root, user profile).
Example to add a private feed and disable package signature validation (for examples):
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="MyPrivateFeed" value="https://pkgs.example.com/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

Creating a NuGet package (SDK-style csproj)
- Add pack settings to the library csproj:
```xml
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <PackageId>Contoso.MyLib</PackageId>
  <Version>1.2.0</Version>
  <Authors>Contoso</Authors>
  <Description>Useful utilities for Contoso apps.</Description>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <RepositoryUrl>https://github.com/contoso/mylib</RepositoryUrl>
  <IncludeSymbols>false</IncludeSymbols>
</PropertyGroup>
```
- Pack the project:
  dotnet pack src/Contoso.MyLib/Contoso.MyLib.csproj -c Release
- The .nupkg appears in bin/Release and can be published.

Publish a package
- Push to nuget.org (requires an API key):
  dotnet nuget push bin/Release/Contoso.MyLib.1.2.0.nupkg --api-key <KEY> --source https://api.nuget.org/v3/index.json
- Publish to a private feed or a local feed (local folder feed):
  dotnet nuget push bin/Release/*.nupkg --source "C:\local-nuget-feed"

Versioning strategies
- Semantic Versioning (MAJOR.MINOR.PATCH) is recommended.
- Use pre-release suffixes for unstable releases: 1.2.0-alpha.1
- Floating versions (e.g., 6.* or 1.0.*) are supported but can lead to unpredictable builds — prefer exact or caret ranges for CI stability.

Dependency locking: packages.lock.json
- Enable lock files to ensure reproducible restores:
  dotnet restore --use-lock-file
- Commit packages.lock.json for CI to guarantee consistent dependency graph and avoid unexpected updates.

Transitive dependencies and security
- Transitive packages are resolved automatically. Use:
  dotnet list package --vulnerable
  or enable Dependabot (or GitHub's security alerts) for automatic vulnerability scanning and PRs.
- Only trust feeds you control or that use signing and good reputation. Use upstream sources (nuget.org) and private feeds behind authentication for internal packages.

Best practices
- Keep dependencies minimal and well-scoped; prefer small focused packages.
- Use ProjectReference while developing across multiple projects in the same repo; convert to PackageReference if you publish a library for reuse.
- Centralize versions with Directory.Packages.props for multi-project repositories.
- Use CI to run dotnet restore/dotnet build/dotnet test and to publish packages from tagged builds only.
- Sign or vet third-party packages and run automated vulnerability scans.
- Avoid floating versions in production code; prefer explicit versions or centrally controlled updates.

Quick commands cheat sheet
- Create package: dotnet pack
- Publish package: dotnet nuget push <pkg> --source <source>
- Add package: dotnet add <proj> package <PackageId> --version <Version>
- Remove package: dotnet remove <proj> package <PackageId>
- List packages: dotnet list <proj> package
- Restore: dotnet restore
- Lock restore: dotnet restore --use-lock-file

Example workflow (small team)
1. Develop shared library as project reference locally.
2. When ready, pack and publish to private feed (CI on tag).
3. Consume published package via PackageReference from other repositories.
4. Use Directory.Packages.props in monorepos for consistent versions.
5. CI runs automated scans and updates via Dependabot.

Summary
- NuGet is central to .NET dependency management. Use PackageReference, prefer explicit versions, centralize versions for many projects, and automate packaging/publishing in CI. Secure your feeds and monitor vulnerabilities to keep dependencies safe and reproducible.
