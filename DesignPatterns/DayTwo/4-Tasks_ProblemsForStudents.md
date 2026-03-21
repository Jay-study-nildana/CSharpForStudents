# Day 2 — Exercises: Singleton & Factory Method

Instructions: Complete the 10 exercises below. Each problem focuses on creational patterns (Singleton and Factory Method) and related lifetime/DI decisions. After you solve each problem, compare your implementation to the provided solution file with the matching name (e.g., problem "01-SingletonLogger" → solution file "01-SingletonLogger.cs"). For lab submission, include a short paragraph explaining your lifetime choices and testability considerations.

1) 01-SingletonLogger  
Problem: Implement a basic Singleton logger class with a global access point `Logger.Instance` and a `Log(string message)` method. Make the simplest working implementation (may be non-thread-safe). In comments, list two problems that can occur with this naive approach.

2) 02-ThreadSafeSingletonLazy  
Problem: Implement a thread-safe singleton logger using `Lazy<T>` or double-checked locking. Ensure the instance is lazily initialized and thread-safe. Add a short comment explaining why your approach is safe in multithreaded scenarios.

3) 03-DIManagedSingletonVsManual  
Problem: Show two approaches for the same logger service: (A) register and consume it as a DI-managed singleton (sketch the registration code), and (B) a manual singleton implementation (class-managed). In comments, list at least three tradeoffs between these approaches (testability, lifecycle control, dependencies).

4) 04-SingletonWithMutableState  
Problem: Implement a singleton-like cache that stores key/value pairs. First show a naive singleton cache that uses a Dictionary (explain the problem). Then refactor to a thread-safe alternative (e.g., `ConcurrentDictionary` or make operations immutable). Explain why the refactor is safer.

5) 05-FactoryMethodNotifier  
Problem: Implement the Factory Method pattern for a notification system:
- `INotifier` interface with `Notify(string message)`.
- `NotifierCreator` abstract base class with a `protected abstract INotifier CreateNotifier()` factory method and a public `Send(string message)` method that uses it.
- Two concrete creators: `EmailNotifierCreator` and `SmsNotifierCreator`.
Explain when Factory Method is preferable to direct construction.

6) 06-FactoryMethodWithConfig  
Problem: Implement a creator that selects the concrete product based on runtime configuration (e.g., a `mode` string passed into the creator's constructor). Use the Factory Method pattern and ensure clients call the creator without referencing concrete product types.

7) 07-FactoryDelegateWithDI  
Problem: Implement a simple factory-delegate approach: define a `Func<string, INotifier>` factory that returns an `INotifier` for a given key (e.g., "email" or "sms"). Show how a consumer (`NotificationService`) uses the delegate to obtain an `INotifier`. Add a commented snippet showing how this factory could be registered in `IServiceCollection`.

8) 08-FactoryReturningDisposable  
Problem: Create a factory that returns disposable resource instances (e.g., `IDatabaseConnection : IDisposable`). Show correct usage in a consumer where the resource is used inside a `using` / `await using` pattern. Explain the lifetime choice for resources returned by the factory.

9) 09-FactoryMethodTestability  
Problem: Demonstrate how Factory Method improves testability. Provide a `TestNotifierCreator` that returns a fake notifier for unit tests. Show a short example test-usage snippet (conceptual code is fine) that asserts the consumer uses the fake notifier.

10) 10-SingletonVsFactoryDesignNotes  
Problem: Write a short design-note file that summarizes when to prefer Singleton (DI-managed) vs Factory Method vs Factory Delegate. Provide a decision checklist with at least five questions to guide pattern choice in a real .NET application.

Deliverables:
- Implementations for problems 1–9 as C# files (one file per problem).  
- For problem 10 provide a concise design-note file (text or comment-block in a C# file).  
- For each solution file, include a top comment summarizing the concept and the rationale for the chosen lifetime or approach.