# Day 8 — Inheritance & Polymorphism Practice Problems

Instructions: For each problem below implement the requested classes using C#. Prefer small, focused classes. For solutions include:
- Base and derived classes (or interfaces) as requested.
- Use of `virtual` / `override` or `abstract` where appropriate.
- A short Main demonstrating polymorphic use (treating derived objects as base type).
- A brief comment describing why inheritance (or composition) was chosen and any LSP considerations.

Problems:

1. Shape_Area_Override  
   - Create an abstract `Shape` base class with an abstract `double Area()` method. Implement `Circle` and `Rectangle` derived classes that override `Area`. Demonstrate computing total area for a list of `Shape`.

2. Animal_Speak_Polymorphism  
   - Implement a base `Animal` class with a virtual `Speak()` method and derived classes `Dog`, `Cat`, and `Bird` overriding it. Show polymorphic calls through a `List<Animal>`.

3. PaymentProcessor_Strategy  
   - Replace a conditional-based payment handler with a polymorphic design: base `PaymentMethod` with `Process(decimal amount)` and derived classes `CreditCardPayment`, `PayPalPayment`, `BankTransferPayment`. Demonstrate selecting and invoking the correct implementation.

4. Vehicle_Start_Virtual  
   - Create a `Vehicle` base class with a virtual `Start()` and derived classes `Car`, `ElectricScooter`, `DieselTruck` that override `Start` with different behavior/log messages. Show polymorphism and mention why `virtual` is used.

5. Report_TemplateMethod  
   - Implement a `ReportGenerator` abstract base with a `Generate()` template method that calls abstract `FetchData()` and `Format()` methods. Provide two concrete reporters (e.g., `SalesReport`, `InventoryReport`) and demonstrate calling `Generate()` polymorphically.

6. DiscountPolicy_Hierarchy  
   - Implement an abstract `DiscountPolicy` with `decimal Apply(decimal total)`; provide `NoDiscount`, `PercentageDiscount`, and `ThresholdDiscount` (e.g., fixed percent over threshold). Replace a switch/if chain with this hierarchy and demonstrate usage.

7. Serialization_Polymorphic_SaveLoad  
   - Design a base `Document` class with virtual `Serialize()` returning a string; implement `Invoice` and `Letter` overrides. Show saving/loading a list of `Document` polymorphically (serialize each). Mention format decisions and why polymorphism helps.

8. LSP_Violation_Fix  
   - Show a classic Rectangle/Square LSP problem: first show a `Rectangle` base and a `Square` derived that incorrectly overrides `set`ters causing LSP violation. Then refactor with a better design (e.g., `IShape` interface or separate classes without inheritance) and explain the fix.

9. Composition_vs_Inheritance_Refactor  
   - Given a `Printer` that prints text and optionally logs, show a design using composition: `IPrinter` + `Logger` decorator vs an inheritance-based approach. Implement a `LoggerPrinter` that composes an `IPrinter` and logs calls. Explain why composition was chosen.

10. Plugin_Interface_and_Loading  
    - Define an `IPlugin` interface with `string Name { get; }` and `void Run()`. Implement two plugins and show how a simple loader can discover and execute them via a `List<IPlugin>` (illustrate polymorphism through interfaces).

Hints:
- Keep hierarchies shallow and cohesive.
- Consider `abstract` when base shouldn't be instantiated.
- Document any Liskov Substitution Principle (LSP) concerns in comments, especially for problem 8.
- For tasks that could use composition instead, mention the alternative in comments and why you chose inheritance (or composition).