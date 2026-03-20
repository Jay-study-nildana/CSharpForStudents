# Day 17 — Dependency Injection & IoC: Problem Set (10 exercises)

Instructions
- Solve each problem using C# and .NET patterns from class (constructor injection, lifetimes, factories, decorators, testing with fakes/mocks, composition root).
- Each problem has a title; the provided solution files match these titles (filename = Title.cs).
- For coding problems, create minimal compilable classes or snippets that demonstrate the required change or pattern. When tests are requested, provide a short unit or integration test snippet.
- Submit: a short note (2–6 lines) describing your change and why it improves testability or correctness.

Problems

1) ConstructorInjectionRefactor
- Given a class that creates its own dependencies with `new`, refactor it to use constructor injection. Show the original anti-pattern briefly and the refactored class that depends on an interface. Provide a small usage example and a one-line explanation.

2) ReplaceServiceLocator
- A legacy class uses a global `ServiceLocator` (or calls `IServiceProvider.GetService` inside business code). Refactor it to accept dependencies via constructor injection. Explain how this change improves testability and what to register in the composition root.

3) FixSingletonCapturingScoped
- Identify a pattern where a singleton service captures a scoped `DbContext` (or other scoped service) and explain why it's wrong. Provide two correct fixes: converting the dependent service to scoped, and using `IServiceScopeFactory` inside the singleton. Show code for both fixes and a one-line recommendation.

4) BackgroundWorkerWithScopedServices
- Implement a `BackgroundService` (or `IHostedService`) that needs to run periodic DB work. The service must not accept `AppDbContext` in its constructor. Provide an implementation that uses `IServiceScopeFactory` to create a scope inside the worker loop and uses a scoped repository.

5) RegisterAppropriateLifetimes
- Given three example components — a stateless formatting helper, a per-request EF DbContext, and an in-memory cache — write the correct DI registrations showing `AddTransient`, `AddScoped`, and `AddSingleton` and explain the rationale in one sentence each.

6) ImplementDecoratorForIEmailSender
- Implement an `IEmailSender` interface and two classes: `SmtpEmailSender` (core) and `LoggingEmailSender` (decorator that logs then delegates to inner). Show how to register the decorator with DI so that resolving `IEmailSender` yields the logging decorator wrapping the SMTP implementation.

7) UseFactoryDelegateForRuntimeParams
- Some services need a runtime parameter to create, e.g., `IReportGenerator Create(string templateName)`. Implement and register a factory delegate (e.g., `Func<string, IReportGenerator>`) that resolves the right implementation at runtime without using a global service locator.

8) UnitTestWithFakeRepository
- Write a unit test for a service class that uses an `ISampleRepository`. Provide a small hand-rolled fake repository and an xUnit test that demonstrates the service behavior without touching a real DB.

9) IntegrationTest_UseInMemoryDb
- Show a minimal example how to configure a test host/WebApplicationFactory to replace the production `AppDbContext` with an in-memory database for integration tests. Provide a one-paragraph explanation of when to use in-memory vs full integration (test DB).

10) CompositionRootSetup
- Provide a `Program.cs`-style composition root that registers the services used across the exercises: repositories, application services, background worker, logging, and DbContext (show only the wiring). Explain in one sentence why composition root is the only location that should `new` concrete implementations.

Deliverables
- One Markdown file (`DI-Problems.md`) with the 10 problems (this file).
- Ten C# files with solutions named:
  - ConstructorInjectionRefactor.cs
  - ReplaceServiceLocator.cs
  - FixSingletonCapturingScoped.cs
  - BackgroundWorkerWithScopedServices.cs
  - RegisterAppropriateLifetimes.cs
  - ImplementDecoratorForIEmailSender.cs
  - UseFactoryDelegateForRuntimeParams.cs
  - UnitTestWithFakeRepository.cs
  - IntegrationTest_UseInMemoryDb.cs
  - CompositionRootSetup.cs

Good luck — solve the problems, then compare your implementations to the provided solution files.