# Arrange — Act — Assert: The Simple Pattern That Makes Tests Clear

The Arrange‑Act‑Assert (AAA) pattern is the foundational structure for readable, maintainable unit tests. It divides each test into three clear steps:
- Arrange: prepare inputs, test doubles, and the system under test (SUT).
- Act: execute the behavior you want to verify.
- Assert: check the observable result or interaction.

This pattern improves clarity (readers quickly see intent), reduces accidental complexity in tests, and helps you follow the single-responsibility rule for tests (one behavior per test).

Why AAA matters
- Readability: any developer can scan the three sections to see what the test sets up, what it does, and what it expects.
- Maintainability: focused tests are easier to update when the SUT changes.
- Debuggability: failures point to either arrangement problems or assertion mismatches.

Basic example (xUnit)

```csharp
// Production code
public class Calculator
{
    public int Add(int a, int b) => a + b;
}

// Unit test using AAA
public class CalculatorTests
{
    [Fact]
    public void Add_ReturnsSum_WhenGivenTwoIntegers()
    {
        // Arrange
        var sut = new Calculator();
        int a = 2, b = 3;

        // Act
        int result = sut.Add(a, b);

        // Assert
        Assert.Equal(5, result);
    }
}
```

Guidelines for each section

- Arrange
  - Keep setup minimal and directly relevant to the behavior under test.
  - Use explicit values (no magic numbers) so intent is clear.
  - Prefer object factories or builders only when setup becomes repetitive or complex.
  - Create test doubles (mocks, stubs, fakes) here for dependencies.

- Act
  - Perform exactly one operation that represents the behavior under test.
  - For async methods, `await` the call. Avoid mixing synchronous blocking (`.Result`/`.Wait()`).
  - Keep Act short—usually a single line.

- Assert
  - Verify what the external observer can see: return value, state change, or interactions.
  - Prefer one assertion per behavioral concept. Multiple assertions are OK when they verify a single logical outcome (e.g., object equality).
  - For interaction verification, assert that the mock was called with expected arguments.

Testing interactions (mock example with Moq)

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
        if (amount <= 0) throw new ArgumentException(nameof(amount));
        return await _gateway.ChargeAsync(amount);
    }
}

public class OrderProcessorTests
{
    [Fact]
    public async Task ProcessAsync_ChargesGatewayAndReturnsTrue_WhenGatewaySucceeds()
    {
        // Arrange
        var mockGateway = new Mock<IPaymentGateway>();
        mockGateway.Setup(g => g.ChargeAsync(100m)).ReturnsAsync(true);
        var sut = new OrderProcessor(mockGateway.Object);

        // Act
        bool result = await sut.ProcessAsync(100m);

        // Assert
        Assert.True(result);
        mockGateway.Verify(g => g.ChargeAsync(100m), Times.Once);
    }
}
```

Common pitfalls and how AAA helps avoid them
- Over-setup: Tests with large Arrange blocks are brittle. Keep setup focused on the behavior.
- Multiple responsibilities: If Act performs several operations, split into separate tests.
- Over-mocking: Verifying internal calls that are implementation details makes tests fragile. Assert observable behavior first; use interaction tests when the interaction is part of the contract.
- Hidden assertions: Assertions inside helper methods make tests harder to read. Keep asserts in the Assert section.

Working with async tests
- Always use async all the way: tests should be async and use `await`.
- Example:

```csharp
[Fact]
public async Task FetchAsync_ReturnsData()
{
    // Arrange
    var client = new FakeHttpClient("ok");
    var sut = new DataFetcher(client);

    // Act
    var data = await sut.FetchAsync("http://example");

    // Assert
    Assert.Equal("ok", data);
}
```

Test organization and AAA
- Use AAA within each test method; use test class setup/teardown sparingly for common wiring.
- If many tests share the same Arrange, consider creating helper methods or small builders—avoid hiding important setup from the test reader.

TDD and AAA
- TDD pairs naturally with AAA:
  - RED: Write the failing test using Arrange‑Act‑Assert (explicitly).
  - GREEN: Implement the minimal production code to pass the Assert.
  - REFACTOR: Improve code while keeping the test’s AAA structure intact.

Examples of transition:
1. RED — write test asserting expected behavior (Arrange minimal state, Act the call, Assert expected error/value).
2. GREEN — implement just enough code to return the expected result.
3. REFACTOR — clean code and tests for readability, keeping AAA clear.

Exercise for the capstone (classroom)
- Pick one business method (e.g., CalculateDiscount, PlaceOrder).
- Write 3 tests in plain English using AAA:
  1. Arrange: a customer with VIP status; Act: calculate discount; Assert: discount is 20%.
  2. Arrange: a normal customer and a negative price; Act: call method; Assert: throws ArgumentException.
  3. Arrange: mock inventory shows item in stock; Act: place order; Assert: order succeeds and ReserveItem called once.

Quick checklist (when writing tests)
- Does the test have three sections (Arrange, Act, Assert) clearly separated?
- Is Arrange minimal and only what you need?
- Does Act do one thing and is it awaited if async?
- Does Assert verify observable behavior or public contract?
- Is the test deterministic and fast?

Naming convention: MethodName_StateUnderTest_ExpectedBehavior
- Example: CalculateDiscount_VipCustomer_Returns20Percent

Wrap-up
- AAA is a simple discipline that makes tests readable, focused, and reliable.
- Use test doubles in Arrange to isolate the unit.
- Keep Act short and Assert precise.
- Apply AAA consistently and you’ll find tests easier to write, review, and maintain.

Further reading
- xUnit documentation, Moq quickstart, and Kent Beck’s TDD examples.