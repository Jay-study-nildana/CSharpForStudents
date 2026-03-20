# Day 9 — Interfaces & Abstract Classes: Practice Problems

Context: Interfaces as contracts, abstract classes, multiple-implementation patterns, and designing for testability and decoupling.

Instructions: For each problem below implement a small C# console program or class that demonstrates the requested design. For each solution include:
- The interface(s) and/or abstract class(es).
- One or more concrete implementations.
- A short Main demonstrating polymorphic usage or testability (fakes/mocks).
- A short comment explaining why the abstraction was chosen and any testability/decoupling notes.

Problems:

1. Define_IPaymentMethod  
   - Create an `IPaymentMethod` interface with `bool Process(decimal amount)`. Implement `CreditCardPayment` and `PayPalPayment`. In `Main`, process a payment using a list of `IPaymentMethod` values.

2. ILogger_and_Implementations  
   - Define `ILogger` and implement `ConsoleLogger` and `NullLogger` (null-object pattern). Show code that accepts `ILogger` and logs via the injected logger.

3. AbstractRepository_Pattern  
   - Create an abstract `Repository<T>` with abstract `T Get(int id)` and a concrete `InMemoryRepository<T>` implementing storage. Demonstrate fetching and saving via the abstract base.

4. Adapter_For_LegacyApi  
   - Given a legacy class `LegacyNotifier.SendMsg(string)` that cannot be changed, write an `INotifier` interface and an adapter `LegacyNotifierAdapter` that implements `INotifier`. Demonstrate using `INotifier` in client code.

5. Decorator_LoggingDecorator  
   - Implement `IService` and a concrete `RealService`. Create a `LoggingServiceDecorator` that wraps an `IService` and logs before/after calls. Demonstrate composing the decorator around the real service.

6. Composite_Notifier  
   - Implement `INotifier` and a `CompositeNotifier` that holds a list of `INotifier` and forwards `Notify(string)` to all children. Show adding multiple notifiers and notifying them.

7. Factory_For_Strategy  
   - Create an `IDiscountPolicy` interface and several implementations (`NoDiscount`, `PercentageDiscount`). Implement a simple factory `DiscountFactory.Create(string kind)` that returns appropriate instance. Demonstrate using the factory to get a policy and apply it.

8. Plugin_Discovery_Interface  
   - Define `IPlugin` with `void Run()` and create two plugin implementations. In `Main`, simulate discovery by placing implementations in a `List<IPlugin>` and executing them.

9. Fake_For_UnitTest  
   - Define `IEmailSender` and a `NotificationService` that depends on it. Implement a `FakeEmailSender` that records calls and demonstrate how a unit test could assert the fake's recorded state.

10. Interface_Segregation_Refactor  
    - Start from a fat interface `IMachine { void Print(); void Scan(); void Fax(); }` and refactor into smaller interfaces (`IPrinter`, `IScanner`, `IFax`). Implement `AllInOneMachine` implementing all three interfaces and `SimplePrinter` implementing only printing. Demonstrate why this improves decoupling.

Good practice: keep abstractions small and focused, inject dependencies into constructors, and prefer returning interfaces to hide implementations. For each problem, include a brief comment about testability and why you chose interface vs abstract class.