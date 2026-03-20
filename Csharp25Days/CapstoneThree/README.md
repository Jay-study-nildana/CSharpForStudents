# Kapow! Comic Book Shop — CLI Application

A fully-featured **console-based comic book shop management system** built with C# and .NET 8.  
This capstone project demonstrates core concepts from the _C# & .NET Fundamentals_ curriculum across 30 days of training.

Note : Built with Claude Opus 4.6. Included with GitHub CoPilot Pro at Premium 3X pricing. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference)

---

## Features

| Area                         | Description                                                                             |
| ---------------------------- | --------------------------------------------------------------------------------------- |
| **Comics Management**        | Add, update, remove, list, and search comics with multi-criteria filtering              |
| **Customer Management**      | Register customers, manage membership tiers (Bronze → Platinum), search by name         |
| **Order Processing**         | Multi-item orders, automatic stock deduction, membership discounts, receipt generation  |
| **Reports & Analytics**      | Inventory summary, sales report, top-selling comics, revenue by genre, low-stock alerts |
| **Event Notifications**      | Real-time alerts for stock changes, low-stock warnings, and new orders                  |
| **Persistent Storage**       | JSON file-based persistence with async I/O                                              |
| **Logging**                  | Structured file logging with date-stamped log files                                     |
| **Unit & Integration Tests** | 30+ test cases using xUnit and Moq                                                      |

---

## Curriculum Topics Covered

| Day(s) | Topic                                 | Where It Appears                                                                                                       |
| ------ | ------------------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| 1–2    | Types, variables, expressions         | Entity properties, enums, constants                                                                                    |
| 3      | Control flow (if/switch/loops)        | Menu loops, validation branching, switch expressions                                                                   |
| 4      | Methods, parameters, scope            | Service methods, helper methods, decomposition                                                                         |
| 5      | Debugging, Git                        | Stack traces, breakpoints (try/catch), project in Git                                                                  |
| 6–7    | OOP: classes, constructors, static    | Entity classes, `ConsoleHelper` (static), `BaseEntity`                                                                 |
| 8      | Inheritance & polymorphism            | `BaseEntity` → `ComicBook` / `Customer` / `Order`                                                                      |
| 9      | Interfaces & abstract classes         | `IRepository<T>`, `IComicBookService`, `IOrderService`, `IAppLogger`                                                   |
| 10     | Structs, records, immutability        | `PriceRange` struct, `ComicBookSummary` / `OrderReceipt` / `SearchCriteria` records                                    |
| 11     | Collections                           | `List<T>`, `Dictionary<K,V>`, collection operations everywhere                                                         |
| 12     | Generics & type safety                | `IRepository<T>`, `JsonRepository<T>`, `GetEnumChoice<TEnum>()`                                                        |
| 13     | LINQ                                  | Search, filter, group, aggregate in services & reports                                                                 |
| 14     | Exception handling                    | Custom exceptions (`ValidationException`, `InsufficientStockException`, `EntityNotFoundException`), try/catch in menus |
| 15     | File I/O & serialization              | `JsonRepository` — async JSON read/write via `System.Text.Json`                                                        |
| 16     | Project & solution structure          | Multi-project solution: Core → Infrastructure → CLI + Tests                                                            |
| 17     | Dependency Injection                  | `Microsoft.Extensions.DependencyInjection`, constructor injection                                                      |
| 18     | Logging & configuration               | `FileLogger`, `appsettings.json`, `AppSettings` POCO binding                                                           |
| 19     | Delegates, events, lambdas            | `StockChanged`, `LowStockAlert`, `OrderPlaced` events; `NotificationService` subscriber; lambdas in LINQ               |
| 20     | Async programming                     | `async`/`await` throughout services and repository                                                                     |
| 21     | Unit testing & test doubles           | xUnit `[Fact]` tests with Moq mocks for repositories and loggers                                                       |
| 22     | Integration testing                   | `JsonRepositoryTests` — real file I/O with temp directory and cleanup                                                  |
| 23     | Code quality & naming                 | Clean naming, small focused methods, single responsibility                                                             |
| 24     | Performance                           | Dictionary lookups, in-memory caching in repository                                                                    |
| 25     | Concurrency & thread safety           | `SemaphoreSlim` in `JsonRepository` and `InventoryManager`; concurrent deduction test                                  |
| 26     | Architecture & separation of concerns | Layered architecture (Core / Infrastructure / CLI)                                                                     |
| 27     | NuGet & packages                      | `Microsoft.Extensions.*`, `xunit`, `Moq`                                                                               |

---

## Solution Structure

```
CapstoneThree/
├── src/
│   ├── ComicBookShop.Core/            # Domain: entities, enums, interfaces, services, events, exceptions
│   │   ├── Entities/                  # BaseEntity, ComicBook, Customer, Order, OrderItem
│   │   ├── Enums/                     # Genre, ComicCondition, MembershipTier, OrderStatus
│   │   ├── Models/                    # Records & struct (DTOs, SearchCriteria, PriceRange)
│   │   ├── Interfaces/               # IRepository<T>, IComicBookService, ICustomerService, IOrderService, IAppLogger
│   │   ├── Events/                    # StockChangedEventArgs, OrderPlacedEventArgs
│   │   ├── Exceptions/               # Custom exceptions
│   │   └── Services/                  # ComicBookService, CustomerService, OrderService, InventoryManager, NotificationService
│   ├── ComicBookShop.Infrastructure/  # Persistence, logging, configuration
│   │   ├── Persistence/              # JsonRepository<T>, DataSeeder
│   │   ├── Logging/                   # FileLogger
│   │   └── Configuration/            # AppSettings
│   └── ComicBookShop.CLI/            # Console UI entry point
│       ├── Menus/                     # MainMenu, ComicBookMenu, CustomerMenu, OrderMenu, ReportMenu
│       ├── Helpers/                   # ConsoleHelper, InputValidator
│       ├── Program.cs                 # Composition root (DI + event wiring)
│       └── appsettings.json           # Configuration
├── tests/
│   └── ComicBookShop.Tests/          # xUnit + Moq tests
│       ├── Unit/                      # ComicBookServiceTests, CustomerServiceTests, OrderServiceTests, InventoryManagerTests
│       └── Integration/              # JsonRepositoryTests
└── README.md
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS)
- Terminal / command prompt
- (Optional) Visual Studio 2022+ or VS Code with C# extension

---

## Build & Run

```bash
# Navigate to the project root
cd CapstoneThree

# Create the solution and link projects (first time only)
dotnet new sln -n ComicBookShop
dotnet sln add src/ComicBookShop.Core
dotnet sln add src/ComicBookShop.Infrastructure
dotnet sln add src/ComicBookShop.CLI
dotnet sln add tests/ComicBookShop.Tests

# Restore packages and build
dotnet build

# Run the application
dotnet run --project src/ComicBookShop.CLI
```

---

## Run Tests

```bash
dotnet test --verbosity normal
```

Expected: **30+ tests** across 5 test classes covering service logic, validation, events, concurrency, and file persistence.

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

## Sample Data

The application seeds **15 comic books** and **5 customers** on first run:

**Comics** include: The Amazing Spider-Man, Batman: The Dark Knight Returns, X-Men: Days of Future Past, Saga, The Walking Dead, Archie, Blankets, The Sandman, Akira, Watchmen, Spawn, Hellboy, One Piece, Bone, Scott Pilgrim.

**Customers**: Tony Parker (Bronze), Diana Prince (Silver), Bruce Wayne (Gold), Clark Kent (Platinum), Peter Parker (Bronze).

Data is stored in the `data/` folder as JSON files and persists between sessions.

---

## Configuration

Edit `src/ComicBookShop.CLI/appsettings.json`:

```json
{
  "AppSettings": {
    "DataDirectory": "data",
    "LogDirectory": "logs",
    "LogLevel": "Information",
    "LowStockThreshold": 5,
    "ShopName": "Kapow! Comic Book Shop"
  }
}
```

---

## Membership Discounts

| Tier     | Discount |
| -------- | -------- |
| Bronze   | 0%       |
| Silver   | 5%       |
| Gold     | 10%      |
| Platinum | 15%      |

---

## License

Educational project — created for the C# & .NET Fundamentals capstone assignment.
