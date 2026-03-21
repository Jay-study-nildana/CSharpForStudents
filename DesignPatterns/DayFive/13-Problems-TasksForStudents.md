# Day 5 — Exercises: Adapter & Facade (10 Problems)

Instructions
- Solve the 10 problems below. Each problem focuses on Adapter and Facade patterns, integration with DI, anti-corruption layers, testing, and refactoring legacy code.
- Implement each solution in a separate C# file named exactly as the problem title (prefix with two-digit ID). Example: Problem "01-Adapter_LegacyPayment.cs" → solution file "01-Adapter_LegacyPayment.cs".
- At the top of each solution file include a short comment summarizing the pattern intent, DI registration/lifetime recommendation, and one testability note.
- Keep examples small and focused so they can be used in class demos or turned into runnable samples later.

Problems

1) 01-Adapter_LegacyPayment.cs  
Problem: You have a legacy payment processor class with `string ProcessPayment(int cents, string currencyCode)` that returns "OK" or error codes. Define an `IPaymentGateway` interface (`bool Charge(decimal amount, string currency)`) and implement `LegacyPaymentAdapter` that adapts the legacy API. Include a short comment about unit testing the adapter.

2) 02-Adapter_LoggingAdapter.cs  
Problem: A legacy logging library exposes `void Write(string level, string message)`. Define an `ILogger` interface (`void Info(string)`, `void Error(string)`) and implement `LegacyLoggingAdapter` that maps calls to the legacy API. Demonstrate usage in a small `ProcessingService`.

3) 03-Facade_NotificationFacade.cs  
Problem: Implement `NotificationFacade` that composes `ILogger`, `IMetrics`, and `IEmailSender` to provide a single `SendNotification(string email, string subject, string body)` method. Add a brief example showing how clients call the facade instead of multiple subsystems.

4) 04-Facade_UserOnboarding.cs  
Problem: Design a `UserOnboardingFacade` that performs these steps: create user in repository, send welcome email via `NotificationFacade`, record onboarding metric, and schedule a welcome task. Provide the public `Onboard(UserDto user)` method and a short pseudo-flow inside the facade. Comment on lifetimes and transaction considerations.

5) 05-Adapter_AntiCorruptionLayer.cs  
Problem: A third-party system models addresses differently. Implement an Anti-Corruption Adapter `ThirdPartyAddressAdapter` that converts `ThirdPartyAddress` to your domain `Address` and exposes a domain-friendly API for address validation/normalization. Explain why an anti-corruption layer may be preferred over direct use of third-party models.

6) 06-AdapterAndFacade_DIRegistration.cs  
Problem: Provide conceptual `IServiceCollection` registration snippets (comments) showing how to register adapters and facades. Include choices for lifetimes (Transient/Scoped/Singleton) and a short rationale for each choice.

7) 07-Adapter_Tests.cs  
Problem: Provide example unit-test style code (conceptual) that shows how to test `LegacyPaymentAdapter` and `NotificationFacade` using fakes or mocks. Include assertions / verifications in comments to show what to check.

8) 08-Facade_CompositionAndErrorHandling.cs  
Problem: Implement a `PaymentFacade` that composes payment gateway, logger, and retry logic. Show high-level retry with a configurable attempt count and error handling that logs and returns a result object. Comment on why facades are good places to centralize retry/backoff and error translation.

9) 09-RefactorLegacyClientToUseFacade.cs  
Problem: Given legacy client code that calls logger, metrics, and email sender directly, provide a refactor where the client depends on `NotificationFacade`. Include before/after comments demonstrating improved readability and testability.

10) 10-PluginAdaptersRegistry.cs  
Problem: Implement a `PaymentAdapterRegistry` that maps provider keys (e.g., "legacy", "stripe") to `IPaymentGateway` adapters and returns an adapter by key. Provide a `PaymentOrchestrator` that accepts the registry and uses the chosen adapter. Comment on plugin loading strategies and DI integration.

Deliverables
- Day5_Exercises.md (this document)
- Ten C# solution files named exactly as the problem titles (01–10).