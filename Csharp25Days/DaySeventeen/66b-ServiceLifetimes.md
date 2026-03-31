# Service lifetimes (conceptual) — C# .NET

This document explains the three primary service lifetimes used with Microsoft.Extensions.DependencyInjection in .NET: Transient, Scoped, and Singleton. It covers conceptual definitions, typical uses, key behaviors (including disposal and thread-safety), common pitfalls, and brief code examples you can show students.

---

## Overview

Dependency injection (DI) containers manage the creation and lifetime of service instances. A service lifetime determines how often a new instance is created and how long that instance is reused. Choosing the correct lifetime is important for correctness, performance, and resource management.

.NET's common lifetimes:
- Transient: a new instance every time the service is requested.
- Scoped: one instance per scope (often one HTTP request in web apps).
- Singleton: one instance for the application's lifetime.

---

## Transient (AddTransient)

Definition
- Transient services produce a new instance every time they are requested from the DI container.
- Registration example: services.AddTransient<IMyService, MyService>();

When to use
- Lightweight, stateless services.
- Small objects that do not hold shared state across calls.
- Services where per-call isolation is desired.

Characteristics and considerations
- Creates many short-lived objects — can increase GC pressure if used for large objects or very high request rates.
- Safe to store transient instances in request-local variables, but do not rely on them to carry per-request global state across resolutions (use scoped for that).
- Disposal: the DI container will dispose IDisposable instances it creates. If a transient is created from a scope, it will be disposed when that scope is disposed; if created from the root provider, when the root provider is disposed (typically app shutdown).

Example
```csharp
services.AddTransient<IGreeter, Greeter>();
```

---

## Scoped (AddScoped)

Definition
- Scoped services are instantiated once per DI scope. In ASP.NET Core, a scope is created per HTTP request by default, so scoped services are one-per-request.
- Registration example: services.AddScoped<IMyRepository, MyRepository>();

When to use
- Per-request state, such as DbContext (EF Core) or unit-of-work patterns.
- Services that must be shared during a single operation (e.g., a web request) but must not persist beyond that operation.

Characteristics and considerations
- A scoped instance is reused for all resolutions within the same scope.
- Scoped services depend on correct scope boundaries: outside a scope (e.g., in background threads not using a scope) you must create a scope manually (IServiceScopeFactory).
- Disposal: when the scope ends the DI container disposes IDisposable scoped services automatically.

Example
```csharp
services.AddScoped<ApplicationDbContext>();
services.AddScoped<IUserService, UserService>();
```

---

## Singleton (AddSingleton)

Definition
- Singleton services are created once and reused for the entire lifetime of the application (or until the root service provider is disposed).
- Registration example: services.AddSingleton<IMyCache, MemoryCache>();

When to use
- Stateless services that are expensive to create.
- Shared caches, configuration holders, services that maintain global, read-mostly resources.
- Services that must be the same instance everywhere.

Characteristics and considerations
- Singletons must be thread-safe because they may be accessed concurrently by multiple threads.
- Do not store per-request or per-user state in singletons.
- If a singleton depends on a scoped service, it causes a "captured" scoped dependency problem (see pitfalls).
- Disposal: IDisposable singletons are disposed when the root provider is disposed (usually when the app shuts down).

Example
```csharp
services.AddSingleton<IConfigurationCache, ConfigurationCache>();
```

---

## Common pitfalls and best practices

1. Captive dependency (injecting scoped into singleton)
   - Problem: If a singleton directly receives a scoped service via constructor injection, the scoped service may be resolved from the root provider and effectively become a singleton, or you may capture the wrong scope. This leads to incorrect lifetimes and possible data corruption.
   - Solutions:
     - Avoid injecting scoped services into singletons.
     - If a singleton must create scoped services, inject an IServiceScopeFactory or IServiceProvider and create a scope when needed:
       ```csharp
       public class MySingleton {
           private readonly IServiceScopeFactory _scopeFactory;
           public MySingleton(IServiceScopeFactory scopeFactory) { _scopeFactory = scopeFactory; }

           public void DoWork() {
               using var scope = _scopeFactory.CreateScope();
               var repo = scope.ServiceProvider.GetRequiredService<IMyRepository>();
               // use repo
           }
       }
       ```

2. Thread-safety for singletons
   - Always design singletons to be thread-safe. Use concurrent collections, locks, or immutability as needed.

3. Disposal semantics
   - The DI container disposes IDisposable services it created. Scoped services are disposed with their scope; singletons at provider disposal; transients disposed with the provider/scope that created them.
   - If you manually create instances (new MyService()), the container won't track them—so dispose them yourself.

4. Performance and memory
   - Transient creates many objects — consider pooling for heavy objects.
   - Singletons remain in memory for app lifetime — avoid holding large disposable resources unless intended.

5. Testing and design
   - Prefer small, focused services with clear lifetimes.
   - Use interfaces and register test doubles with appropriate lifetimes in tests.

---

## Quick guidance matrix

- Transient: Use for lightweight, stateless work. New instance per use. (AddTransient)
- Scoped: Use for per-request resources like DbContext, per-unit-of-work caches. One instance per scope. (AddScoped)
- Singleton: Use for shared, thread-safe, long-lived resources like caches or config. One instance for app lifetime. (AddSingleton)

---

## Teaching tips

- Demonstrate with a small web API: register a transient, scoped, and singleton service that each expose a GUID. Show that transient GUID changes every resolution, scoped stays constant during a request, and singleton remains constant across requests.
- Show the captive dependency example and the correct pattern using IServiceScopeFactory.
- Encourage students to think about state ownership (who owns the data?) and thread-safety.

---

## Short example for class demo

```csharp
public interface IExample { Guid Id { get; } }
public class Example : IExample { public Guid Id { get; } = Guid.NewGuid(); }

// In Startup or Program.cs:
services.AddTransient<IExample, Example>(); // transient
services.AddScoped<IExample, Example>();    // scoped
services.AddSingleton<IExample, Example>(); // singleton
```

Then inject IExample into controllers or services to observe ID behavior across requests and resolutions.

---

## Conclusion

Choosing the right lifetime avoids subtle bugs and improves performance. Use:
- Transient for ephemeral, stateless operations;
- Scoped for per-request/per-unit-of-work services (DbContext, repositories);
- Singleton for shared, long-lived, thread-safe services or caches.

When in doubt, prefer scoped for per-request scenarios and design singletons carefully for thread-safety. Demonstrations with GUIDs and an explanation of the captive dependency problem make the differences concrete for students.
