# Day 14 — Validation & Fail‑Fast: Practice Problems (C# / .NET)

Instructions
- Solve each problem in C# (.NET 6+) using idiomatic validation and fail‑fast principles.
- Prefer guard clauses, Try-patterns or ValidationResult for expected validation, and exceptions for programmer/infrastructure faults.
- Each solution should include a short explanation comment (why chosen approach) and a Main() demonstrating usage.

Problems

1) GuardClauses  
Implement a `Guard` static helper with methods `NotNull<T>(T? value, string name)`, `NotNullOrWhiteSpace(string? s, string name)`, and `InRange(int value, int min, int max, string name)`. Demonstrate usage in a method that constructs a `User` object and fails fast for invalid input.

2) ValidateUserDto  
Create a `UserDto` (Name, Email, Age). Implement `ValidationResult ValidateUser(UserDto dto)` that returns a ValidationResult containing error messages (do not throw). Show how a controller or caller would use the result to decide between returning a 400 or proceeding.

3) TryParseCustomId  
Design a `struct OrderId` that wraps a `Guid`. Implement `bool OrderId.TryParse(string input, out OrderId id)` following the Try-pattern (no exceptions for invalid parse). Demonstrate usage.

4) CustomExceptionWrapping  
Define a custom exception `OrderProcessingException` that carries `OrderId` and preserves inner exceptions. Write code that calls a `SaveOrder` method which may throw `InvalidOperationException` or `IOException` — catch low-level exceptions and wrap them in `OrderProcessingException` with `throw;` or inner-preserving approach as appropriate.

5) FailFastStartupConfig  
Write a `Bootstrapper` that reads a minimal configuration (e.g., `ConnectionString`) from a dictionary and validates required keys at startup. If missing or invalid, fail fast by throwing `InvalidOperationException` with a clear message. Show how to call this at program startup.

6) SafeResourceHandling  
Implement a method `ReadAllTextSafe(string path)` that opens a file, reads contents, and ensures cleanup using both `using` and `try/finally` (two variants). Demonstrate behavior when file is missing and show how to handle `FileNotFoundException`.

7) RetryTransientFailures  
Implement a `RetryPolicy` helper that retries an `Action` or `Func<Task>` up to N times catching only transient exceptions (simulate via `IOException`) with exponential backoff. On persistent failure rethrow the original exception. Demonstrate with a flaky function.

8) ParallelWorkWithAggregateException  
Run several tasks in parallel where some tasks throw exceptions. Show how to wait on all tasks, catch `AggregateException` (using Task.WaitAll or Task.WhenAll().Wait()), inspect `InnerExceptions`, and log/handle each. Also show the preferred `await` pattern and how it differs.

9) InputSanitizationAndLimits  
Write `string SanitizeAndValidate(string input, int maxLength)` that trims, normalizes whitespace, rejects overly long input (throw `ArgumentException`), and protects against huge inputs (return a validation error before heavy processing). Demonstrate protecting against a very large input string.

10) ValidationPipelineShortCircuit  
Implement a small validation pipeline for `NewOrderDto` where validators are functions `Func<NewOrderDto, ValidationResult>`. Compose validators and execute them in order; short‑circuit on first failure and return immediate `ValidationResult` (fail‑fast). Provide sample validators (non-empty customer id, at least one item, positive qty) and show usage.

Deliverables
- One `.md` file with the 10 problems (this file).
- Ten C# files (one per problem) implementing solutions with demonstration `Main()` methods.