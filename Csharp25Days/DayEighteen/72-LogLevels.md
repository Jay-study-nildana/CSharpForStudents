# Log Levels and Correlation (C# / .NET)

Purpose
- Explain log levels and practical guidance for choosing them.
- Show how to add and propagate a CorrelationId across requests and services so logs can be tied together.
- Provide C# code examples (ILogger, middleware, HttpClient propagation, and JSON log examples).

Why levels and correlation matter
- Log levels let you control verbosity and focus: production systems should record important events (Info/Warning/Error) while dev environments can enable Debug/Trace.
- Correlation ties related log entries (request lifecycle, background job + downstream calls) so you can trace the path of a single operation across components and services.

Log level quick reference
- Trace: Extremely detailed; used only for diagnosing deep internals. High volume.
- Debug: Diagnostic information helpful during development (state, intermediate values).
- Information: High-level business events and important lifecycle events (request started/completed, major domain events).
- Warning: Unusual or recoverable problems that may need attention but not immediate action.
- Error: Failures of an operation that require investigation (exceptions that are handled or bubble up).
- Critical: System-wide failure or unrecoverable condition (outage, data corruption).

When to use which level (examples)
- Trace: "SQL generated: SELECT * FROM ...", internal cache key hit/miss counts.
- Debug: "Repository returned 42 items", configuration values loaded on startup.
- Information: "User {UserId} created order {OrderId}" — business events you want in dashboards.
- Warning: "ExternalService X returned 429 for user {UserId}" or validation warnings.
- Error: "Payment processing failed for order {OrderId}" with exception and context.
- Critical: "Message queue cannot be reached — system cannot continue."

Example: using ILogger with message templates
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
            // work...
            _log.LogDebug("Order {OrderId}: validated payload, items={ItemCount}", orderId, 3);
        }
        catch (ValidationException ve)
        {
            _log.LogWarning(ve, "Validation failed for order {OrderId}", orderId);
            throw;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to place order {OrderId}", orderId);
            throw;
        }
    }
}
```
- Use message templates (named properties) instead of string interpolation so structured log stores can index properties.

Correlation basics
- CorrelationId: a client-generated or server-generated identifier attached to a request. Include it in all logs for that request and return it in responses.
- For distributed systems prefer W3C TraceContext for traceparent/tracestate (OpenTelemetry) for full distributed tracing. For basic correlation, X-Correlation-ID is sufficient and widely used.

Middleware to establish CorrelationId and scope logs
```csharp
public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    public CorrelationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        var correlationId = ctx.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                           ?? ctx.TraceIdentifier
                           ?? Guid.NewGuid().ToString();

        // Add header to response
        ctx.Response.Headers["X-Correlation-ID"] = correlationId;

        // Create a logging scope so ILogger includes CorrelationId for all logs in this request
        using (var scope = ctx.RequestServices
                .GetRequiredService<ILogger<CorrelationMiddleware>>()
                .BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            await _next(ctx);
        }
    }
}
```
- Register middleware early in the pipeline so all downstream components inherit the scope.

Propagating correlation to downstream HTTP calls
- When calling other services, include the same correlation id header so downstream services can log with the same id.

HttpClient handler to propagate header:
```csharp
public class CorrelationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CorrelationHandler(IHttpContextAccessor accessor) => _httpContextAccessor = accessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx?.Request.Headers.TryGetValue("X-Correlation-ID", out var values) == true)
            request.Headers.TryAddWithoutValidation("X-Correlation-ID", values.First());

        return base.SendAsync(request, ct);
    }
}
```
- Register handler and IHttpContextAccessor in DI:
```csharp
services.AddHttpContextAccessor();
services.AddTransient<CorrelationHandler>();
services.AddHttpClient("Downstream")
        .AddHttpMessageHandler<CorrelationHandler>();
```

Correlation with Activity / OpenTelemetry
- For richer tracing across async calls and queues, use System.Diagnostics.Activity and OpenTelemetry exporters; they propagate W3C traceparent automatically.
- Example start activity:
```csharp
using var activity = new Activity("PlaceOrder").Start();
// add tags / baggage
activity.SetTag("OrderId", orderId.ToString());
```
- Prefer Activity/OpenTelemetry for production distributed tracing.

Sample_*
