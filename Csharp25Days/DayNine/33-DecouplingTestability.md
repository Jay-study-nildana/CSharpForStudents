# Decoupling Designs for Testability (C# / .NET)

Good design makes code easy to test. Decoupling — separating concerns and depending on abstractions, not concrete implementations — is the central idea that enables fast, reliable unit tests. This guide shows practical patterns and short C# examples you can apply immediately.

---

## Principles at a glance
- Depend on abstractions (interfaces or abstract classes), not concrete types.
- Inject dependencies (constructor injection preferred) instead of creating them inside methods.
- Keep side-effects (I/O, DB, network) at the edges; keep core logic pure and side-effect free.
- Provide seams (places to substitute collaborators) so tests can use lightweight fakes/stubs.

---

## Constructor injection (favored)
Give collaborators to a class via its constructor so tests can pass substitutes.

Example:
```csharp
public interface IEmailSender { void Send(string to, string body); }

public class NotificationService
{
    private readonly IEmailSender _email;
    public NotificationService(IEmailSender email) => _email = email;

    public void Welcome(string userEmail) => _email.Send(userEmail, "Welcome!");
}
```

In tests, pass a fake implementation:
```csharp
class FakeEmailSender : IEmailSender
{
    public string LastTo; public string LastBody;
    public void Send(string to, string body) { LastTo = to; LastBody = body; }
}

// Test
var fake = new FakeEmailSender();
var svc = new NotificationService(fake);
svc.Welcome("test@x.com");
Assert.Equal("test@x.com", fake.LastTo);
```

Benefits: tests are deterministic, fast, and don't require network or disk.

---

## Seams and test doubles
Common test doubles:
- Fake: a simple working implementation (e.g., in-memory DB).
- Stub: returns canned responses.
- Spy: records calls for assertions (our FakeEmailSender above acts as a spy).
- Mock: verifies interactions (often provided by frameworks like Moq, NSubstitute).

Use an in-memory fake for integration-like tests:
- In-memory caches, queues, or DB (SQLite or in-memory EF Core) let you test behavior without external infrastructure.

---

## Avoid static dependencies and singletons
Static calls (e.g., Logger.Log(...)) and global singletons are hard to replace in tests. Prefer injectable abstractions:
- If a component needs logging, inject `ILogger`.
- If a static factory must exist, wrap it behind an interface and inject the wrapper.

Service Locator is an anti-pattern for testability: hidden dependencies inside `ServiceLocator.Get<T>()` make objects hard to instantiate and mock in tests. Prefer explicit constructor injection.

---

## Interfaces, abstractions, and small contracts
Design small, focused interfaces (Interface Segregation). Large interfaces force tests to implement irrelevant members.

Good:
```csharp
public interface ICustomerRepository { Customer Get(int id); void Save(Customer c); }
```
Avoid bloated interfaces like `IRepository` with 20 unrelated methods unless genuinely needed.

---

## Composition over inheritance
Favor composing behavior via injected collaborators (strategy/decorator) to vary functionality in tests rather than subclassing large classes. Composition yields clearer seams for substitution.

Example: logging decorator
```csharp
public class LoggingPayment : IPayment
{
    private readonly IPayment _inner; private readonly ILogger _log;
    public LoggingPayment(IPayment inner, ILogger log) { _inner = inner; _log = log; }
    public void Pay(decimal amount) { _log.Log("pay"); _inner.Pay(amount); }
}
```
Tests can inject a fake `IPayment` or `ILogger` to observe behavior.

---

## Design for unit vs integration tests
- Unit tests: small, fast, isolated — use fakes/mocks; no I/O.
- Integration tests: exercise more real components (e.g., EF Core in-memory or Dockerized DB) to catch infra issues.
Design your application so you can run many fast unit tests and a smaller set of integration tests.

---

## Practical tips & checklist
- Constructor-inject dependencies; avoid `new` for collaborators inside methods.
- Return interfaces (e.g., `IEnumerable<T>`, `IReadOnlyList<T>`) instead of concrete collections to hide implementation details.
- Keep methods small and single-purpose — easier to test.
- Use dependency injection containers only at composition roots; keep object graphs explicit for tests (avoid Service Locator).
- For async/Task methods, prefer returning `Task` and use `Task.CompletedTask` or synchronous test doubles where appropriate.
- Write a few tests that use in-memory implementations for end-to-end logic (not full production infra).

---

## Example: repository + service with tests
```csharp
public interface IOrderRepository { Order Get(int id); void Save(Order o); }

public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
    public void Cancel(int id)
    {
        var o = _repo.Get(id) ?? throw new InvalidOperationException();
        o.Cancel();
        _repo.Save(o);
    }
}
```
Test with a fake repo that stores orders in a dictionary — no DB required.

---

## Final note
Designing for testability is design for decoupling: small interfaces, explicit dependencies, composition, and seams make code more maintainable and verifiable. Start at the class boundaries: if you find yourself reaching for `new` to create collaborators or sprinkling static calls, that's a sign to introduce an abstraction and inject it — your tests (and future you) will thank you.