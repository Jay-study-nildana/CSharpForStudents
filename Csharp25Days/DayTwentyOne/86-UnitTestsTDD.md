# Unit Tests & Test-Driven Development (TDD) — C#/.NET (Day 21)

This one-page guide explains why unit tests matter, the TDD cycle, how to write clear unit tests (Arrange-Act-Assert), and how to use basic test doubles (mocks/fakes). It includes short C# snippets (xUnit + Moq style) showing a TDD workflow and testable design patterns.

Why unit tests matter
- Fast feedback: unit tests run quickly and validate small units of behavior.
- Design aid: tests encourage small, single-responsibility units and explicit dependencies.
- Regression safety: they catch unintended changes when refactoring.
- Documentation: tests show how code is intended to be used.

Key principles
- Test one thing per test. Keep tests deterministic and isolated.
- Use the Arrange-Act-Assert (AAA) pattern for readability.
- Keep production code decoupled from external resources (I/O, DB, network) so units can be tested in memory.
- Prefer interfaces and dependency injection to enable test doubles.

Arrange-Act-Assert (AAA) — pattern and example
- Arrange: set up inputs, dependencies, and the system under test (SUT).
- Act: execute the behavior under test.
- Assert: verify the expected outcome.

Example (xUnit):

```csharp
// Production code
public class Calculator
{
    public int Add(int a, int b) => a + b;
}

// Unit test
public class CalculatorTests
{
    [Fact]
    public void Add_ReturnsSum_WhenGivenTwoIntegers()
    {
        // Arrange
        var calc = new Calculator();

        // Act
        var result = calc.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }
}
```

Test-Driven Development (TDD) — the red/green/refactor loop
1. RED: write a small failing test that expresses a single behavior.
2. GREEN: implement the minimal production code to make the test pass.
3. REFACTOR: clean up code and tests while keeping tests green.

TDD example: adding behavior for a simple service

Step 1 — Write the failing test (RED)
```csharp
[Fact]
public void IsAdult_ReturnsTrue_IfAgeIsAtLeast18()
{
    var svc = new AgeService();
    Assert.True(svc.IsAdult(18));
}
```

Run tests — fail because AgeService.IsAdult doesn't exist. Implement minimal code:

Step 2 — Make it pass (GREEN)
```csharp
public class AgeService
{
    public bool IsAdult(int age) => age >= 18;
}
```

Step 3 — Refactor (if needed) — keep tests passing.

Design for testability: use abstractions
When code depends on external systems, inject abstractions (interfaces) so tests can replace real resources with fakes or mocks.

Example: processing payments — use an interface for the payment gateway.

```csharp
public interface IPaymentGateway
{
    Task<bool> ChargeAsync(decimal amount);
}

public class OrderProcessor
{
    private readonly IPaymentGateway _gateway;
    public OrderProcessor(IPaymentGateway gateway) => _gateway = gateway;

    public async Task<bool> ProcessAsync(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("amount");
        return await _gateway.ChargeAsync(amount);
    }
}
```

Using a mock in a unit test (Moq + xUnit)
```csharp
[Fact]
public async Task ProcessAsync_ReturnsTrue_WhenGatewaySucceeds()
{
    // Arrange
    var mock = new Mock<IPaymentGateway>();
    mock.Setup(g => g.ChargeAsync(100m)).ReturnsAsync(true);
    var processor = new OrderProcessor(mock.Object);

    // Act
    var result = await processor.ProcessAsync(100m);

    // Assert
    Assert.True(result);
    mock.Verify(g => g.ChargeAsync(100m), Times.Once);
}
```

Test doubles — quick guide
- Dummy: placeholder, not used by test (e.g., null object).
- Fake: simple working implementation (in-memory DB).
- Stub: returns predefined responses.
- Mock: verifies interactions (calls, arguments).
Pick the simplest double that keeps the test clear and focused.

When to write integration tests
Unit tests exercise logic in isolation. Integration tests verify pieces work together (DB, file system, real HTTP). Mark integration tests separately and run them less frequently (CI pipeline).

Practical tips and tooling
- Use xUnit or NUnit for test framework; Moq, NSubstitute, or FakeItEasy for mocking.
- Run tests with `dotnet test` and automate them in CI.
- Keep test names descriptive: MethodName_StateUnderTest_ExpectedBehavior.
- Aim for quick test runs (seconds) and stable, deterministic tests.
- Avoid testing private implementation details; test observable behavior.

Common TDD pitfalls
- Over-mocking: tests become brittle when they mirror implementation details.
- Large, slow tests mistaken for unit tests: keep true units fast and isolated.
- Writing tests after the code without using them to guide design — TDD shines because tests drive small, incremental design decisions.

Classroom exercise (short)
1. Pick a small function in your capstone (e.g., calculate discount).
2. Write 3 unit tests first (boundary conditions and an invalid input).
3. Implement minimal code to pass tests and refactor.
4. Repeat for a method that calls an external dependency — use a mock in tests.

Quick checklist for TDD in your capstone
- Start each new behavior with a test (RED).
- Write the simplest production code to satisfy the test (GREEN).
- Refactor for clarity and remove duplication (REFACTOR).
- Use interfaces for external dependencies so tests remain unit-level.
- Keep tests focused: one assertion concept per test.

Commands
- Create a test project: `dotnet new xunit -o MyProject.Tests`
- Add reference to project under test: `dotnet add MyProject.Tests reference ../MyProject`
- Run tests: `dotnet test`

Further reading
- “Test-Driven Development: By Example” — Kent Beck
- Microsoft docs: Unit testing C# in .NET (xUnit, MSTest, NUnit)
