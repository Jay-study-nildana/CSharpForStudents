# Day 11 — Integration / Refactor Lab — Problems (C# / .NET)

Overview
- Goal: apply multiple design patterns to improve a small legacy-style codebase.
- Deliverables: for each problem produce a short design note, the refactored code (or code sketch), and a brief test plan (unit + integration steps).
- Each problem below should be solved in a single `.cs` file (console demo) named exactly as the problem title with `.cs` appended.

Instructions
- Implement minimal, focused refactors. Write characterization tests first where indicated.
- Keep demos short: show "before" behavior (via comment or small snippet), then the refactored approach with patterns applied.
- Use interfaces and DI-friendly constructors. Avoid frameworks—plain C# is fine.
- Each solution should include a Main() demonstrating expected behavior.

Problems
1. 01_ExtractRepository
   - Extract data-access from a service into IOrderRepository and InMemoryOrderRepository.
   - Demo: show service using repository for save/load instead of inline SQL.

2. 02_IntroduceDependencyInjection
   - Replace hard-coded new() calls in a legacy service with constructor injection.
   - Demo: register implementations in a simple composition root and swap a mock.

3. 03_ImplementFactoryBuilder
   - Replace complex inline construction of Invoice with an IInvoiceFactory and an InvoiceBuilder for multi-step creation.
   - Demo: create invoices using builder and factory.

4. 04_ApplyStrategyForTaxCalculation
   - Introduce ITaxStrategy to encapsulate tax calculation algorithms; wire it into BillingService.
   - Demo: swap two tax strategies at runtime and show different totals.

5. 05_EventBusObserverIntegration
   - Introduce a lightweight EventBus/IEventBus to decouple notifications (order charged, invoice created).
   - Demo: multiple subscribers (email, logging) receive events.

6. 06_RefactorToMediator
   - When many components interact (Editor, Autosave, Status), centralize coordination with a Mediator.
   - Demo: components register with mediator and coordinate save flow.

7. 07_IntroduceAdapterForLegacyApi
   - Wrap a legacy third-party or static API in an adapter to fit your interface (e.g., ILegacyPaymentAdapter -> IPaymentGateway).
   - Demo: adapter translates calls and shields rest of app from legacy details.

8. 08_CreateFacadeForSubsystem
   - Provide a Facade that simplifies several subsystem calls (Repository + Payment + Notification) into one high-level operation.
   - Demo: use facade to perform "ProcessOrder" in one call.

9. 09_AddDecoratorForCaching
   - Add a caching decorator around IProductRepository that caches GetById responses.
   - Demo: show caching avoids repeated underlying calls.

10. 10_CharacterizationTestsAndIncrementalRefactor
    - Write a small set of inline characterization checks (assertions) that capture a legacy behavior before refactor, then refactor and show assertions remain green.
    - Demo: run assertions before/after change in Main; throw on failure.

Hints & Evaluation
- Keep each solution self-contained and runnable.
- Demonstrate the pattern’s intent and one primary benefit (testability, decoupling, simplicity, performance).
- Include a one-line test plan comment at the top of each file explaining what to test.
- For characterization tests, use simple Assert helpers (throw on failure) to avoid external test runners.

Good luck — after you review these, I can:
- Package into a ZIP,
- Add xUnit tests for selected solutions,
- Or generate a short design doc + UML (PlantUML) for one chosen problem.