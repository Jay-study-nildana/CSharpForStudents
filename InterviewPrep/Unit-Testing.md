# Unit Testing, Frameworks, Mocks & Moq — C#/.NET Reference with Examples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Why Unit Test? Benefits & Naming Conventions](#why-unit-test-benefits--naming-conventions)  
3. [Test Structure: Arrange — Act — Assert (AAA)](#test-structure-arrange--act--assert-aaa)  
4. [Testing Frameworks — NUnit, xUnit, MSTest (compare)](#testing-frameworks---nunit-xunit-mstest-compare)  
   - Examples for each framework  
5. [Asserts, Setup/TearDown & Lifecycle Hooks](#asserts-setupteardown--lifecycle-hooks)  
   - NUnit, xUnit and MSTest examples and equivalents  
6. [Parameterized Tests & Data-driven Testing](#parameterized-tests--data-driven-testing)  
   - Examples across frameworks  
7. [Code Coverage — Tools & CI Integration](#code-coverage---tools--ci-integration)  
8. [Mocks, Stubs, Fakes, Moles — Concepts & Differences](#mocks-stubs-fakes-moles---concepts--differences)  
9. [Moq Framework — Practical Recipes & Samples](#moq-framework---practical-recipes--samples)  
   - Creating mocks, Setup/Returns, SetupSequence  
   - Argument-dependent matching (It.Is / It.IsAny / It.IsInRange / It.IsRegex)  
   - SetupGet / SetupSet for properties, value tracking with Callback  
   - Mocking events (Raise) and callbacks  
   - Verifying calls, Verify/VerifyNoOtherCalls, VerifyOrder (via sequence in code)  
   - Behavior customization: Strict vs Loose, CallBase, DefaultValue, Async methods  
   - Throwing exceptions from mocks and testing error paths  
10. [Best Practices & Anti-patterns](#best-practices--anti-patterns)  
11. [Short Q&A — Common Interview Points](#short-qa---common-interview-points)  
12. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document is a practical, example-rich reference for unit testing C# code. It covers why unit testing matters, naming conventions, test structure (AAA), compares common .NET test frameworks (NUnit, xUnit, MSTest), explains lifecycle hooks and parameterized tests, describes code coverage tools, explains mock/test double categories, and provides detailed Moq examples for common mocking scenarios (properties, methods, events, callbacks, verification, async scenarios, and behavior modes).

All code samples are in C# and intended to run with .NET Core / .NET 5+ and the listed test frameworks and Moq (NuGet packages).

---

## 2. Why Unit Test? Benefits & Naming Conventions

Benefits
- Fast feedback on correctness and regressions.
- Enables safe refactoring and design improvements.
- Acts as executable documentation.
- Helps design for testability (loose coupling, DI).
- Encourages smaller, single-responsibility units.

Naming conventions
- Use descriptive test names showing intent and expected behavior.
- Common pattern: MethodName_StateUnderTest_ExpectedBehavior
  - Example: `CalculateDiscount_OrderOver100_Returns10Percent`
- Alternatives: Given_When_Then or Should_DoSomething_WhenCondition

---

## 3. Test Structure: Arrange — Act — Assert (AAA)

Arrange: set up objects, mocks, test data.  
Act: execute the unit under test.  
Assert: verify results and side-effects.

Example:
```csharp
// Arrange
var repoMock = new Mock<IOrderRepository>();
repoMock.Setup(r => r.GetTotal(It.IsAny<int>())).Returns(150m);
var svc = new DiscountService(repoMock.Object);

// Act
var result = svc.CalculateDiscount(123);

// Assert
Assert.Equal(0.10m, result);
```

---

## 4. Testing Frameworks — NUnit, xUnit, MSTest (compare)

Quick comparison
- MSTest
  - Microsoft-built, integrated in Visual Studio.
  - Uses attributes like [TestMethod], [TestClass].
- NUnit
  - Mature, widely used with expressive attributes (TestCase, TestFixture).
  - Rich assertion helpers (Assert.That).
- xUnit
  - Modern, designed for .NET Core; uses constructor-based setup and `IClassFixture` for shared context.
  - Encourages non-static lifecycle hooks, avoids [SetUp]/[TearDown].

Choose:
- For new .NET Core projects, xUnit is common (built-in templates often use xUnit).
- Use MSTest when constrained by Microsoft tooling or legacy projects.
- Use NUnit when you prefer its features (TestCase, more assertion styles).

Install (example):
```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq
```

Example tests (simple "Add" calculator) across frameworks:

NUnit
```csharp
using NUnit.Framework;

[TestFixture]
public class CalculatorTests
{
    [Test]
    public void Add_TwoIntegers_ReturnsSum()
    {
        var calc = new Calculator();
        Assert.AreEqual(3, calc.Add(1,2));
    }
}
```

xUnit
```csharp
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Add_TwoIntegers_ReturnsSum()
    {
        var calc = new Calculator();
        Assert.Equal(3, calc.Add(1,2));
    }
}
```

MSTest
```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void Add_TwoIntegers_ReturnsSum()
    {
        var calc = new Calculator();
        Assert.AreEqual(3, calc.Add(1,2));
    }
}
```

---

## 5. Asserts, Setup/TearDown & Lifecycle Hooks

Assertions
- Each framework offers standard asserts: Equal/True/False/Null/NotNull, and more advanced assertions (collections, exceptions).
- Example: assert an exception thrown.

NUnit
```csharp
Assert.Throws<ArgumentException>(() => service.Do(null));
```

xUnit
```csharp
var ex = Assert.Throws<ArgumentNullException>(() => service.Do(null));
Assert.Equal("paramName", ex.ParamName);
```

MSTest
```csharp
Assert.ThrowsException<ArgumentNullException>(() => service.Do(null));
```

Lifecycle hooks

NUnit
- Per-test: [SetUp], [TearDown]  
- Per-fixture: [OneTimeSetUp], [OneTimeTearDown]

```csharp
[SetUp]
public void Setup() { /* runs before each test */ }

[OneTimeSetUp]
public void OneTime() { /* runs once per fixture */ }
```

xUnit
- No attributes for setup/teardown. Use constructor and IDisposable:
```csharp
public class MyTests : IDisposable
{
    public MyTests() { /* runs before each test */ }
    public void Dispose() { /* runs after each test */ }
}
```
- For per-class (one-time) setup, use IClassFixture<TFixture> or ICollectionFixture<TFixture>.

MSTest
- Per-test: [TestInitialize], [TestCleanup]  
- Per-class: [ClassInitialize], [ClassCleanup] (static methods)

```csharp
[TestInitialize]
public void Init() { }

[ClassInitialize]
public static void ClassInit(TestContext ctx) { }
```

---

## 6. Parameterized Tests & Data-driven Testing

NUnit (TestCase, TestCaseSource)
```csharp
[TestCase(1,2,3)]
[TestCase(-1,5,4)]
public void Add_VariousInputs_ReturnsSum(int a, int b, int expected)
{
    Assert.AreEqual(expected, new Calculator().Add(a,b));
}
```

xUnit (Theory + InlineData / MemberData)
```csharp
[Theory]
[InlineData(1,2,3)]
[InlineData(-1,5,4)]
public void Add_VariousInputs_ReturnsSum(int a, int b, int expected)
{
    Assert.Equal(expected, new Calculator().Add(a,b));
}
```

MSTest (DataTestMethod + DataRow)
```csharp
[DataTestMethod]
[DataRow(1,2,3)]
[DataRow(-1,5,4)]
public void Add_VariousInputs_ReturnsSum(int a, int b, int expected)
{
    Assert.AreEqual(expected, new Calculator().Add(a,b));
}
```

Advanced: use external CSV/JSON or produce dynamic data via MemberData / TestCaseSource.

---

## 7. Code Coverage — Tools & CI Integration

Common tools
- Coverlet (cross-platform, integrates with dotnet test)
  - `dotnet add package coverlet.collector`
  - Use in CI: `dotnet test /p:CollectCoverage=true`
- Visual Studio built-in code coverage (Windows)
- ReportGenerator to transform coverage files (Cobertura, HTML)

Example (collect coverage and produce report):
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport"
```

CI (GitHub Actions) sample:
- Use actions/setup-dotnet, dotnet test with Coverlet, then upload report as artifact or use codecov.io.

---

## 8. Mocks, Stubs, Fakes, Moles — Concepts & Differences

Test doubles taxonomy
- Dummy: Passed but not used (e.g., null object).
- Fake: Working implementation but simpler (in-memory DB).
- Stub: Provides canned responses for test (no behavior verification).
- Mock: Pre-programmed expectations and verifications; used to assert interactions.
- Moles / Shims (Microsoft Fakes): Replace .NET method calls including static/system calls (useful for legacy code) — available in Visual Studio Enterprise (not cross-platform).
- Static fakes: sometimes require shims or wrapper abstractions to avoid hard-to-test static methods.

Guidelines
- Use mocks to verify interactions (e.g., that Save was called).
- Use stubs/fakes for supplying controlled data.
- Prefer design that allows easy DI and testing without heavy shims.

---

## 9. Moq Framework — Practical Recipes & Samples

Install Moq via NuGet:
```bash
dotnet add package Moq
```

Basic mock and Setup/Verify
```csharp
using Moq;
using Xunit;

public interface IRepo { int GetCount(); void Save(Order o); }

public class OrderServiceTests
{
    [Fact]
    public void PlaceOrder_CallsSave()
    {
        // Arrange
        var repoMock = new Mock<IRepo>();
        var svc = new OrderService(repoMock.Object);

        // Act
        svc.PlaceOrder(new Order{ Id = 1 });

        // Assert: verify Save was called once with any Order
        repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);
    }
}
```

Setup Returns & SetupSequence
```csharp
repoMock.Setup(r => r.GetCount()).Returns(42);

// Sequence (multiple calls):
repoMock.SetupSequence(r => r.GetCount())
        .Returns(1)
        .Returns(2)
        .Throws(new InvalidOperationException());
```

Argument-dependent matching
```csharp
repoMock.Setup(r => r.Save(It.Is<Order>(o => o.Total > 100)))
        .Verifiable();

repoMock.Setup(r => r.Find(It.Is<int>(id => id > 0)))
        .Returns<int>(id => new Order { Id = id });
```

Common It helpers
- `It.IsAny<T>()`
- `It.Is<T>(predicate)`
- `It.IsInRange<T>(low, high, Range.Inclusive)`
- `It.IsRegex("pattern")` for strings
- `It.IsIn<T>(collection)`

Setup with callbacks (capture arguments / value tracking)
```csharp
Order captured = null;
repoMock.Setup(r => r.Save(It.IsAny<Order>()))
        .Callback<Order>(o => captured = o);

svc.PlaceOrder(new Order{ Id = 99, Total = 150 });

Assert.NotNull(captured);
Assert.Equal(150, captured.Total);
```

SetupGet / SetupSet for properties
```csharp
// backing field not necessary with SetupGet - useful to mock properties with behavior
var mock = new Mock<ISettings>();
mock.SetupGet(s => s.Timeout).Returns(TimeSpan.FromSeconds(30));

// verify property setter
mock.SetupSet(s => s.Name = "Alice").Verifiable();
mock.Object.Name = "Alice";
mock.Verify();
```

Mocking events (Raise)
```csharp
public interface INotifier { event EventHandler<EventArgs> Alert; }

var notifier = new Mock<INotifier>();
bool eventHandled = false;
notifier.Object.Alert += (s,e) => eventHandled = true;

// Raise event from mock
notifier.Raise(n => n.Alert += null, EventArgs.Empty);
Assert.True(eventHandled);
```

Callbacks and Returns with arguments
```csharp
repoMock.Setup(r => r.Add(It.IsAny<string>()))
    .Callback<string>(s => Console.WriteLine($"Added: {s}"))
    .Returns(true);
```

Verification examples
```csharp
repoMock.Verify(r => r.Save(It.Is<Order>(o => o.Id == 1)), Times.Once);
repoMock.VerifyNoOtherCalls(); // ensures no unexpected interactions
```

Behavior customization
- Strict vs Loose:
  - `new Mock<IService>(MockBehavior.Strict)` — any call not set up will throw.
  - `MockBehavior.Loose` (default) — returns default(T) for unconfigured calls.
- CallBase:
  - `mock.CallBase = true` — calls base class implementation if not setup (useful for partial mocks of concrete classes).
- DefaultValue:
  - `mock.DefaultValue = DefaultValue.Mock` — returning mocks for nested properties(not common).

Async methods and ReturnsAsync
```csharp
public interface IApi { Task<string> GetAsync(string url); }

var apiMock = new Mock<IApi>();
apiMock.Setup(a => a.GetAsync(It.IsAny<string>())).ReturnsAsync("hello");

var result = await apiMock.Object.GetAsync("x");
Assert.Equal("hello", result);
```

Throwing exceptions from mock
```csharp
repoMock.Setup(r => r.Save(It.IsAny<Order>())).Throws(new InvalidOperationException("fail"));

var ex = Assert.Throws<InvalidOperationException>(() => svc.PlaceOrder(new Order()));
Assert.Equal("fail", ex.Message);
```

Out/ref parameter handling (example)
```csharp
public interface IParser { bool TryParse(string s, out int value); }

var mock = new Mock<IParser>();
int outVal = 123;
mock.Setup(m => m.TryParse("42", out outVal)).Returns(true);

int result;
var ok = mock.Object.TryParse("42", out result); // ok==true, result==123
```

Using `It.Ref<T>.IsAny` for ref/out flexible matching:
```csharp
int any;
mock.Setup(m => m.TryParse(It.IsAny<string>(), out any)).Returns(true);
```

Complex verification order (manual)
- Moq doesn't provide a built-in VerifyOrder API. To verify call order you can use `MockSequence`:
```csharp
var seq = new MockSequence();
var mock = new Mock<IRepo>();
mock.InSequence(seq).Setup(m => m.Connect());
mock.InSequence(seq).Setup(m => m.Save(It.IsAny<Order>()));
mock.Object.Connect();
mock.Object.Save(new Order());
mock.Verify(m => m.Connect());
mock.Verify(m => m.Save(It.IsAny<Order>()));
```

Testing events with arguments
```csharp
public class PriceChangedEventArgs : EventArgs { public decimal NewPrice { get; set; } }

var productMock = new Mock<IProduct>();
bool gotPriceChange = false;
productMock.Object.PriceChanged += (s, e) => gotPriceChange = e.NewPrice == 9.99m;

// Raise with typed args
productMock.Raise(p => p.PriceChanged += null, new PriceChangedEventArgs{ NewPrice = 9.99m });
Assert.True(gotPriceChange);
```

Notes on Moq and performance/behavior
- Avoid over-mocking: prefer testing behavior and state rather than implementation details.
- Use `Verify` to assert interactions when those interactions are the contract (e.g., you must call repository.Save).
- Prefer `Returns` with lambdas (`.Returns<int>(id => new Order{ Id = id})`) to produce dynamic results.

---

## 10. Best Practices & Anti-patterns

Best practices
- Test one thing per unit test.
- Use DI and interfaces to enable mocking.
- Prefer testing public behaviour and outputs rather than private implementation.
- Use inline data/theory tests for multiple cases.
- Keep tests deterministic and independent (no shared mutable global state).
- Use test-driven development (TDD) where helpful.
- Validate test coverage but do not chase 100% blindly — aim for meaningful tests.

Anti-patterns
- Overuse of Service Locator in tests — hides dependencies.
- Over-specifying mocks (verifying internal calls) which makes tests brittle to refactor.
- Testing implementation detail (private methods) — prefer public API.
- Slow tests hitting real database / network — prefer in-memory fakes or integration tests separated from unit tests.

---

## 11. Short Q&A — Common Interview Points

Q: When should you mock vs use a fake?  
A: Mock when you need to assert interactions (e.g., Save called). Use a fake (in-memory DB) when you want realistic behavior without complexity.

Q: What's the AAA pattern?  
A: Arrange, Act, Assert — common structure of a unit test.

Q: How do you test async methods?  
A: Use `async Task` test methods and await the target method. Use framework support (`ReturnsAsync`, `ThrowsAsync`) in mocks.

Q: What is Test Fixture (NUnit) vs Fixture (xUnit IClassFixture)?  
A: Fixture provides shared context. NUnit uses attributes ([TestFixture], [OneTimeSetUp]). xUnit uses fixture classes injected via constructor (IClassFixture).

Q: How to measure code coverage in .NET Core?  
A: Use Coverlet (dotnet test /p:CollectCoverage=true) and ReportGenerator to create readable reports.

---

## 12. References & Further Reading

- xUnit docs: https://xunit.net/  
- NUnit docs: https://nunit.org/  
- MSTest docs: https://learn.microsoft.com/visualstudio/test/mstest-overview  
- Moq GitHub: https://github.com/moq/moq4  
- Coverlet: https://github.com/coverlet-coverage/coverlet  
- ReportGenerator: https://danielpalme.github.io/ReportGenerator/  
- "The Art of Unit Testing" by Roy Osherove (book)  
- "Mocks Aren't Stubs" — Martin Fowler article

---

Prepared as a hands-on reference to get started writing unit tests, integrating them with CI, and using Moq to create expressive, maintainable test doubles. If you want, I can:  
- generate example test projects (xUnit + Moq) with sample classes and tests,  
- produce CI workflow (GitHub Actions) with test & coverage steps, or  
- convert any of the examples into runnable code files and open a PR in one of your repos. Which would you like next?