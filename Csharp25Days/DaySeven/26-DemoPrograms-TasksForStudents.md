# Day 7 — Instance vs Static Practice Problems

Instructions: For each problem below, implement a short C# program or class demonstrating the requested concept. Include:
- Which members are static vs instance and why.
- A brief note on cohesion and single responsibility.
- A small Main/test demonstrating the behavior.

Problems:

1. InstanceVsStatic_Decision  
   - Given a small example (e.g., Math helpers and a Counter class), decide which members should be static and which should be instance. Implement both and demonstrate usage.

2. Convert_Helper_To_Service  
   - You have a static helper `StringFormatter.FormatName(string)`. Refactor it into an instance-based `IStringFormatter` service suitable for dependency injection and unit testing. Provide a small demo showing mocking/faking.

3. Testable_IdGenerator  
   - Implement two Id generators: a static `IdGenerator.StaticNext()` and a testable instance `IIdGenerator` implementation. Show how the instance approach is easier to unit-test.

4. StaticCache_ThreadSafe  
   - Implement a thread-safe static cache using `ConcurrentDictionary` and an instance cache using `Dictionary` plus locking. Explain trade-offs and show a simple concurrent usage demo.

5. Instance_Methods_For_OrderProcessing  
   - Model `Order` (instance data) with instance methods `AddItem`, `Total`, and an `OrderService` that takes dependencies (e.g., tax provider). Demonstrate calculating an order total via instance methods.

6. Refactor_Monolithic_Class  
   - Given a description of a monolithic `ReportGenerator` that fetches data, formats it, and writes to disk, refactor into cohesive pieces: `IDataFetcher`, `IReportFormatter`, `IReportWriter`, and a small composition example.

7. Cohesion_Assessment  
   - Provide two small classes: one high-cohesion (e.g., `TemperatureSensor` with related methods) and one low-cohesion (a "God" class stub). Explain in comments why one is preferable.

8. Overload_Static_Instance  
   - Demonstrate overload resolution and the difference between static helper overloads and instance method overloads. Show how `this` instance state can change behavior of an overload.

9. DependencyInjection_For_Testing  
   - Show a class `EmailSender` that depends on `ISmtpClient`. Provide a fake `ISmtpClient` for testing and show how injecting the dependency avoids static calls and enables tests.

10. UML_Text_Outline  
    - Produce a textual UML-like outline (classes, fields, methods, visibility) for a small domain (e.g., Library system: Book, Member, Loan, Library). Ensure responsibilities are clear and cohesion is high.

Deliverable: For each problem create a C# file named exactly as the problem title (use underscores as shown above). Each file should be a small, self-contained console program demonstrating the solution and printing a short explanation when run.