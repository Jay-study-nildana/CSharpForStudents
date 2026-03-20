# Day 14 — Exception Handling & Defensive Programming (C# / .NET)

Objectives
- Understand try / catch / finally semantics and exception propagation.
- Recognize common built-in exception types and when to use them.
- Design and implement custom exceptions correctly.
- Apply validation strategies and the fail‑fast principle for robust code.

Why exceptions matter
Exceptions represent unexpected or error conditions that disrupt normal control flow. Proper handling makes programs more robust and maintainable. But exceptions are not a substitute for validation or normal control flow — they should be used for exceptional conditions, not routine branching.

Try / catch / finally: semantics (basic)
- try: run a block of code that may throw.
- catch: handle exceptions of specific types. You can have multiple catch blocks; the first compatible one runs.
- finally: executes regardless of whether an exception was thrown (useful to free resources).

Key points:
- If a catch handles and does not rethrow, execution continues after the try/catch/finally.
- If a catch rethrows (using `throw;` or by not catching), the exception propagates to the caller.
- `finally` always runs, even when returning inside try or catch (except process termination).

Example — basic usage:
```csharp
try
{
    var text = File.ReadAllText(path);
    Process(text);
}
catch (FileNotFoundException fnf)
{
    // handle specific known error
    Console.WriteLine($"File not found: {fnf.FileName}");
}
catch (IOException io)
{
    // handle other IO issues
    Console.WriteLine($"IO error: {io.Message}");
}
finally
{
    // cleanup, always executed
    ReleaseTemporaryResources();
}
```

Exception propagation and rethrowing
- To preserve the original stack trace when rethrowing, use `throw;` inside a catch block.
- Avoid `throw ex;` — it resets the stack trace and makes debugging harder.

Example — correct rethrow:
```csharp
try
{
    DoWork();
}
catch (Exception)
{
    // maybe log details, then rethrow preserving stack
    Log("error happened");
    throw; // preserves original stack trace
}
```

When to catch specific types vs System.Exception
- Prefer catching specific exceptions (FileNotFoundException, ArgumentException). Catching `System.Exception` hides bugs and makes diagnosis hard.
- Use broad catches centrally (e.g., at top-level request handler) to log and fail gracefully.

Exception filters (C# feature)
- Filters let you conditionally catch exceptions without changing the exception flow when filter fails:
```csharp
catch (Exception ex) when (ex is SqlException sql && sql.Number == 547)
{
    // handle FK constraint violation
}
```
This keeps intent explicit and avoids nested logic in catch bodies.

Built-in exception types (common guidance)
- ArgumentNullException — argument is null and must not be.
- ArgumentException / ArgumentOutOfRangeException — invalid argument value or range.
- InvalidOperationException — method call invalid for the object's current state.
- IOException, FileNotFoundException — IO-specific errors.
- TimeoutException — operation timed out.
- NotSupportedException — operation is not supported.
Use the most specific type that accurately describes the failure.

Validation strategies and guard clauses (fail-fast)
- Validate inputs early and fail fast with clear exceptions. This makes errors visible close to the origin.
- Use guard clauses to keep methods short and readable.

Example — guard clauses:
```csharp
public void AddUser(string name, int age)
{
    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name is required", nameof(name));
    if (age < 0 || age > 120) throw new ArgumentOutOfRangeException(nameof(age));

    // proceed with logic
}
```

Avoid using exceptions for control flow — prefer Try-style methods:
- For expected failure scenarios, provide `TryX` patterns (e.g., `int.TryParse`) instead of throwing repeatedly.

Custom exceptions: patterns and best practices
When to create custom exceptions:
- You need a new semantic category for callers to catch.
- You want to include domain-specific data (e.g., OrderId) with the error.

Design rules:
- Derive from `Exception` (or a more specific built-in when appropriate).
- Provide standard constructors:
  - parameterless
  - message
  - message + innerException
  - protected serialization constructor (if you need serialization)
- Make them immutable or expose read-only properties for extra data.
- Keep messages clear, actionable, and not localized (log-localizable UI separately).

Custom exception example:
```csharp
[Serializable]
public class OrderProcessingException : Exception
{
    public int OrderId { get; }

    public OrderProcessingException() { }

    public OrderProcessingException(string message) : base(message) { }

    public OrderProcessingException(string message, Exception inner) : base(message, inner) { }

    public OrderProcessingException(int orderId, string message, Exception inner = null)
        : base(message, inner)
    {
        OrderId = orderId;
    }

    // If your type is serialized (remoting, distributed caches), include this:
    protected OrderProcessingException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        OrderId = info.GetInt32(nameof(OrderId));
    }

    // If serialized, override GetObjectData:
    public override void GetObjectData(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(OrderId), OrderId);
    }
}
```

Wrapping exceptions vs preserving inner exceptions
- When converting low-level exceptions to higher-level ones, wrap the original exception as `innerException` so diagnostics remain available:
```csharp
try
{
    SaveToDatabase(order);
}
catch (SqlException ex)
{
    throw new OrderProcessingException(order.Id, "Failed to save order.", ex);
}
```

Logging and observability
- Log sufficient context and the exception (message + stack) at appropriate levels.
- Avoid logging sensitive data.
- Use structured logging to attach properties (orderId, userId).

Async exceptions, Task, and AggregateException
- In async methods (`async/await`), exceptions are captured into the returned `Task` and rethrown when awaited.
- When using `Task.Wait()` / `Task.Result` or `Task.WhenAll`, multiple exceptions can aggregate into `AggregateException`. Prefer `await` which unwraps the original exception.

Example — async:
```csharp
public async Task ProcessAsync()
{
    try
    {
        await DoSomethingAsync();
    }
    catch (IOException ex)
    {
        // handle I/O issues
    }
}
```

Defensive programming checklist
- Validate inputs (guard clauses).
- Use the most specific exception types.
- Catch exceptions at boundaries (API edges, background worker entry points), not everywhere.
- Fail fast for incorrect API usage; allow higher-level handlers to log and translate exceptions for users.
- Don’t swallow exceptions silently — log or rethrow.
- Use `using` statements (or `try/finally`) to ensure resources are freed.

When *not* to use exceptions
- For routine, expected control flow (use Try patterns).
- To signal success/failure in performance-critical loops.
- As a substitute for validating user input — validate instead.

Sample error-handling policy (short)
- Throw specific exceptions for invalid arguments (ArgumentNullException, ArgumentException).
- Wrap infrastructure exceptions into domain-level exceptions with inner exceptions preserved.
- Catch exceptions near the top-level request/worker to log and convert to friendly messages/exit codes.
- Never use exceptions for normal control flow; provide TryX alternatives.
- Always clean up resources in finally blocks or via `using`.

Summary
Exceptions are a powerful tool for handling unexpected conditions. Use try/catch/finally thoughtfully: prefer specific catches, preserve stack traces, validate early, and design custom exceptions only when
