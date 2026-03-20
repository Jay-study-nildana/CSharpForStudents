# Library Management System — C# Capstone Project

A fully-featured console application built as a capstone project for the
**C# & .NET Fundamentals** course (30 days × 2 hours).

Note : Built with Claude Sonnet 4.6. Included with GitHub CoPilot Pro at Premium 1X pricing. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference)

---

## Features

| Feature             | Description                                                      |
| ------------------- | ---------------------------------------------------------------- |
| Book catalogue      | Add, remove, search, and list books with availability tracking   |
| Member registry     | Register members, search by name or email                        |
| Loan management     | Borrow and return books with due-date tracking                   |
| Overdue detection   | Computed `IsOverdue` property + dedicated overdue view           |
| Async file I/O      | All data persisted to JSON files using `async/await`             |
| Event notifications | `BookBorrowed` / `BookReturned` events fire on every transaction |
| Structured logging  | Colour-coded console log + daily rolling log file                |
| Unit tests          | 22 tests with Moq test doubles covering service logic and models |

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download) (or newer)
- A terminal (PowerShell, Bash, etc.)
- Optional: Visual Studio 2022 / VS Code with C# extension

---

## Solution structure

```
LibraryManagement.sln
├── src/
│   ├── LibraryManagement.Core/           ← Domain layer (no external dependencies)
│   │   ├── Models/                       ← Book, Member, Loan, LoanSummary (record), enums, EventArgs
│   │   ├── Interfaces/                   ← IRepository<T>, IBookRepository, ILibraryService, IAppLogger
│   │   ├── Services/                     ← LibraryService (business logic)
│   │   └── Exceptions/                   ← LibraryException hierarchy
│   │
│   ├── LibraryManagement.Infrastructure/ ← Data + logging implementations
│   │   ├── Repositories/                 ← JsonRepositoryBase<T>, JsonBookRepository, …
│   │   └── Logging/                      ← ConsoleFileLogger
│   │
│   └── LibraryManagement.CLI/            ← Console UI entry point
│       ├── Program.cs                    ← Composition root (manual DI)
│       ├── AppSettings.cs                ← Immutable config record
│       ├── appsettings.json              ← Runtime configuration
│       └── Menus/                        ← MainMenu, BookMenu, MemberMenu, LoanMenu
│
├── tests/
│   └── LibraryManagement.Core.Tests/
│       ├── Services/LibraryServiceTests.cs   ← 14 unit tests (Moq)
│       └── Models/                           ← BookTests + LoanTests (8 tests)
│
└── data/                                 ← Pre-loaded sample JSON data
    ├── books.json    (5 books)
    ├── members.json  (3 members)
    └── loans.json    (empty — start borrowing!)
```

---

## Build

```powershell
# From the solution root
dotnet build
```

---

## Run

```powershell
dotnet run --project src/LibraryManagement.CLI
```

The app creates the `data/` and `logs/` directories next to the executable on first run.

---

## Run tests

```powershell
# Run all tests
dotnet test

# With detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

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
Start-Process .\CoverageReport\index.htm
```

## Sample data

The `data/` folder contains pre-loaded records so you can explore the app immediately:

**Books (5)**

| Title                    | Author            | ISBN              | Category   |
| ------------------------ | ----------------- | ----------------- | ---------- |
| Clean Code               | Robert C. Martin  | 978-0-13-235088-4 | Technology |
| The Pragmatic Programmer | Andrew Hunt       | 978-0-13-595705-9 | Technology |
| 1984                     | George Orwell     | 978-0-45-152493-5 | Fiction    |
| A Brief History of Time  | Stephen Hawking   | 978-0-55-338016-3 | Science    |
| Sapiens                  | Yuval Noah Harari | 978-0-06-231609-7 | History    |

**Members (3)** — Alice Johnson, Bob Smith, Carol White

---

## Quick walkthrough

1. Run the app and choose **2. Members** → **1. List all members** to get a Member ID.
2. Choose **1. Books** → **2. List available books** to get a Book ID.
3. Choose **3. Loans** → **3. Borrow a book** — paste the Book ID and Member ID.
4. Watch the `BookBorrowed` event fire in cyan.
5. Choose **3. Loans** → **1. View active loans** to see the formatted loan table.
6. Return the book via **3. Loans** → **4. Return a book** using the Loan ID.

---

## Curriculum topics covered

| Day(s) | Topic                                          | Where                                                               |
| ------ | ---------------------------------------------- | ------------------------------------------------------------------- |
| 2      | Primitive types, variables, expressions        | All models                                                          |
| 3      | Control flow (loops, switch)                   | Menu loops, LINQ delegates                                          |
| 4      | Methods, scope, parameters                     | Service and menu methods                                            |
| 6–7    | Classes, properties, constructors, static      | `Book`, `Member`, `Loan`                                            |
| 8      | Inheritance & polymorphism                     | `LibraryException` hierarchy                                        |
| 9      | Interfaces & abstract classes                  | `IRepository<T>`, `ILibraryService`, `JsonRepositoryBase<T>`        |
| 10     | Structs, records, immutability                 | `LoanSummary` record, `AppSettings` record                          |
| 11     | Collections (List, Dictionary)                 | Repository List storage; LINQ `.ToDictionary()`                     |
| 12     | Generics                                       | `IRepository<T>`, `JsonRepositoryBase<T>`                           |
| 13     | LINQ                                           | Filtering, projection, `Where`, `Select`, `ToDictionary` in service |
| 14     | Exception handling                             | `try/catch` in menus; custom exception hierarchy                    |
| 15     | File I/O, JSON serialisation, atomic writes    | `JsonRepositoryBase<T>`                                             |
| 16     | Multi-project solution, separation of concerns | Core / Infrastructure / CLI                                         |
| 17     | Dependency injection (manual)                  | `Program.cs` composition root                                       |
| 18     | Logging, configuration, secrets                | `ConsoleFileLogger`, `AppSettings`, `appsettings.json`              |
| 19     | Delegates, events, lambdas                     | `BookBorrowed` / `BookReturned` events; lambda subscribers          |
| 20     | Async / await                                  | All repository and service methods                                  |
| 21     | Unit tests, mocks, AAA pattern                 | `LibraryServiceTests`, `BookTests`, `LoanTests`                     |
| 22     | Test data management                           | Sample JSON data files                                              |
| 23     | Code quality, naming                           | Single-responsibility menus and helper methods                      |
| 25     | Thread safety                                  | `SemaphoreSlim` in `JsonRepositoryBase` and `ConsoleFileLogger`     |
| 26     | Architecture, DTO vs domain model              | `LoanSummary` record, layered project structure                     |

---

## Capstone milestones

| Milestone                            | Status                               |
| ------------------------------------ | ------------------------------------ |
| Day 11 — Entity & collection design  | ✅ Book, Member, Loan + LINQ queries |
| Day 15 — Persistence plan            | ✅ JSON files with atomic writes     |
| Day 21 — Test plan (≥ 8 unit tests)  | ✅ 22 unit tests across 3 classes    |
| Days 28–29 — Implementation & polish | ✅ All features implemented          |
| Day 30 — Demo & submit               | ▶ Run `dotnet run` to demo           |

---

## Stretch goals implemented

- **Events/notifications** — `BookBorrowed` and `BookReturned` events with lambda subscribers
- **Reusable library** — `LibraryManagement.Core` is a standalone class library with no UI dependency
- **Thread-safe concurrent access** — `SemaphoreSlim` guards all file writes
