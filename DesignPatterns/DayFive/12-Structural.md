# Day 5 — Structural Patterns: Adapter & Facade

This lesson explains two important structural patterns: Adapter (bridge incompatible interfaces) and Facade (simplify complex subsystems). Read the intent, study the examples, and use the lab prompt to practice designing both patterns in C#.

---

## Quick overview — intent & when to use
- Adapter: Convert one interface into another expected by clients. Use when you need to integrate an existing/legacy/third‑party API whose interface doesn't match what your code expects.
- Facade: Provide a simplified unified interface to a set of subsystems. Use when you want to hide complexity and present a higher-level API for common workflows.

Both patterns improve separation of concerns and make client code easier to read, test, and evolve.

---

## Pattern anatomy (concise)
Adapter:
- Intent: Make two incompatible interfaces work together.
- Participants: Target (interface clients expect), Adaptee (existing/third-party class), Adapter (implements Target, delegates to Adaptee).
- Tradeoffs: Adds a small indirection layer; keeps integration code isolated.

Facade:
- Intent: Provide a simple interface to a group of classes or subsystems.
- Participants: Facade class (high-level API), Subsystems (many cooperating classes).
- Tradeoffs: Reduces coupling for clients but can become a god-object if it accumulates too much responsibility.

UML hints:
- Adapter: Target <- Adapter ; Adapter -- Adaptee (dependency or composition).
- Facade: Client --> Facade --> SubsystemA, SubsystemB (Facade depends on subsystems; client only depends on facade).

---

## Adapter — example (C#)
Scenario: Your app expects `IPaymentGateway` but a legacy provider exposes a different API.

```csharp
// Target interface the app expects
public interface IPaymentGateway
{
    bool Charge(decimal amount, string currency);
}

// Adaptee: legacy third-party API with incompatible method
public class LegacyPaymentProcessor
{
    // Legacy method signature and semantics differ
    public string ProcessPayment(int cents, string currencyCode)
    {
        // returns "OK" or error code
        return "OK";
    }
}

// Adapter: converts IPaymentGateway calls into LegacyPaymentProcessor usage
public class LegacyPaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentProcessor _processor;
    public LegacyPaymentAdapter(LegacyPaymentProcessor processor) => _processor = processor;

    public bool Charge(decimal amount, string currency)
    {
        // adapt decimal dollars to integer cents, map currency names, interpret legacy return.
        var cents = (int)(amount * 100m);
        var result = _processor.ProcessPayment(cents, currency);
        return result == "OK";
    }
}

// Usage (client code uses IPaymentGateway only)
public class CheckoutService
{
    private readonly IPaymentGateway _gateway;
    public CheckoutService(IPaymentGateway gateway) => _gateway = gateway;

    public bool Checkout(decimal amount) => _gateway.Charge(amount, "USD");
}
```

Notes:
- Adapter isolates adaptation logic (conversion of units, error mappings). Tests can mock IPaymentGateway without touching legacy code.
- Use Adapter when you cannot change the adaptee (third‑party or legacy) and when multiple parts of your system need the unified Target interface.

---

## Facade — example (C#)
Scenario: You want a single `NotificationFacade.Send()` that logs, records metrics, and sends an email without the client managing each subsystem.

```csharp
public interface ILogger { void Log(string msg); }
public interface IMetrics { void Increment(string key); }
public interface IEmailSender { void SendEmail(string to, string subject, string body); }

public class NotificationFacade
{
    private readonly ILogger _logger;
    private readonly IMetrics _metrics;
    private readonly IEmailSender _email;

    public NotificationFacade(ILogger logger, IMetrics metrics, IEmailSender email)
    {
        _logger = logger; _metrics = metrics; _email = email;
    }

    // Simplified high-level operation
    public void Send(string userEmail, string subject, string message)
    {
        _logger.Log($"Preparing to notify {userEmail}");
        _metrics.Increment("notifications.sent");
        _email.SendEmail(userEmail, subject, message);
        _logger.Log($"Notification sent to {userEmail}");
    }
}
```

Usage:
- Client code depends only on `NotificationFacade` and doesn't need to orchestrate logger, metrics, and email sender. This reduces duplication and centralizes cross-cutting concerns.

Design notes:
- Keep a facade focused on a coherent set of responsibilities (e.g., user notifications). If it grows too large, split into smaller facades.
- Facade can coordinate internal retry/backoff logic, error handling, and compositional workflows.

---

## Adapter vs Facade — quick decision guide
- Use Adapter when: you need to make an existing component conform to an expected interface. It’s about compatibility.
- Use Facade when: you want to present a simpler API over a complex/subsystem. It’s about simplification and orchestration.

They are complementary: you might adapt a third-party subsystem and then expose it via a facade to clients.

---

## DI, lifetimes & testing guidance
- Register adapters and facades as services in DI:
  - Adapter: typically Transient/Scoped, depending on the adaptee’s lifetime.
  - Facade: often Transient or Scoped; singleton only if injected dependencies are thread-safe singletons.
- Testability:
  - Client tests should depend on abstractions (interfaces) and use mocks/fakes of the adapter/facade.
  - Adapter allows mocking the Target interface without touching third‑party code.
  - Facade simplifies testing clients because only one dependency needs to be faked.

Example DI registration (conceptual):
```csharp
// services.AddTransient<IPaymentGateway, LegacyPaymentAdapter>();
// services.AddTransient<NotificationFacade>();
```

---

## Common pitfalls & tradeoffs
- Over-adapting: writing adapters for every small mismatch can add unnecessary indirection. Prefer adapters for stable or reused boundaries.
- God Facade: facades that bundle unrelated responsibilities become maintenance burdens. Keep facades cohesive.
- Leaky abstractions: facades should not expose subsystem internals; if clients need subsystem features frequently, maybe skip the facade or expose a richer API.

---

## Lab prompt (in-class)
1. Adapter task: Given a legacy logging service with `void Write(string level, string text)` adapt it to `ILogger` interface (`void Info(string msg)`, `void Error(string msg)`) with an adapter that maps levels and formats messages.
2. Facade task: Design a `UserOnboardingFacade` that performs: create user in repository, send welcome email, log metrics, and schedule initial tasks. Sketch UML (classes and relationships) and write the public `Onboard(UserDto user)` method signature and a short pseudo-flow.

Deliverable: UML sketch, adapter/facade class signatures, and a 200–300 word justification for your lifetime choices and test strategy.

---

## Homework
Write a one-page justification: "When to prefer a Facade over direct subsystem access." Include an example scenario, benefits, and one potential drawback.

Further reading
- GoF: Adapter & Facade chapters  
- Articles on anti-corruption layers (AC layer as an advanced adapter concept)  
- Microsoft docs: designing for DI and testability in .NET

Bring your lab sketches and questions to class — we’ll implement the adapter and facade patterns together and run unit tests.