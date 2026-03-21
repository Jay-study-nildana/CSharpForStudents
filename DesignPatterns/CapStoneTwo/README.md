# Smart Task Management System

### Design Patterns Capstone — C# & .NET Course

A fully-featured console application that demonstrates **every major design pattern** from the 12-day curriculum, wired together into a working, testable system.

---

Note : Built with Claude Sonnet 4.6 which is 1X premium request with GitHub CoPilot Pro. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference). 

## Quick Start

```bash
# Build the solution
dotnet build

# Run the interactive console app
dotnet run --project src/TaskManagement.App

# Run all unit tests
dotnet test
```

---

## Solution Structure

```
CapStoneTwo/
├── src/
│   ├── TaskManagement.Core/              # Domain + all pattern implementations
│   │   ├── Domain/
│   │   │   ├── TaskItem.cs               # Core entity (Builder output)
│   │   │   └── TaskGroup.cs             # Composite pattern (leaf + group)
│   │   ├── Interfaces/
│   │   │   └── IInterfaces.cs           # All abstractions (Repository, Strategy, Command, …)
│   │   ├── Patterns/
│   │   │   ├── Creational/
│   │   │   │   ├── TaskBuilder.cs        # Builder pattern
│   │   │   │   ├── TaskFactory.cs        # Factory Method pattern
│   │   │   │   └── NotificationFactory.cs # Abstract Factory pattern
│   │   │   ├── Structural/
│   │   │   │   ├── TaskServiceDecorators.cs # Decorator pattern (Logging + Validation)
│   │   │   │   └── FacadeBridgeProxy.cs  # Facade, Bridge, and Proxy patterns
│   │   │   └── Behavioral/
│   │   │       ├── PriorityStrategies.cs # Strategy pattern
│   │   │       ├── TaskProcessor.cs      # Template Method pattern
│   │   │       ├── TaskEventBus.cs       # Observer pattern
│   │   │       ├── Commands.cs           # Command pattern + undo/redo history
│   │   │       ├── ValidationChain.cs    # Chain of Responsibility pattern
│   │   │       └── WorkflowMediator.cs   # Mediator pattern
│   │   └── TaskService.cs               # Core service (uses Strategy + UoW)
│   │
│   ├── TaskManagement.Infrastructure/    # Technical concerns (persistence, logging, adapters)
│   │   ├── Persistence/
│   │   │   ├── JsonFileTaskRepository.cs # Repository pattern (JSON file store)
│   │   │   └── JsonFileUnitOfWork.cs     # Unit of Work pattern
│   │   ├── Logging/
│   │   │   └── AppLogger.cs             # Singleton pattern (DI-managed)
│   │   └── Adapters/
│   │       └── LegacyTaskAdapter.cs     # Adapter pattern (legacy CSV → domain)
│   │
│   └── TaskManagement.App/              # Entry point + console UI
│       ├── UI/
│       │   └── ConsoleMenu.cs           # Interactive menu shell
│       └── Program.cs                   # Composition Root (DI wiring)
│
└── tests/
    └── TaskManagement.Tests/
        └── CapstoneTests.cs             # 20 unit tests covering all major patterns
```

---

## Design Patterns Covered

| #   | Pattern                     | Category    | Where in Code                                                                                                      | Curriculum Day |
| --- | --------------------------- | ----------- | ------------------------------------------------------------------------------------------------------------------ | -------------- |
| 1   | **SOLID / DI**              | Principle   | `Program.cs` — all registrations via `IServiceCollection`                                                          | Day 1          |
| 2   | **Singleton**               | Creational  | `AppLogger` — registered `AddSingleton`; one instance app-wide                                                     | Day 2          |
| 3   | **Factory Method**          | Creational  | `TaskFactory.cs` — `BugTaskFactory`, `FeatureTaskFactory`, `ChoreTaskFactory`                                      | Day 2          |
| 4   | **Abstract Factory**        | Creational  | `NotificationFactory.cs` — `ConsoleNotificationProviderFactory` / `EmailNotificationProviderFactory`               | Day 3          |
| 5   | **Builder**                 | Creational  | `TaskBuilder.cs` — fluent API for constructing immutable `TaskItem` objects                                        | Day 3          |
| 6   | **Repository**              | Data Access | `JsonFileTaskRepository.cs` — `ITaskRepository` implementation over JSON                                           | Day 4          |
| 7   | **Unit of Work**            | Data Access | `JsonFileUnitOfWork.cs` — flushes all repo changes atomically                                                      | Day 4          |
| 8   | **Adapter**                 | Structural  | `LegacyTaskAdapter.cs` — converts legacy pipe-delimited CSV to domain `TaskItem`                                   | Day 5          |
| 9   | **Facade**                  | Structural  | `WorkflowFacade` in `FacadeBridgeProxy.cs` — one call does create+assign+notify+log                                | Day 5          |
| 10  | **Decorator**               | Structural  | `TaskServiceDecorators.cs` — `LoggingDecorator` and `ValidationDecorator` stacked over core service                | Day 6          |
| 11  | **Proxy**                   | Structural  | `LazyTaskServiceProxy` in `FacadeBridgeProxy.cs` — defers real service creation until first use                    | Day 6          |
| 12  | **Composite**               | Structural  | `TaskGroup.cs` — `TaskGroup` (composite) + `TaskLeaf` (leaf) with uniform `ITaskComponent`                         | Day 7          |
| 13  | **Bridge**                  | Structural  | `TaskReportGenerator` + `IReportRenderer` — abstraction decoupled from `ConsoleReportRenderer`/`CsvReportRenderer` | Day 7          |
| 14  | **Strategy**                | Behavioral  | `PriorityStrategies.cs` — `PriorityDescendingStrategy`, `DueDateAscendingStrategy`, `StatusGroupingStrategy`       | Day 8          |
| 15  | **Template Method**         | Behavioral  | `TaskProcessor.cs` — invariant pipeline with `PreProcess`/`DoWork`/`PostProcess` overridable steps                 | Day 8          |
| 16  | **Observer**                | Behavioral  | `TaskEventBus.cs` — pub/sub bus; `LoggingTaskEventListener` and `ConsoleAlertEventListener`                        | Day 9          |
| 17  | **Mediator**                | Behavioral  | `WorkflowMediator.cs` — coordinates service, event bus, and logger without direct coupling                         | Day 9          |
| 18  | **Command + Undo/Redo**     | Behavioral  | `Commands.cs` — `AssignTaskCommand`, `CompleteTaskCommand`, `CommandHistory` with full undo/redo                   | Day 10         |
| 19  | **Chain of Responsibility** | Behavioral  | `ValidationChain.cs` — `TitleValidationHandler` → `DueDateValidationHandler` → `PriorityConsistencyHandler`        | Day 10         |

---

## Architecture Overview

```
┌──────────────────── Console Menu ──────────────────────┐
│  (user input)                                           │
└──────┬───────────────────────────────────────┬──────────┘
       │                                       │
  WorkflowFacade                         CommandHistory
  (Facade pattern)                       (Command + Undo)
       │                                       │
  ┌────▼────────────────────────────────────────────────┐
  │         ITaskService  (decorated chain)              │
  │  ValidationDecorator → LoggingDecorator → TaskService│
  └────┬──────────────────────────────────────────┬──────┘
       │                                          │
  IUnitOfWork                             IPriorityStrategy
  (Unit of Work)                          (Strategy)
       │
  ITaskRepository
  (Repository)
       │
  JsonFileTaskRepository
  (tasks.json)
```

**Event flow:**

```
TaskService.CreateTask()
    → TaskEventBus.PublishCreated()
        → LoggingTaskEventListener.OnTaskCreated()
        → ConsoleAlertEventListener.OnTaskCreated()
    → WorkflowMediator.NotifyAsync("TaskCreated")
```

---

## How the Decorator Chain Works

```csharp
// In Program.cs (Composition Root):
ITaskService decorated = new TaskService(uow, strategy);           // 1. Core
decorated = new ValidationTaskServiceDecorator(decorated, chain);  // 2. Validation
decorated = new LoggingTaskServiceDecorator(decorated, logger);    // 3. Logging
```

Every `CreateTaskAsync` call passes through:

1. **Logging decorator** — records entry/exit
2. **Validation decorator** — runs the CoR chain; throws on failure
3. **Core TaskService** — creates task via Factory Method, persists via Repository/UoW

---

## Running the App

On first launch, 3 sample tasks are imported from the built-in legacy CSV adapter:

```
[LEGACY-001] Fix login page crash   Bug     High    Alice
[LEGACY-002] Add dark mode support  Feature Medium  Bob
[LEGACY-003] Update dependencies    Chore   Low     (unassigned)
```

Then the Composite tree view and Template Method processors run, followed by the interactive menu:

```
  1. Create Task        ← Factory Method + Builder + Validation CoR + Observer
  2. List Tasks         ← Repository + Strategy (sorted)
  3. Assign Task        ← Command (stored in CommandHistory for undo)
  4. Complete Task      ← Command (undoable)
  5. Undo Last Action   ← CommandHistory.UndoAsync()
  6. Redo Last Action   ← CommandHistory.RedoAsync()
  7. Show Report        ← Bridge (TaskReportGenerator + ConsoleReportRenderer)
  8. Show Summary       ← Mediator
  9. Show Log           ← Singleton Logger history
  0. Exit
```

Data is persisted to `<app>/data/tasks.json` between runs.

---

## Unit Tests (20 tests, all passing)

| Test Class              | Tests | Patterns Verified       |
| ----------------------- | ----- | ----------------------- |
| `TaskBuilderTests`      | 3     | Builder                 |
| `TaskFactoryTests`      | 4     | Factory Method          |
| `ValidationChainTests`  | 3     | Chain of Responsibility |
| `PriorityStrategyTests` | 2     | Strategy                |
| `TaskEventBusTests`     | 2     | Observer                |
| `TaskGroupTests`        | 3     | Composite               |
| `CommandHistoryTests`   | 2     | Command + Undo/Redo     |
| `DecoratorTests`        | 1     | Decorator               |

```bash
dotnet test
# Test summary: total: 20, failed: 0, succeeded: 20
```

---

## Extension Ideas (Day 12 / follow-on projects)

- **Swap the renderer**: change `ConsoleReportRenderer` to `CsvReportRenderer` in `Program.cs` — Bridge pattern makes this a one-line change.
- **Add a Slack notification channel**: create `SlackNotificationProviderFactory` — Abstract Factory makes this isolated.
- **Add a new sort strategy**: implement `IPriorityStrategy` — Strategy makes this open/closed.
- **Persist to SQLite**: replace `JsonFileUnitOfWork` — Repository/UoW ensures the rest of the code is unaffected.
- **Add authentication**: wrap `ITaskService` in an `AuthorizationTaskServiceDecorator` — decorator chain grows without touching existing code.

---

## Technologies Used

- **.NET 9** / C# 13
- **Microsoft.Extensions.DependencyInjection** — DI container (composition root)
- **System.Text.Json** — JSON persistence
- **xUnit** — unit testing framework
- **Moq** — mocking library for test doubles
