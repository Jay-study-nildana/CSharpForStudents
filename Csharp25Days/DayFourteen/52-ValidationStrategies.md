# Day 14 — Validation Strategies & Fail‑Fast Principles (C# / .NET)

Why validation and fail‑fast matter
Validation makes assumptions explicit and prevents invalid data from propagating through the system. Failing fast (detecting and stopping on invalid input early) reduces debugging time, avoids hidden bugs, and keeps error handling localized and meaningful. In C#/.NET, combine guard clauses, typed validation results, and domain invariants to build robust, maintainable code.

Principles
- Validate at boundaries: check inputs at the public API (controller, service entry, file parser, configuration loader).
- Fail fast and clearly: reject invalid input early with clear messages (use exceptions for programmer/API contract violations; use result objects for expected validation failures).
- Use the right tool: exceptions for exceptional programmer errors, Try-patterns or ValidationResult for expected validation failures (user input).
- Keep invariants inside types: enforce class invariants in constructors/factory methods.
- Avoid throwing exceptions for ordinary control flow in hot paths—use TryParse, TryGetValue, or result types.

Guard clauses — concise precondition checks
Use small guard helpers to keep methods readable and ensure input correctness.

```csharp
public static class Guard
{
    public static T NotNull<T>(T? value, string paramName)
        where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    public static void NotNullOrWhiteSpace(string? s, string paramName)
    {
        if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Value is required", paramName);
    }

    public static void InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName);
    }
}
```

Usage:
```csharp
public void CreateUser(string name, int age)
{
    Guard.NotNullOrWhiteSpace(name, nameof(name));
    Guard.InRange(age, 0, 120, nameof(age));
    // proceed ...
}
```

Preconditions vs expected validation
- Preconditions: programmer contract violations (null arguments, invalid state). Throw specific exceptions (ArgumentNullException, ArgumentException, ArgumentOutOfRangeException, InvalidOperationException).
- Expected validation: user-provided data that may be invalid (form fields, uploaded file). Return validation results (error list) rather than throwing for control flow—this is friendly to callers and avoids frequent exceptions.

Try-patterns and result objects
For operations where failure is common and expected, expose Try-style methods or result objects:

```csharp
public bool TryParseOrderId(string input, out Guid orderId) =>
    Guid.TryParse(input, out orderId);

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();
}
```

Example: returning validation results from a service:
```csharp
public ValidationResult ValidateNewOrder(NewOrderDto dto)
{
    var result = new ValidationResult();
    if (string.IsNullOrWhiteSpace(dto.CustomerEmail)) result.Errors.Add("Customer email required.");
    if (dto.Items.Count == 0) result.Errors.Add("At least one item required.");
    return result;
}
```

Enforce invariants in domain types
Keep objects valid by construction—fail fast in constructors or factory methods when invariants are violated.

```csharp
public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency required", nameof(currency));

        Amount = amount;
        Currency = currency;
    }
}
```

Validation at system boundaries
- API controllers: validate model state and return 400 with details rather than throwing for user mistakes.
- Background workers: validate configuration at startup and fail fast (prevent long-running, misconfigured runs).
- Configuration: validate required configuration and crash during startup with clear logs if missing.

ASP.NET Core controller example:
```csharp
[HttpPost]
public IActionResult Create([FromBody] OrderDto dto)
{
    if (!ModelState.IsValid) return BadRequest(ModelState);
    var validation = _orderService.ValidateNewOrder(dto);
    if (!validation.IsValid) return BadRequest(validation.Errors);
    _orderService.Create(dto);
    return Accepted();
}
```

Configuration validation at startup:
```csharp
var settings = configuration.GetSection("MyApp").Get<MySettings>();
if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
{
    throw new InvalidOperationException("Missing MyApp:ConnectionString in configuration");
}
```

Custom validation frameworks and attributes
- DataAnnotations: quick validation attributes for DTOs (`[Required]`, `[Range]`, `[EmailAddress]`) used in MVC model binding.
- FluentValidation or custom validators: for richer logic and testable validation rules.
Use frameworks for complex input validation and keep domain validation inside domain models.

Error messages and observability
- Give clear, actionable messages (what failed, why, and which field).
- Avoid leaking internal or sensitive information (e.g., stack traces, secrets) to end users; log those details securely for operators.
- Use structured logging to attach contextual properties (userId, orderId) when validation fails.

When to use exceptions vs return results
- Use exceptions for programmer/API errors and unexpected failures (missing dependency, disk I/O failure, corrupted state). These are rarer and indicate bugs or infrastructure problems.
- Use result objects/Try-pattern for validation of user input or routine conditions (e.g., lookup not found).

Example: parsing user input
```csharp
if (!int.TryParse(input, out var id))
{
    // expected validation failure — handle gracefully
    return BadRequest("Id must be a number");
}
```

Fail‑fast in long‑running apps
Validate critical dependencies and configuration during startup so that the service fails fast rather than running misconfigured. This reduces surprising runtime errors and helps CI/CD detect misconfigurations early.

Security and validation
- Sanitize inputs and avoid trusting client data.
- Validate lengths/characters to prevent denial-of-service via huge inputs.
- Principle of least privilege: validate and normalize before passing to sensitive subsystems (DB, file system, command execution).

Testing validation
- Unit-test guard clauses and validators with valid and invalid inputs.
- Test domain invariant enforcement by attempting invalid constructions.
- For controllers, test both valid and invalid model states (including edge cases).

Checklist: defensive validation & fail‑fast
- Validate inputs at public API boundaries.
- Use guard clauses for programmer contracts and throw specific exceptions.
- Use Try-patterns or ValidationResult for expected user-level validation failures.
- Enforce invariants inside constructors/factory methods.
- Fail fast for missing configuration or unrecoverable startup errors.
- Log errors with context; avoid leaking sensitive info to users.
- Prefer small, testable validation units (validators) instead of ad-hoc checks scattered across code.

Summary
Validation and fail‑fast design keep errors close to their origin and make systems easier to reason about and maintain. Use guard clauses and exceptions for programmer contract violations, result objects or Try-patterns for expected user validation, and centralize boundary checks. Validate eagerly at startup and at API edges so invalid states never silently propagate into production behavior.