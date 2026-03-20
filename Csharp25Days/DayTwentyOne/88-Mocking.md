# Mocking Concepts and Testability Design

Objectives
- Understand why mocking matters.
- Learn the common kinds of test doubles.
- Apply testability-first design patterns in C#.
- See a short example using xUnit and Moq (Arrange–Act–Assert).

Why mock?
Unit tests should be fast, deterministic, and focused on a small unit of behavior (a single class or method). Real external dependencies—databases, web services, file systems, clocks, or email servers—are slow, flaky, and make tests non-deterministic. Mocking (or using test doubles) replaces those external dependencies with controlled substitutes so tests can focus on logic and behavior.

Types of test doubles (short)
- Dummy: passed but never used (e.g., a required parameter).
- Stub: provides canned responses for the system under test.
- Fake: a working, simplified implementation (e.g., in-memory DB).
- Spy: records how it was called (used for assertions).
- Mock: pre-programmed with expectations and can verify interactions.

Design for testability — practical checklist
- Depend on abstractions (interfaces), not concrete classes.
- Use constructor injection (or a DI container) to supply dependencies.
- Avoid static/global state (DateTime.Now, static singletons) — wrap them.
- Keep methods small and single-responsibility.
- Prefer return values over side-effects where reasonable.
- Provide seams: small points where behavior can be replaced in tests.
- Use in-memory fakes for simple integration in unit tests but rely on real integration tests for cross-cutting behavior.
- Fail fast and avoid hidden behavior in constructors (IO or heavy work in ctor).

Minimal example: production code design
- OrderProcessor depends on IPaymentGateway and IOrderRepository and IEmailSender.
- All dependencies are injected; there is no static access to DateTime or configuration.

```csharp
// Production interfaces
public interface IPaymentGateway
{
    bool Charge(string cardToken, decimal amount);
}

public interface IOrderRepository
{
    void Save(Order order);
}

public interface IEmailSender
{
    void Send(string to, string subject, string body);
}

// Class under test
public class OrderProcessor
{
    private readonly IPaymentGateway _payment;
    private readonly IOrderRepository _repo;
    private readonly IEmailSender _email;

    public OrderProcessor(IPaymentGateway payment, IOrderRepository repo, IEmailSender email)
    {
        _payment = payment ?? throw new ArgumentNullException(nameof(payment));
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public bool Process(Order order, string cardToken)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (string.IsNullOrWhiteSpace(cardToken)) throw new ArgumentException("cardToken required");

        var charged = _payment.Charge(cardToken, order.Total);
        if (!charged)
        {
            _email.Send(order.CustomerEmail, "Payment failed", "Please update payment info.");
            return false;
        }

        _repo.Save(order);
        _email.Send(order.CustomerEmail, "Order received", "Thanks for your order.");
        return true;
    }
}
```

Unit test example using xUnit + Moq
- Goals: stub the payment gateway response and verify repository/email behavior.
- Pattern: Arrange — Act — Assert.

```csharp
using Moq;
using Xunit;

public class OrderProcessorTests
{
    [Fact]
    public void Process_SuccessfulPayment_SavesOrderAndSendsConfirmation()
    {
        // Arrange
        var order = new Order { Id = 1, Total = 100m, CustomerEmail = "c@ex.com" };
        var mockPayment = new Mock<IPaymentGateway>();
        var mockRepo = new Mock<IOrderRepository>();
        var mockEmail = new Mock<IEmailSender>();

        mockPayment.Setup(p => p.Charge(It.IsAny<string>(), order.Total)).Returns(true);

        var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);

        // Act
        var result = sut.Process(order, "token-123");

        // Assert
        Assert.True(result);
        mockRepo.Verify(r => r.Save(order), Times.Once);
        mockEmail.Verify(e => e.Send(order.CustomerEmail, "Order received", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Process_PaymentFails_SendsFailureEmailAndDoesNotSave()
    {
        // Arrange
        var order = new Order { Id = 2, Total = 55m, CustomerEmail = "d@ex.com" };
        var mockPayment = new Mock<IPaymentGateway>();
        var mockRepo = new Mock<IOrderRepository>();
        var mockEmail = new Mock<IEmailSender>();

        mockPayment.Setup(p => p.Charge(It.IsAny<string>(), order.Total)).Returns(false);

        var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);

        // Act
        var result = sut.Process(order, "token-bad");

        // Assert
        Assert.False(result);
        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
        mockEmail.Verify(e => e.Send(order.CustomerEmail, "Payment failed", It.IsAny<string>()), Times.Once);
    }
}
```

Key mocking techniques demonstrated
- Stubbing return values (Setup(...).Returns(...)).
- Verifying interactions (Verify(..., Times.Once)).
- Using It.IsAny<T>() for non-interesting parameters.
- Making tests readable by naming variables `mockPayment`, `mockRepo` and `sut` (system under test).

When to prefer integration tests instead
- Integration tests are required when you need to validate behavior across components or with real infrastructure:
  - Database transactions and migrations.
  - End-to-end API contracts.
  - Message queues and background workers.
  - Security auth flows.
- Integration tests are slower and less isolated; they complement—not replace—unit tests.

Common pitfalls and anti-patterns
- Over-mocking: If you need to mock many internal methods of a class, it may be doing too much. Favor smaller classes and clearer responsibilities.
- Testing implementation details: Prefer testing observable behavior, not internal calls (except when the contract requires them).
- Tightly coupled design: If your business logic directly constructs dependencies (new HttpClient(), new SqlConnection()), it's hard to test; refactor to inject abstractions.

Extras: abstracting time and environment
- Avoid DateTime.Now or Environment.GetEnvironmentVariable directly. Add small abstractions:
```csharp
public interface IClock { DateTime UtcNow { get; } }
public class SystemClock : IClock { public DateTime UtcNow => DateTime.UtcNow; }
```
This allows deterministic tests for time-dependent logic.

Classroom exercise (guided)
- Write 10 unit test descriptions in plain English for a chosen capstone function (e.g., `Process` above). For each, indicate:
  - The dependency to mock (payment, repo, email).
  - Whether a fake would suffice (in-memory repo) or real integration is needed (DB).
  - Expected outcome and one assertion.

Closing notes
- Mocking is a pragmatic tool to keep unit tests fast and deterministic.
- Design for testability early: small abstractions, constructor injection, and seams reduce friction when writing tests.
- Use integration tests to validate cross-component behavior and real infra interactions.

Further reading and libraries
- xUnit: https://xunit.net
- Moq: https://github.com/moq/moq4
- FluentAssertions (for expressive asserts): https://fluentassertions.com
