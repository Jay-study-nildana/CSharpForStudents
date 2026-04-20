# DC Comics SOLID Tutor

This is an interactive console application (C#) built to teach the SOLID principles using DC Comics themed examples.

Quick start

1. Install .NET SDK 8.0 or newer: https://dotnet.microsoft.com
2. From repository root run the app:

```bash
dotnet run --project src/SolidDc
```

Running tests

Run the unit tests (xUnit) for the project and collect coverage in one step:

```bash
# run all tests and collect coverage (produces a coverage.cobertura.xml file)
dotnet test tests/SolidDc.Tests --collect:"XPlat Code Coverage"
```

After the test run the coverage file will be placed under the test results folder, for example:

```
tests/SolidDc.Tests/TestResults/<run-guid>/coverage.cobertura.xml
```

Generating an HTML coverage report

Install ReportGenerator (global tool) and generate an HTML report from the Cobertura XML:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:tests/**/coverage.cobertura.xml -targetdir:CoverageReport -reporttypes:Html
```

Open `CoverageReport/index.htm` in your browser to view the coverage summary.

Notes

- Tests are in `tests/SolidDc.Tests` and are simple smoke tests that exercise each principle example.
- If `coverlet.collector` resolves to a different version you'll see a NuGet warning — that's safe.
- On CI, run the same `dotnet test` command and publish the generated `CoverageReport` folder or the Cobertura XML to your coverage service.

What you'll find

- An interactive menu to explore each SOLID principle
- DC Comics themed code examples and refactor demonstrations
- Plenty of comments in the source to help students follow along

Files of interest

- `src/SolidDc/Program.cs` — main entry and interactive menu
- `src/SolidDc/Principles/*` — one file per principle with demos
- `src/SolidDc/Models/*` — DC-themed models used across examples
- `src/SolidDc/Helpers/ConsoleHelper.cs` — simple helper for console IO

License

Use for teaching. No warranty.
