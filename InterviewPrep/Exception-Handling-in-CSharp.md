# Exception Handling in C# — Interview Reference Guide for Developers

---

## Table of Contents

1. [Overview: What is Exception Handling?](#overview-what-is-exception-handling)
2. [Exception Types in .NET / C#](#exception-types-in-net--c)
3. [Basic Try / Catch / Finally Syntax](#basic-try--catch--finally-syntax)
4. [Catching Specific Exceptions and Best Order](#catching-specific-exceptions-and-best-order)
5. [Using finally and deterministic cleanup (IDisposable / using)](#using-finally-and-deterministic-cleanup-idisposable--using)
6. [Throwing Exceptions: throw vs throw ex](#throwing-exceptions-throw-vs-throw-ex)
7. [Creating Custom Exceptions](#creating-custom-exceptions)
8. [Inner Exceptions & Exception Wrapping](#inner-exceptions--exception-wrapping)
9. [AggregateException, Task exceptions and async/await](#aggregateexception-task-exceptions-and-asyncawait)
10. [Exception Filters (when keyword)](#exception-filters-when-keyword)
11. [Exception Safety and Design Principles](#exception-safety-and-design-principles)
12. [Best Practices for APIs and Libraries](#best-practices-for-apis-and-libraries)
13. [Performance Considerations and Cost of Exceptions](#performance-considerations-and-cost-of-exceptions)
14. [Preserving Stack Trace and ExceptionDispatchInfo](#preserving-stack-trace-and-exceptiondispatchinfo)
15. [Logging, Telemetry and Correlation](#logging-telemetry-and-correlation)
16. [Handling Exceptions in ASP.NET Core & Desktop Apps](#handling-exceptions-in-aspnet-core--desktop-apps)
17. [Global and Unhandled Exceptions](#global-and-unhandled-exceptions)
18. [Security Considerations and Information Disclosure](#security-considerations-and-information-disclosure)
19. [Testing Exceptions (unit tests, integration tests)](#testing-exceptions-unit-tests-integration-tests)
20. [Debugging Tips and Tools](#debugging-tips-and-tools)
21. [Static Analysis, Roslyn Analyzers & Code Contracts](#static-analysis-roslyn-analyzers--code-contracts)
22. [Common Mistakes and Anti-Patterns](#common-mistakes-and-anti-patterns)
23. [Comprehensive Q&A — Developer & Interview Questions (with answers)](#comprehensive-qa--developer--interview-questions-with-answers)
24. [Practical Exercises & Projects](#practical-exercises--projects)
25. [References & Further Reading](#references--further-reading)

---

## 1. Overview: What is Exception Handling?

Exception handling is the mechanism for responding to runtime errors, unexpected conditions, or other abnormal situations in a controlled manner. In C# and the .NET runtime, exceptions are objects deriving from System.Exception. Proper handling preserves program correctness, enables recovery or graceful termination, and provides diagnostic information.

Key goals:
- Communicate error conditions across layers.
- Allow graceful resource cleanup.
- Enable centralized logging and telemetry.
- Maintain program invariants and safety.

---

## 2. Exception Types in .NET / C#

- System.Exception: base type for all CLR exceptions.
- System.SystemException: parent of runtime exceptions (e.g., NullReferenceException).
- System.ApplicationException: legacy; do not rely on it.
- Common exceptions:
  - ArgumentException, ArgumentNullException, ArgumentOutOfRangeException
  - InvalidOperationException
  - NullReferenceException, IndexOutOfRangeException
  - IOException, FileNotFoundException, DirectoryNotFoundException
  - UnauthorizedAccessException, SecurityException
  - TimeoutException
  - NotSupportedException, NotImplementedException
  - OperationCanceledException (cancellation)
  - AggregateException (task parallel library)
- Exceptions can be system-level (runtime) or application-defined (custom exceptions).

Important: Exceptions represent exceptional conditions — not normal control flow.

---

## 3. Basic Try / Catch / Finally Syntax

Minimal structure:
```csharp
try
{
    // Code that may throw exceptions
}
catch (SpecificException ex)
{
    // Handle or translate exception
}
catch (Exception ex)
{
    // Fallback: log and potentially rethrow
    throw; // or handle
}
finally
{
    // Executed whether or not exception occurred
}
```

Examples:
```csharp
try
{
    var text = File.ReadAllText(path);
}
catch (FileNotFoundException ex)
{
    // Handle missing file
    Console.WriteLine($"File missing: {ex.FileName}");
}
catch (IOException ex)
{
    // Other I/O issues
}
finally
{
    // Cleanup if necessary
}
```

Notes:
- finally is always executed except in extreme cases (e.g., Environment.FailFast, process termination).
- Avoid empty catch blocks — they swallow errors.

---

## 4. Catching Specific Exceptions and Best Order

- Catch the most specific exceptions first, then more general ones.
- Don’t catch System.Exception unless you have a good reason (e.g., top-level boundary).
- Example order:
  - ArgumentNullException
  - InvalidOperationException
  - IOException
  - Exception

Incorrect:
```csharp
catch (Exception ex)
{
    // broad catch blocks prevent specific handling
}
catch (IOException ex) // unreachable
{
}
```

Use guards and exception filters to avoid catching unwanted cases.

---

## 5. Using finally and deterministic cleanup (IDisposable / using)

Use `using` (syntactic sugar for try/finally + Dispose) to ensure resources are freed:
```csharp
using (var stream = new FileStream(path, FileMode.Open))
{
    // use stream
} // stream.Dispose() is called even if an exception occurs
```

Equivalent:
```csharp
var stream = new FileStream(...);
try
{
    // use stream
}
finally
{
    stream?.Dispose();
}
```

Prefer `using` and `IAsyncDisposable` with `await using` in async contexts (.NET Core / C# 8+).

---

## 6. Throwing Exceptions: throw vs throw ex

- Use `throw;` to rethrow while preserving original stack trace.
- `throw ex;` resets the stack trace — avoid unless intentionally creating a new stack context.

Example:
```csharp
catch (Exception ex)
{
    // Add context & rethrow preserving stack trace
    Log(ex);
    throw;
}
```

If wrapping into another exception:
```csharp
catch (Exception ex)
{
    throw new MyDomainException("Something failed", ex);
}
```
Wrap to add context and set the inner exception.

---

## 7. Creating Custom Exceptions

Guidelines:
- Derive from Exception (or a more specific subclass).
- Mark as [Serializable] if you support remoting/serialization.
- Implement standard constructors: parameterless, message, message+innerException, protected serialization constructor.

Example:
```csharp
[Serializable]
public class PaymentFailedException : Exception
{
    public PaymentFailedException() {}
    public PaymentFailedException(string message) : base(message) {}
    public PaymentFailedException(string message, Exception inner) : base(message, inner) {}
    protected PaymentFailedException(SerializationInfo info, StreamingContext context)
        : base(info, context) {}
}
```

Naming: end with "Exception". Only create custom exceptions when you need to represent a distinct error condition.

---

## 8. Inner Exceptions & Exception Wrapping

- Use InnerException to preserve the original exception when wrapping:
```csharp
catch (SqlException sqlEx)
{
    throw new DataAccessException("Failed to load user", sqlEx);
}
```
- Preserves diagnostic info and stack traces.
- Avoid nesting too deeply; surface meaningful information.

---

## 9. AggregateException, Task exceptions and async/await

- AggregateException: used by TPL to aggregate multiple exceptions (e.g., Parallel.ForEach, Task.WhenAll).
- Unwrapping:
  - Use `.Flatten()` to flatten nested aggregates.
  - Use `.Handle(Func<Exception, bool>)` to handle some and rethrow others.

Async/await:
- When awaiting a Task that faulted, await rethrows the original exception, not AggregateException — helpful.
- When using Task.Wait or Task.Result, exceptions are wrapped in AggregateException.
Example:
```csharp
// Correct: use async/await
try
{
    await TaskThatThrowsAsync();
}
catch (SpecificException ex)
{
    // handle
}

// If you use .Result or .Wait, you'll get AggregateException
```

Parallel / Task.WhenAll:
```csharp
try
{
    await Task.WhenAll(tasks);
}
catch
{
    // tasks exceptions are rethrown as single exception with inner exceptions
    var agg = ex as AggregateException;
}
```

---

## 10. Exception Filters (when keyword)

C# supports exception filters to conditionally catch exceptions without affecting the stack trace.

Example:
```csharp
catch (IOException ex) when (ex.Message.Contains("disk full"))
{
    // handle disk full specifically
}
catch (IOException ex)
{
    // other IO errors
}
```
Benefits:
- Filters execute outside catch blocks and do not change exception stack traces.
- Useful for logging and conditional handling.

---

## 11. Exception Safety and Design Principles

- Do not use exceptions for regular control flow.
- Fail-fast vs graceful recovery: for corrupted state prefer failing fast to avoid inconsistent state.
- Ensure invariants: after an exception, objects should remain in a valid state.
- Prefer pure functions where possible, reduce side effects.
- Implement transactional behavior where appropriate (database transactions, rollback).

Exception safety levels:
- No-throw guarantee: operation never throws (rare).
- Strong exception safety: on failure, program state unchanged (e.g., copy-and-swap).
- Basic exception safety: invariants preserved but state may have changed.

---

## 12. Best Practices for APIs and Libraries

- Define clear exception contracts in documentation.
- Throw specific exceptions (ArgumentNullException, ArgumentOutOfRangeException) for invalid arguments.
- Avoid exposing internal exception types across public boundaries.
- Prefer returning result types (Try pattern) for expected failures:
  - bool TryParse(..., out T result)
  - TryGet pattern avoids exceptions for common failures.
- Use cancellation tokens for cooperative cancellation; use OperationCanceledException.
- Avoid catching exceptions you cannot handle; bubble up or translate to domain-specific exceptions.

---

## 13. Performance Considerations and Cost of Exceptions

- Throwing exceptions is expensive — allocation + stack unwinding.
- Exceptions should be exceptional, not part of hot paths.
- Measure and optimize: use profiling tools to identify hot exception paths.
- Use Try-patterns (TryParse, TryGet) for expected failures.
- Exception objects typically allocate memory; minimize creation in tight loops.

---

## 14. Preserving Stack Trace and ExceptionDispatchInfo

- Use `throw;` to preserve stack trace when rethrowing.
- Use `ExceptionDispatchInfo.Capture(ex).Throw();` to rethrow an exception while preserving original stack trace across async boundaries.

Example:
```csharp
catch (Exception ex)
{
    // store for later rethrow preserving stack trace
    _saved = ExceptionDispatchInfo.Capture(ex);
}

// later
_saved.Throw();
```

Useful when delaying exception propagation (e.g., in async workflows or when marshaling exceptions between threads).

---

## 15. Logging, Telemetry and Correlation

- Log exceptions with:
  - Message, stack trace, exception type, inner exceptions.
  - Correlation ID (HTTP header / request id) for distributed tracing.
  - Contextual data (user id, request path, operation name).
- Use structured logging (Serilog, Microsoft.Extensions.Logging) to enable querying.
- Avoid logging sensitive data (PII, secrets).
- Add exception telemetry to APM tools (Application Insights, Datadog).

Example using Microsoft.Extensions.Logging:
```csharp
try
{
    // code
}
catch (Exception ex)
{
    _logger.LogError(ex, "Failed to process order {OrderId}", orderId);
    throw;
}
```

---

## 16. Handling Exceptions in ASP.NET Core & Desktop Apps

ASP.NET Core:
- Use middleware for centralized error handling:
```csharp
app.UseExceptionHandler("/Home/Error"); // or custom middleware
app.Use(async (context, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        // log and return friendly error
    }
});
```
- For APIs: return standardized error responses (ProblemDetails, HTTP status codes).
- Map exceptions to HTTP status codes (e.g., NotFoundException -> 404).

ASP.NET Core example: Exception Translation Middleware:
```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public async Task Invoke(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex) { await Handle(ex, context); }
    }
}
```

Desktop (WPF/WinForms):
- Handle Application.DispatcherUnhandledException or AppDomain.CurrentDomain.UnhandledException to log and prompt the user, but avoid attempting to continue in corrupted state.

---

## 17. Global and Unhandled Exceptions

- AppDomain.UnhandledException: last-resort handler for unhandled exceptions on any thread.
- TaskScheduler.UnobservedTaskException: receives unhandled exceptions from Tasks (unobserved exceptions may terminate process in older .NET versions).
- For UI apps:
  - Windows Forms: Application.ThreadException
  - WPF: Application.DispatcherUnhandledException
- Use these to log, attempt graceful shutdown, or restart. Do not rely on them for recovery.

Example:
```csharp
AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    var ex = (Exception)e.ExceptionObject;
    LogCritical(ex);
    // maybe attempt graceful shutdown
};
```

---

## 18. Security Considerations and Information Disclosure

- Do not reveal internal exception messages to end users (avoid stack traces in error pages).
- Sanitize logs for sensitive information.
- Use custom error responses containing safe messages and correlation IDs for support.

---

## 19. Testing Exceptions (unit tests, integration tests)

- Test expected exceptions using testing frameworks.

xUnit:
```csharp
[Fact]
public void DoSomething_InvalidInput_ThrowsArgumentNullException()
{
    var sut = new MyService();
    Assert.Throws<ArgumentNullException>(() => sut.DoSomething(null));
}
```

NUnit:
```csharp
Assert.Throws<ArgumentNullException>(() => sut.DoSomething(null));
```

FluentAssertions:
```csharp
Action act = () => sut.DoSomething(null);
act.Should().Throw<ArgumentNullException>();
```

- Test async methods:
```csharp
await Assert.ThrowsAsync<MyException>(async () => await sut.DoAsync(null));
```

- Integration tests: verify logging, HTTP status codes, and error payloads.

---

## 20. Debugging Tips and Tools

- Use break on thrown exceptions in Visual Studio (Exception Settings) to catch exceptions where they are thrown.
- Inspect stack traces, inner exceptions, and local variables.
- Use First Chance Exceptions to understand flows (but avoid noise).
- Use dump debugging (dotnet-dump, WinDBG) for production crashes.
- Attach a logger and correlation IDs to trace errors across services.

---

## 21. Static Analysis, Roslyn Analyzers & Code Contracts

- Use Roslyn analyzers, FxCop, SonarQube to detect anti-patterns (empty catches, catch-all, discarded exceptions).
- Consider Code Contracts / nullable reference types and annotations to reduce NREs (NullReferenceException).
- Use nullable reference types (C# 8+) to catch potential null dereferences at compile time.

---

## 22. Common Mistakes and Anti-Patterns

- Swallowing exceptions:
  ```csharp
  catch (Exception) { /* nothing */ } // bad
  ```
- Catching System.Exception at low-level code.
- Using exceptions for common flow control.
- Re-throwing with throw ex; (loses stack trace).
- Overusing custom exceptions with no additional info.
- Not logging sufficient context.
- Leaking resources by not using using/Dispose.

---

## 23. Comprehensive Q&A — Developer & Interview Questions (with answers)

Q1: What is the difference between Exception and SystemException?  
A: Exception is the base class for all exceptions. SystemException is a subclass representing exceptions thrown by the CLR. ApplicationException exists but is not commonly used.

Q2: When should you create a custom exception?  
A: When you need to represent a domain-specific error that callers should catch explicitly, and when distinct handling or information is required.

Q3: Why is throw; preferred over throw ex;?  
A: throw; preserves the original stack trace. throw ex; resets the stack trace to the current location, losing original context.

Q4: How do async/await change exception handling?  
A: Awaiting a faulted task rethrows the original exception. Using Task.Wait/Result wraps exceptions in AggregateException. Use await and handle exceptions in try/catch around await.

Q5: What is AggregateException and how do you handle it?  
A: AggregateException holds one or more exceptions, used by TPL. Use Flatten, Handle, or iterate InnerExceptions to process each.

Q6: What are exception filters and why use them?  
A: Catch (E ex) when (condition) lets you conditionally catch exceptions without changing stack trace. Great for targeted logging and conditional handling.

Q7: How do you preserve stack trace when rethrowing on another thread?  
A: Use ExceptionDispatchInfo.Capture(ex).Throw() to preserve the original stack trace.

Q8: When should methods return Try-pattern instead of throwing?  
A: When the failure is expected and common (e.g., parse failure), returning a bool TryParse pattern is more efficient than throwing.

Q9: How should you map exceptions to HTTP status codes?  
A: Translate domain exceptions to appropriate codes (NotFoundException -> 404, ValidationException -> 400, UnauthorizedAccessException -> 401). Use middleware to centralize mapping.

Q10: How to handle exception logging in high-throughput systems?  
A: Use asynchronous/structured logging, avoid logging every exception verbatim in hot paths, sample logs where appropriate, attach correlation ids.

Q11: How do you avoid information disclosure in exceptions returned to clients?  
A: Return sanitized error messages and a correlation id. Log the full details server-side.

Q12: How do you test exception flows?  
A: Use unit tests that assert exceptions are thrown/translated. Use integration tests to verify HTTP responses and telemetry.

Q13: What is the best practice for resource cleanup in exceptions?  
A: Use using or try/finally to ensure Dispose is called. For asynchronous disposables use IAsyncDisposable and await using.

Q14: What is ExceptionDispatchInfo and when to use it?  
A: It captures an exception and its stack trace so it can be rethrown later with original information. Useful for preserving stack traces across async boundaries or when marshalling exceptions.

Q15: How to handle cancellation exceptions?  
A: Use CancellationToken and catch OperationCanceledException. Ensure you check token.IsCancellationRequested for cooperative cancellation.

Q16: Should you catch OutOfMemoryException or StackOverflowException?  
A: Generally no — these indicate unrecoverable conditions. StackOverflowException cannot be caught in most cases. OutOfMemoryException may be borderline; better to fail fast.

Q17: What are unobserved task exceptions, and how to mitigate them?  
A: Exceptions thrown in tasks that are never awaited may become unobserved. Handle TaskScheduler.UnobservedTaskException or ensure tasks are awaited/observed.

Q18: When should a library throw exceptions vs returning error codes?  
A: Libraries should throw for exceptional conditions and use Try-patterns for expected failures. For interop/native scenarios, mapping to HRESULT may be appropriate.

Q19: What are some guidelines for exception messages?  
A: Keep messages clear, actionable, and non-sensitive. Include context (ids, operation names) and avoid leaking system internals.

Q20: How to handle exceptions in parallel loops?  
A: Let Parallel.For/ForEach aggregate exceptions and catch AggregateException; or use PLINQ and handle OperationCanceledException appropriately.

Q21: What is the difference between AggregateException and Flatten()?  
A: Flatten() collapses nested AggregateException structures into a single top-level AggregateException with a flat InnerExceptions collection.

Q22: Explain the "Fail Fast" principle.  
A: When encountering a corrupted or unknown state, fail fast (terminate) to avoid data corruption and undefined behavior. Use Environment.FailFast if needed.

Q23: When is it appropriate to rethrow an exception wrapped in a new exception?  
A: When you need to add higher-level contextual information, translate to a domain-specific type, or map to a different API contract. Always include the original as InnerException.

Q24: How to handle exceptions across AppDomain boundaries?  
A: Exceptions crossing AppDomain boundaries must be serializable. Consider using remoting-safe exceptions or a transport-specific error representation.

Q25: What patterns help reduce exception-related bugs?  
A: Use Try-patterns, boundary-specific handling, strong typing, unit tests, nullable reference types, and static analyzers to catch likely issues early.

---

## 24. Practical Exercises & Projects

1. Basic exercises:
   - Implement a Calculator.Parse method that returns bool TryParseVariant instead of throwing for invalid input.
   - Build a small file reader that logs FileNotFoundException and retries with an alternative path.

2. Intermediate:
   - Create a custom DomainException hierarchy for an e-commerce checkout (PaymentFailedException, InventoryException). Demonstrate wrapping data store exceptions and mapping to HTTP responses.
   - Implement an ErrorHandlingMiddleware for ASP.NET Core that returns ProblemDetails and logs correlation ids.

3. Advanced:
   - Build a concurrent processing pipeline using Task.WhenAll and demonstrate handling AggregateException, logging every inner exception with context.
   - Simulate a long-running workflow that captures exceptions with ExceptionDispatchInfo to rethrow them on the calling thread later.
   - Create a library that exposes both throwing and Try-pattern APIs and write comprehensive unit tests for both.

4. Testing:
   - Use xUnit and FluentAssertions to write tests that assert thrown exceptions, exception messages, and inner exception types.
   - Create integration tests that verify correct HTTP status codes for various domain exceptions in your API.

---

## 25. References & Further Reading

- Microsoft Docs: Exception Handling (C# Programming Guide)  
  https://learn.microsoft.com/dotnet/csharp/fundamentals/exceptions/
- ExceptionDispatchInfo: https://learn.microsoft.com/dotnet/api/system.runtime.exceptionservices.exceptiondispatchinfo
- Task-based Asynchronous Pattern: https://learn.microsoft.com/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
- System.Threading.Tasks.Task.Exception and AggregateException documentation
- Best Practices for Exception Handling — various blog posts and Microsoft guidance
- Patterns of Enterprise Application Architecture (for failure patterns)
- Books and resources on distributed tracing and structured logging (Serilog, Application Insights)

---

Practical checklist for implementing exception handling in a codebase:
- [ ] Define exceptions for clear domain semantics.
- [ ] Document exception contracts on public APIs.
- [ ] Use try/catch at appropriate boundaries (top-level, worker entry points).
- [ ] Use using and try/finally for resource cleanup.
- [ ] Prefer Try-patterns for expected failures.
- [ ] Preserve stack traces when rethrowing.
- [ ] Centralize HTTP error mapping in middleware.
- [ ] Log structured exceptions with correlation ids.
- [ ] Write unit and integration tests for error paths.
- [ ] Use analyzers to catch common anti-patterns.

---

Prepared for developer interview preparation and practical engineering use. This guide can be used as a reference for coding interviews, architecture discussions, and code reviews related to exception handling in C# and .NET applications.