# SmartStore — Design Patterns Capstone Project

A fully self-contained **.NET 8 console application** that demonstrates **16 design patterns** (12 GoF + Repository + Unit of Work + DI/Singleton) in a coherent, runnable domain scenario: an order-management system for an online store.

---

Note : Built with Claude Sonnet 4.6 which is 1X premium request with GitHub CoPilot Pro. (the entire curricullum was built with GPT5-mini but mentioning additional details for future reference). 

## How to Run

```bash
cd src/SmartStore
dotnet run
```

Or from the solution root:

```bash
dotnet run --project src/SmartStore/SmartStore.csproj
```

Press **Enter** to step through each demo section.

> **Tip:** To switch to the dark console theme, open `Program.cs` and change `useDarkTheme: false` → `useDarkTheme: true`.

---

## Patterns Demonstrated

### Creational Patterns

| Pattern                | File                                                | What it shows                                                                                                                                                                                         |
| ---------------------- | --------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Builder**            | `Patterns/Creational/OrderBuilder.cs`               | Fluent step-by-step construction of complex `Order` objects with optional items, bundles, and notes.                                                                                                  |
| **Factory Method**     | `Patterns/Creational/NotificationChannelFactory.cs` | Abstract factory method `CreateChannel()` — subclasses decide whether to create a `ConsoleNotificationChannel` or `EmailNotificationChannel`.                                                         |
| **Abstract Factory**   | `Patterns/Creational/ThemeFactory.cs`               | `IThemeFactory` produces a coherent family of UI renderers (`IHeaderRenderer`, `ITableRenderer`, `IStatusRenderer`). Swap `LightThemeFactory` ↔ `DarkThemeFactory` to change the entire console look. |
| **Singleton (via DI)** | `DI/AppServiceContainer.cs`                         | `AppServiceContainer` is the composition root. Singletons (`EventManager`, `CommandHistory`, `AuditLog`) are created once and injected. Compares to naive static singletons.                          |

### Structural Patterns

| Pattern       | File                                                     | What it shows                                                                                                                                                                                                          |
| ------------- | -------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Adapter**   | `Patterns/Structural/LegacyPaymentAdapter.cs`            | `LegacyPaymentProcessor` takes cents as int and returns a string. `LegacyPaymentAdapter` makes it fit `IPaymentGateway` (decimal, bool) without touching the legacy code.                                              |
| **Facade**    | `Patterns/Structural/CheckoutFacade.cs`                  | `Checkout()` hides the orchestration of 5 subsystems: validation chain, pricing strategy, payment adapter, repository/UoW persistence, and observer notifications.                                                     |
| **Decorator** | `Patterns/Structural/LoggingOrderRepositoryDecorator.cs` | Wraps any `IOrderRepository` to add structured logging transparently. Stacking is trivial — add a caching decorator in the same way.                                                                                   |
| **Proxy**     | `Patterns/Structural/CachedProductRepositoryProxy.cs`    | Controls access to `InMemoryProductRepository`. First call loads data; subsequent calls serve from cache. `InvalidateCache()` resets it.                                                                               |
| **Composite** | `Patterns/Structural/BundleOrderItem.cs`                 | `BundleOrderItem` is a composite node; `OrderItem` is a leaf. Both extend `OrderItemBase`. `Order.SubTotal` calls `GetTotalPrice()` uniformly on all items.                                                            |
| **Bridge**    | `Patterns/Structural/NotificationBridge.cs`              | Abstraction hierarchy (`OrderConfirmedNotification`, `OrderCancelledNotification`) is decoupled from implementation hierarchy (`ConsoleNotificationChannel`, `EmailNotificationChannel`). Compose any pair at runtime. |

### Behavioral Patterns

| Pattern                     | File                                                        | What it shows                                                                                                                                                                                                          |
| --------------------------- | ----------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Strategy**                | `Patterns/Behavioral/Strategies/PricingStrategies.cs`       | `RegularPricingStrategy`, `DiscountPricingStrategy`, `VipPricingStrategy` — swappable at runtime, selected by customer type via the DI container.                                                                      |
| **Template Method**         | `Patterns/Behavioral/OrderProcessingTemplate.cs`            | `Process()` is the invariant skeleton. `StandardOrderProcessor` and `ExpressOrderProcessor` override `ValidateOrder`, `ReserveStock`, and `ApplyBusinessRules` without changing the sequence.                          |
| **Observer**                | `Patterns/Behavioral/OrderEventManager.cs`                  | `OrderEventManager` (subject) broadcasts events to subscribed `IOrderObserver` instances (`EmailObserver`, `InventoryObserver`, `AuditLogObserver`). Demonstrates subscribe, unsubscribe, and multi-cast notification. |
| **Mediator**                | `Patterns/Behavioral/CheckoutMediator.cs`                   | `CartComponent`, `PaymentComponent`, and `SummaryComponent` communicate only through `ConcreteCheckoutMediator`. No component holds a reference to another.                                                            |
| **Command + Undo**          | `Patterns/Behavioral/Commands/OrderCommands.cs`             | `PlaceOrderCommand` and `CancelOrderCommand` implement `ICommand`. `CommandHistory` maintains a stack — call `Undo()` to reverse the last command.                                                                     |
| **Chain of Responsibility** | `Patterns/Behavioral/ValidationChain/ValidationHandlers.cs` | `EmptyCartValidationHandler → StockValidationHandler → MinimumOrderValueHandler`. Any handler can short-circuit with a failure or pass control to the next.                                                            |

### Data Access Patterns

| Pattern          | File                                        | What it shows                                                                                                    |
| ---------------- | ------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| **Repository**   | `Infrastructure/InMemoryOrderRepository.cs` | Abstracts data access behind `IOrderRepository`. Swap for an EF Core implementation without touching the domain. |
| **Unit of Work** | `Infrastructure/InMemoryUnitOfWork.cs`      | Groups multiple repository operations into one `Commit()` / `Rollback()` boundary.                               |

---

## Project Structure

```
CapStoneOne/
├── SmartStore.sln
├── README.md
└── src/
    └── SmartStore/
        ├── SmartStore.csproj
        ├── GlobalUsings.cs          ← project-wide using declarations
        ├── Program.cs               ← composition root + 15 demo sections
        │
        ├── Core/
        │   ├── Domain/              ← pure domain entities (no dependencies)
        │   │   ├── Customer.cs
        │   │   ├── Product.cs
        │   │   ├── OrderStatus.cs
        │   │   ├── OrderItemBase.cs ← Composite: component
        │   │   ├── OrderItem.cs     ← Composite: leaf
        │   │   └── Order.cs
        │   └── Interfaces/          ← contracts the domain depends upon
        │       ├── IOrderRepository.cs
        │       ├── IProductRepository.cs
        │       ├── IUnitOfWork.cs
        │       ├── IPricingStrategy.cs
        │       ├── ICommand.cs
        │       ├── INotificationChannel.cs
        │       └── IPaymentGateway.cs
        │
        ├── Patterns/
        │   ├── Creational/
        │   │   ├── OrderBuilder.cs
        │   │   ├── NotificationChannelFactory.cs
        │   │   └── ThemeFactory.cs
        │   ├── Structural/
        │   │   ├── LegacyPaymentAdapter.cs
        │   │   ├── CheckoutFacade.cs
        │   │   ├── LoggingOrderRepositoryDecorator.cs
        │   │   ├── CachedProductRepositoryProxy.cs
        │   │   ├── BundleOrderItem.cs
        │   │   └── NotificationBridge.cs
        │   └── Behavioral/
        │       ├── Strategies/
        │       │   └── PricingStrategies.cs
        │       ├── OrderProcessingTemplate.cs
        │       ├── OrderEventManager.cs
        │       ├── CheckoutMediator.cs
        │       ├── Commands/
        │       │   └── OrderCommands.cs
        │       └── ValidationChain/
        │           └── ValidationHandlers.cs
        │
        ├── Infrastructure/
        │   ├── InMemoryOrderRepository.cs
        │   ├── InMemoryProductRepository.cs
        │   └── InMemoryUnitOfWork.cs
        │
        └── DI/
            └── AppServiceContainer.cs   ← composition root / lifetime management
```

---

## Demo Sections (15 sections in Program.cs)

| #   | Section                        | Pattern(s)                  |
| --- | ------------------------------ | --------------------------- |
| 1   | Themed Console Header          | Abstract Factory            |
| 2   | Cached Product Catalog         | Proxy                       |
| 3   | Building Orders with Bundles   | Builder + Composite         |
| 4   | Order Validation Pipeline      | Chain of Responsibility     |
| 5   | Pricing Algorithms             | Strategy                    |
| 6   | Place & Cancel with Undo       | Command + Unit of Work      |
| 7   | Order Processing Steps         | Template Method             |
| 8   | Order Event Broadcasting       | Observer                    |
| 9   | Notification Delivery Channels | Bridge                      |
| 10  | Full Checkout Pipeline         | Facade + Adapter            |
| 11  | Checkout UI Coordination       | Mediator                    |
| 12  | Dynamic Channel Creation       | Factory Method              |
| 13  | Transparent Logging            | Decorator                   |
| 14  | Order Store Summary            | Repository + Unit of Work   |
| 15  | Audit Log Review               | Observer (AuditLogObserver) |

---

## SOLID Principles Applied

| Principle                     | Where                                                                                                                                |
| ----------------------------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| **S** — Single Responsibility | Each class has one reason to change (e.g., `LoggingOrderRepositoryDecorator` only logs, `StockValidationHandler` only checks stock). |
| **O** — Open/Closed           | Add a new pricing strategy or validation rule by adding a class — existing code unchanged.                                           |
| **L** — Liskov Substitution   | Any `IPricingStrategy`, `IOrderRepository`, or `INotificationChannel` can replace another transparently.                             |
| **I** — Interface Segregation | Separate interfaces: `IOrderRepository`, `IProductRepository`, `IUnitOfWork`, `IPaymentGateway`, etc.                                |
| **D** — Dependency Inversion  | `CheckoutFacade`, services, and commands depend on interfaces, not on concrete infrastructure.                                       |

---

## Suggested Extensions (Day 11 / 12 Lab Ideas)

- Add a `SmsNotificationChannel` (Bridge implementor) without touching existing notification code.
- Add a `BulkDiscountPricingStrategy` (new Strategy) and register it in `AppServiceContainer`.
- Add a `ShipOrderCommand` with redo support.
- Replace `InMemoryOrderRepository` with a JSON-file repository — the rest of the app stays the same.
- Add an `IOrderRepository` caching decorator (analogous to `CachedProductRepositoryProxy`).
- Write unit tests with a mock `IOrderRepository` and `IPaymentGateway`.

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Any terminal (cmd, PowerShell, bash)
- No external NuGet packages required
