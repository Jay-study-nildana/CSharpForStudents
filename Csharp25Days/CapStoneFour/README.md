# DC Comics Super Heroes Capstone

`DCSuperHeroesCapstone` is a multi-project .NET console solution built as a terminal-based capstone for the `C# & .NET Fundamentals` curriculum. The theme is a fan-made `DC Comics Super Heroes` operations console where the Justice League manages heroes, missions, assignments, analytics, persistence, and mission outcomes from the Watchtower.

This project is designed to cover as many curriculum topics as possible while staying approachable for freshers.

Note : Built with GTP-5.4 which is included 1X Premium usage with GitHub CoPilot Pro. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference). 

## What The App Does

The console application lets a user:

1. Register DC-inspired heroes such as Batman, Superman, Wonder Woman, The Flash, and Zatanna.
2. Create missions with threat levels, stealth requirements, mystic requirements, schedules, and team sizes.
3. Ask Mission Control for team recommendations using hero readiness scoring.
4. Assign heroes to missions and launch or complete those missions.
5. View reports such as roster summaries, top heroes, mission counts, threat breakdowns, and availability.
6. Persist everything to JSON files so the state survives between runs.

## Solution Structure

```text
DCSuperHeroesCapstone.sln
|-- data/
|   |-- heroes.json
|   |-- missions.json
|   `-- assignments.json
|-- src/
|   |-- DCSuperHeroes.Core/
|   |   |-- Common/
|   |   |-- Entities/
|   |   |-- Enums/
|   |   |-- Events/
|   |   |-- Exceptions/
|   |   |-- Interfaces/
|   |   |-- Models/
|   |   `-- ValueObjects/
|   |-- DCSuperHeroes.Application/
|   |   |-- Contracts/
|   |   |-- Interfaces/
|   |   `-- Services/
|   |-- DCSuperHeroes.Infrastructure/
|   |   |-- Configuration/
|   |   |-- Logging/
|   |   `-- Persistence/
|   `-- DCSuperHeroes.Cli/
|       |-- Menus/
|       |-- Support/
|       |-- Program.cs
|       `-- appsettings.json
`-- tests/
    `-- DCSuperHeroes.Tests/
        |-- Integration/
        |-- Support/
        `-- Unit/
```

## Main Features

1. `Console application`: runs fully in the terminal with menu-driven navigation.
2. `OOP`: abstract base class `Hero` plus derived hero types `MetahumanHero`, `TechHero`, and `MysticHero`.
3. `Inheritance and polymorphism`: each hero type computes mission readiness differently.
4. `Collections`: lists, dictionaries, grouping, sorting, and filtering are used across the project.
5. `Generics`: the persistence layer uses a generic `JsonRepository<T>`.
6. `LINQ`: analytics, recommendations, searches, dashboards, and roster summaries use LINQ extensively.
7. `Exceptions and validation`: invalid rosters, missing entities, and bad input fail with domain-specific exceptions.
8. `Async programming`: repositories, logging, and application services are async.
9. `File I/O and JSON`: all entities persist to JSON files in the `data/` folder.
10. `Events and delegates`: mission assignment and mission completion raise events that the CLI subscribes to.
11. `Logging`: mission actions are logged to rolling daily files under `logs/`.
12. `Testing`: both unit tests and integration tests are included.
13. `Architecture`: split into Core, Application, Infrastructure, CLI, and Tests.

## Curriculum Coverage Map

1. `Days 1-4`: program structure, methods, parameters, scope, control flow, and decomposition are present across all projects.
2. `Days 6-10`: classes, constructors, records, enums, structs, abstraction, and inheritance appear in the domain model.
3. `Days 11-13`: collections, generics, and LINQ power the search and analytics features.
4. `Days 14-15`: validation, exceptions, file I/O, and JSON serialization are part of the service and repository layers.
5. `Days 16-18`: project structure, separation of concerns, manual DI, configuration, and logging are all included.
6. `Days 19-20`: mission events and async service/repository operations are used throughout the runtime flow.
7. `Days 21-22`: unit and integration tests are included under `tests/`.
8. `Days 23-26`: naming, refactoring-friendly structure, architecture boundaries, and DTO-style models are demonstrated.
9. `Days 27-30`: the project is packaged as a complete capstone with sample data, README, tests, and a demoable console workflow.

## Sample Runtime Flow

1. Start the Watchtower console.
2. Open `Hero Registry` and review the seeded Justice League roster.
3. Open `Mission Control` and inspect missions like `Apokolips Shadow` and `Oblivion Sigil`.
4. Request a recommended team for a mission.
5. Assign heroes by copying hero and mission IDs from the listings.
6. Launch the mission when the roster is full.
7. Complete the mission with an outcome summary.
8. Open `Reports and Analytics` to review updated dashboard data.

## Prerequisites

1. `.NET SDK 9.0` or newer installed.
2. A terminal such as PowerShell, Windows Terminal, or Command Prompt.
3. Optional: VS Code or Visual Studio for editing and debugging.

## Build

From the solution root:

```powershell
dotnet build DCSuperHeroesCapstone.sln
```

## Run

From the solution root:

```powershell
dotnet run --project src/DCSuperHeroes.Cli
```

The CLI reads configuration from `src/DCSuperHeroes.Cli/appsettings.json` and by default uses:

1. `data/` for persisted entity files
2. `logs/` for daily log files

## Test

Run all tests with:

```powershell
dotnet test DCSuperHeroesCapstone.sln
```

## Code Coverage

Collect coverage using the built-in collector:

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

Then, open the produced `.xml` file to review coverage details.

Alternative: generate an HTML report with ReportGenerator

Install ReportGenerator (one-time):

```powershell
dotnet tool install --global dotnet-reportgenerator-globaltool
```

Create HTML and open it:

```powershell
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html;
Start-Process .\CoverageReport\index.html
```

## Data Files

The repository includes starter JSON data:

1. `data/heroes.json`: seeded hero roster with polymorphic hero types.
2. `data/missions.json`: sample open missions.
3. `data/assignments.json`: empty at first so the user can build rosters manually.

Because the persistence layer uses polymorphic JSON, hero entries include a `$type` discriminator so derived hero types round-trip correctly.

## Important Design Decisions

1. `Manual DI instead of external framework`
   The application is beginner-friendly and keeps wiring visible in `Program.cs`.

2. `Abstract Hero base class`
   This makes inheritance and polymorphism explicit and useful rather than decorative.

3. `Generic JSON repository`
   This demonstrates generics, async file I/O, and reusable infrastructure code.

4. `Mission recommendation scoring`
   This uses collections, LINQ, and polymorphic behavior to solve a realistic domain problem.

5. `Separate Application layer`
   Service orchestration lives outside the domain entities, which supports testing and cleaner boundaries.

## Key Files To Explore

1. `src/DCSuperHeroes.Core/Entities/Hero.cs`
2. `src/DCSuperHeroes.Core/Entities/MetahumanHero.cs`
3. `src/DCSuperHeroes.Core/Entities/TechHero.cs`
4. `src/DCSuperHeroes.Core/Entities/MysticHero.cs`
5. `src/DCSuperHeroes.Application/Services/JusticeLeagueService.cs`
6. `src/DCSuperHeroes.Infrastructure/Persistence/JsonRepository.cs`
7. `src/DCSuperHeroes.Cli/Program.cs`
8. `src/DCSuperHeroes.Cli/Menus/MissionsMenu.cs`
9. `tests/DCSuperHeroes.Tests/Unit/JusticeLeagueServiceTests.cs`

## Capstone Scope Fit

This project satisfies the user requirements:

1. `Console project`: yes, it runs entirely in the terminal.
2. `Detailed README`: included here.
3. `Many folders and files`: the solution is intentionally split into multiple layers, folders, domain files, support files, and tests.
4. `Theme`: built around `DC Comics Super Heroes` and the Justice League Watchtower.

## Notes

1. This is an educational, fan-made training project inspired by DC Comics characters and themes.
2. It is structured to maximize curriculum coverage rather than to mimic a production comic franchise product.
