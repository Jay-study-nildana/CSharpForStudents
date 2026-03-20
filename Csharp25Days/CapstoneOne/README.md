# Capstone: Console Task Tracker

This is a console-style capstone project designed to exercise core C# and .NET fundamentals from the course curriculum: OOP, collections, LINQ, async file I/O, serialization, dependency inversion (interfaces), events/delegates, unit testing, and simple persistence.

Note : Built with GTP5-mini which is included for free and unlimited usage with GitHub CoPilot Pro. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference). 

## Quick commands

PowerShell (Windows) / VS Code Terminal:

```powershell
dotnet build
dotnet run --project src/Capstone.Console/Capstone.Console.csproj
dotnet test
dotnet test --no-build
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

## What I created

- `src/Capstone.Core` : core domain models, storage interface, file storage, and `TaskService`.
- `src/Capstone.Console` : console UI and program entrypoint.
- `tests/Capstone.Tests` : unit tests for `TaskService` using an in-memory storage test double.

## Primary features

- Create, list, search and complete tasks
- Persistent JSON storage (`data/tasks.json`) via an `IStorage<T>` abstraction
- Async file I/O and JSON serialization
- Events (task completed) and lambdas/LINQ for queries
- Unit tests that exercise core behavior and the event pattern

## Files of interest

- [src/Capstone.Core/Models/TaskItem.cs](src/Capstone.Core/Models/TaskItem.cs)
- [src/Capstone.Core/Services/TaskService.cs](src/Capstone.Core/Services/TaskService.cs)
- [src/Capstone.Core/Storage/FileStorage.cs](src/Capstone.Core/Storage/FileStorage.cs)
- [src/Capstone.Console/Program.cs](src/Capstone.Console/Program.cs)
- [src/Capstone.Console/ConsoleUI.cs](src/Capstone.Console/ConsoleUI.cs)
- [tests/Capstone.Tests/TaskServiceTests.cs](tests/Capstone.Tests/TaskServiceTests.cs)

## Seeding Tasks From File

- The console app includes a menu option `8) Seed Tasks From File` which imports tasks from a JSON seed file and persists them to the application's data file (`data/tasks.json`).
- A sample seed file is included at [src/Capstone.Console/Data/seed_tasks.json](src/Capstone.Console/Data/seed_tasks.json).
- Behavior:
  - The import ignores duplicate tasks by `Id` (existing tasks are left unchanged).
  - The app searches a handful of likely locations for `seed_tasks.json` (project Data folder, `data/`, current directory, and runtime paths). If no file is found it prints the locations it checked.
  - After importing the found tasks the app automatically saves them to the configured data file.

## Usage (quick)

1. Build and run the console app:

```powershell
dotnet build
dotnet run --project src/Capstone.Console/Capstone.Console.csproj
```

2. From the app menu select `8` to seed tasks from the included `seed_tasks.json` file.

3. The app will report how many tasks were imported and save them to `data/tasks.json`.

If you'd like different behavior (prompt before saving, non-destructive preview, or CSV import support), tell me and I can add it.

## Next steps

- Run `dotnet run` to try the console app, then `dotnet test` to run unit tests.

- Run `dotnet run` to try the console app, then `dotnet test` to run unit tests.
