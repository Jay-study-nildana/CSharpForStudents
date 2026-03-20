# Structured Logging (C# / .NET)

Purpose
- Explain structured logging and why it matters.
- Show how to produce and consume structured logs in .NET (ILogger + Serilog examples).
- Provide practical patterns: message templates, properties, correlation, enrichment, sensitive-data handling, and sample log entries.

What is structured logging?
- Structured logging records log events as a timestamped record with named properties (key/value pairs) rather than plain free-form text.
- Example fields: timestamp, level, messageTemplate, and properties such as UserId, OrderId, CorrelationId, DurationMs.
- Structured logs are machine-readable (JSON) and make searching, filtering, and alerting more reliable.

Why prefer structured logs?
- Queryable: easily filter by properties (e.g., UserId, OrderId).
- Durable semantics: message templates separate intent from runtime values.
- Safer for parsing and aggregation by log stores (ELK, Seq, Splunk).
- Better for automated analysis (alerts, dashboards, metrics).

Message templates and properties
- Use message templates with named placeholders rather than string concatenation.
- The logger captures both the template and the property values for structured storage.

Example (constructor injection + template usage)
```csharp
public class OrderService
{
    private readonly ILogger<OrderService> _log;
    public OrderService(ILogger<OrderService> log) => _log = log;

    public async Task PlaceOrderAsync(Guid orderId, Guid userId)
    {
        _log.LogInformation("Placing order {OrderId} for user {UserId}", orderId, userId);
        try
        {
            // do work...
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to place order {OrderId} for user {UserId}", orderId, userId);
            throw;
        }
    }
}
```
- The log stores properties OrderId and UserId separately, enabling queries like `OrderId: "..." AND Level: "Error"`.

JSON output example (what a structured entry looks like)
```json
{
  "Timestamp":"2026-03-20T08:12:34.123Z",
  "Level":"Information",
  "MessageTemplate":"Placing order {OrderId} for user {UserId}",
  "RenderedMessage":"Placing order 4f2e... for user 1a7b...",
  "Properties": {
    "OrderId":"4f2e-...",
    "UserId":"1a7b-...",
    "RequestPath":"/api/orders"
  }
}
```

Serilog quick setup (recommended for structured JSON logs)
- Install packages:
  dotnet add package Serilog.AspNetCore
  dotnet add package Serilog.Sinks.File
  dotnet add package Serilog.Sinks.Console
  dotnet add package Serilog.Enrichers.Environment
- Program.cs (minimal)
```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(); // replace default logger
    var app = builder.Build();
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
```

Correlation and scopes
- Use a CorrelationId to tie logs across layers (HTTP request -> background job -> downstream calls).
- Use logging scopes (BeginScope) or Serilog’s LogContext to add correlated properties automatically.

Middleware to ensure a CorrelationId:
```csharp
public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    public CorrelationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var correlationId = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        using (_ = httpContext.RequestServices.GetRequiredService<ILogger<CorrelationMiddleware>>()
            .BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            httpContext.Response.Headers["X-Correlation-ID"] = correlationId;
            await _next(httpContext);
        }
    }
}
```
- All log entries within the request scope will include CorrelationId.

Enrichment and contextual properties
- Enrich logs with environment, machine name, version, and user identity.
- Use Serilog Enrichers (e.g., WithEnvironmentName, WithMachineName, WithProperty("Service","capstone-api")).

Sensitive data and redaction
- Never log raw secrets, passwords, full credit card numbers, or PII.
- Prefer logging identifiers (UserId) and redact or hash sensitive values.
- Use property filtering or custom sinks to remove sensitive properties before storage.

Example: avoid logging full payloads:
- Bad: _log.LogInformation("Received payment details: {Payload}", fullCardPayload);
- Better: _log.LogInformation("Payment requested for user {UserId}, amount {Amount}", userId, amount);

Performance and sampling
- Logging can be synchronous and expensive; use asynchronous sinks and batching (Serilog sinks often support buffering).
- For high-volume logs, implement sampling or rate-limiting (e.g., only record every Nth telemetry event or use adaptive sampling).
- Avoid expensive-to-compute log values when level would suppress them; prefer lazy evaluation via message templates.

Log levels and guidance
- Trace/Debug: detailed diagnostics (local dev).
- Information: important runtime events and normal operations (business events).
- Warning: unexpected but recoverable situations.
- Error: failures needing attention; include exceptions and context.
- Critical: unrecoverable or system-level failures.

Searchable, consistent keys
- Agree on property names (e.g., CorrelationId, UserId, OrderId, DurationMs).
- Prefer camelCase or PascalCase consistently across services.

Storage and analysis
- Export structured logs to centralized stores: ELK (Elasticsearch), Seq, Splunk, or cloud log services.
- Use indexes and dashboards for common queries: errors by endpoint, slow requests, user exceptions.

Sample log entry descriptions (homework hint)
- Successful order: Information — messageTemplate Placing order {OrderId} for user {UserId}
- External API timeout: Warning — external service {ServiceName} timed out after {ElapsedMs}
- Validation failure: Information/Warning — validation errors with field names (no PII)
- Unhandled exception: Error — exception with CorrelationId, stack trace, route, userId if available

Best practices summary
- Use structured message templates, not string interpolation.
- Inject ILogger<T> via constructor and call LogInformation/Error with typed properties.
- Add correlation IDs and use BeginScope/LogContext for automatic propagation.
- Enrich with environment/service info and redact sensitive fields.
- Choose asynchronous sinks, buffering, and sampling to avoid performance issues.
- Centralize log analysis (ELK/Seq) and standardize property names for reliable queries.

Further reading
- Serilog documentation and enrichers
- Microsoft.Extensions.Logging guidance
- Logging strategies for production systems (sampling, redaction, retention policies)
