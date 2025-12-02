# Dependency Injection & IoC — C#/.NET Reference with Examples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [What are Dependencies?](#what-are-dependencies)  
3. [Definition of IoC and DI](#definition-of-ioc-and-di)  
4. [Why Refactor Toward Better Design?](#why-refactor-toward-better-design)  
5. [Dependency Injection Techniques](#dependency-injection-techniques)  
   - Constructor Injection  
   - Property (Setter) Injection  
   - Interface Injection  
6. [DI Frameworks for .NET](#di-frameworks-for-net)  
7. [Architectural Implications of DI](#architectural-implications-of-di)  
   - Lifetimes: Transient / Scoped / Singleton  
   - Composition Root pattern  
   - Testability, SOLID, Separation of Concerns  
8. [DI and IoC Containers — Concepts & Features](#di-and-ioc-containers---concepts--features)  
   - Registration & Resolution  
   - Open generics, named/Keyed services, interceptors, decorators  
9. [Building a Simple IoC Container (example)](#building-a-simple-ioc-container-example)  
   - Simple register & resolve implementation (C#)  
10. [Service Locator — Pattern & Anti-pattern](#service-locator---pattern--anti-pattern)  
11. [Common Pitfalls & How to Avoid Them](#common-pitfalls--how-to-avoid-them)  
12. [Best Practices & Guidelines](#best-practices--guidelines)  
13. [Comprehensive Q&A — Interview-style (short)](#comprehensive-qa---interview-style-short)  
14. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This document explains Dependency Injection (DI) and Inversion of Control (IoC) in the context of C#/.NET applications. It covers techniques (constructor, property, interface), DI frameworks, architectural implications (lifetimes, composition root), how IoC containers work, and provides practical code samples including a minimal IoC container implementation and DI usage with Microsoft.Extensions.DependencyInjection and Autofac. It also explains the Service Locator pattern and why it's usually considered an anti-pattern.

---

## 2. What are Dependencies?

A dependency is any object or value your class needs to do its job.

Example:
```csharp
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService() {
        _repo = new SqlOrderRepository(); // concrete dependency created inside the class
    }
    // ...
}
```
Problems with this approach:
- Tight coupling to concrete types
- Hard to unit test (can't easily swap repository)
- Violates Dependency Inversion Principle

Better: express dependency via abstraction (interface) and inject concrete implementation from the outside.

---

## 3. Definition of IoC and DI

- IoC (Inversion of Control): a design principle where control of object creation and lifetime is inverted — the framework or external component takes responsibility for wiring dependencies rather than objects constructing their own collaborators.
- DI (Dependency Injection): a pattern and mechanism to provide dependencies to a class, typically via constructor, property (setter), or interface injection.

DI is one way to implement IoC.

---

## 4. Why Refactor Toward Better Design?

Refactoring to DI improves:
- Testability: easily provide mocks/fakes
- Maintainability: swap implementations with minimal changes
- Separation of concerns & single responsibility: classes focus on a single role
- Composition: centralize object graphs and configuration

Small refactor example:

Before (tight coupling):
```csharp
public class PaymentProcessor {
    private StripeClient _stripe = new StripeClient();
}
```

After (inverted dependency):
```csharp
public class PaymentProcessor {
    private readonly IPaymentClient _paymentClient;
    public PaymentProcessor(IPaymentClient paymentClient) {
        _paymentClient = paymentClient;
    }
}
```

---

## 5. Dependency Injection Techniques

### Constructor Injection (recommended)
- Dependencies are provided through the constructor.
- Ensures immutability and that required dependencies are present.

Example:
```csharp
public interface IEmailSender { Task SendAsync(string to, string subject, string body); }

public class SmtpEmailSender : IEmailSender {
    public Task SendAsync(string to, string subject, string body) { /* send */ return Task.CompletedTask; }
}

public class NotificationService {
    private readonly IEmailSender _emailSender;
    public NotificationService(IEmailSender emailSender) {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }
    public Task NotifyAsync(string userEmail) => _emailSender.SendAsync(userEmail, "Hi", "Body");
}
```

Registration with Microsoft DI:
```csharp
var services = new ServiceCollection();
services.AddTransient<IEmailSender, SmtpEmailSender>();
services.AddTransient<NotificationService>();
var provider = services.BuildServiceProvider();
var notif = provider.GetRequiredService<NotificationService>();
```

### Property (Setter) Injection
- Dependencies are set via writable properties.
- Useful for optional dependencies or legacy frameworks that require parameterless constructors.
- Less safe — dependency might be null if not set.

Example:
```csharp
public class ReportService {
    public IFormatter Formatter { get; set; } // optional
    public void Run() {
        var f = Formatter ?? new DefaultFormatter();
        Console.WriteLine(f.Format("data"));
    }
}
```

### Interface Injection (rare)
- The dependency supplies an interface that the consumer implements to receive the dependency.
- Not commonly used in .NET; supported conceptually.

---

## 6. DI Frameworks for .NET

Popular DI containers:
- Microsoft.Extensions.DependencyInjection (built-in, recommended for most apps)
- Autofac (feature-rich: modules, advanced lifetime scopes)
- Simple Injector (performance-oriented and simple semantics)
- Ninject (older, flexible)
- StructureMap (now merged into Lamar for .NET Core)
- Castle Windsor (mature, advanced features)

Example — Autofac basic registration:
```csharp
var builder = new ContainerBuilder();
builder.RegisterType<SmtpEmailSender>().As<IEmailSender>().InstancePerDependency();
builder.RegisterType<NotificationService>().InstancePerDependency();
var container = builder.Build();
using var scope = container.BeginLifetimeScope();
var svc = scope.Resolve<NotificationService>();
```

When to prefer Microsoft DI:
- Simplicity, integration with ASP.NET Core built-in features (logging, config, options), and good enough for most apps.
Choose third-party when you need:
- rich modules, interception, decorators, complex lifetime scopes, or legacy features.

---

## 7. Architectural Implications of DI

Lifetimes (important in .NET DI containers):
- Transient: new instance every time (services.AddTransient<T>())
- Scoped: one instance per "scope" (typically per web request in ASP.NET Core) (services.AddScoped<T>())
- Singleton: one instance for application lifetime (services.AddSingleton<T>())

Key rules:
- Do not inject a Scoped service into a Singleton — it can lead to captured scoped state beyond intended lifetime.
- Dispose IDisposable created by container will be handled by the container for Scoped/Singleton lifecycle; transient disposal sometimes needs attention.

Composition Root
- All wiring/registration should happen at application start in a single place (Composition Root). Avoid scattering service registrations across code.
- Composition root is the only place with knowledge of concrete types.

DI improves SOLID:
- Single Responsibility: classes depend on abstractions, not on creation logic.
- Open/Closed: implementations can be replaced without modifying consumers.

---

## 8. DI and IoC Containers — Concepts & Features

Common container features:
- Registration by type, by factory, by instance
- Named/keyed registrations
- Open generic registration (e.g., IRepository<T>)
- Interception / AOP (logging, transactions)
- Lifetime scopes / nested containers
- Auto-wiring/resolving by constructor injection
- Resolution of IEnumerable<T> for multiple registrations

Examples of registration patterns:

Register by type:
```csharp
services.AddTransient<IRepository, SqlRepository>();
```

Register factory:
```csharp
services.AddSingleton(sp => {
    var cfg = sp.GetRequiredService<IConfiguration>();
    return new MyService(cfg["Url"]);
});
```

Open generics:
```csharp
services.AddTransient(typeof(IRepository<>), typeof(SqlRepository<>));
```

Resolve multiple registrations:
```csharp
// When multiple IValidator<T> registered, constructor can accept IEnumerable<IValidator<T>>
```

---

## 9. Building a Simple IoC Container (example)

This minimal container demonstrates core ideas: register mapping and resolve instances, basic singleton and transient lifetimes, and constructor injection. It is intentionally simplistic and lacks many production features (thread-safety, disposing, scope handling, circular detection, open generics).

Example implementation:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public enum Lifetime { Transient, Singleton }

public class SimpleContainer
{
    private readonly Dictionary<Type, (Type implementation, Lifetime lifetime)> _registrations = new();
    private readonly Dictionary<Type, object> _singletons = new();

    public void Register<TService, TImpl>(Lifetime lifetime = Lifetime.Transient)
    {
        _registrations[typeof(TService)] = (typeof(TImpl), lifetime);
    }

    public void Register<TService>(Func<SimpleContainer, object> factory, Lifetime lifetime = Lifetime.Transient)
    {
        // For brevity we don't store factory separately here; a production container would.
        throw new NotImplementedException("Factory registration not implemented in this sample.");
    }

    public T Resolve<T>() => (T)Resolve(typeof(T));

    private object Resolve(Type serviceType)
    {
        if (_singletons.TryGetValue(serviceType, out var inst)) return inst;

        if (!_registrations.TryGetValue(serviceType, out var reg))
        {
            if (!serviceType.IsAbstract && !serviceType.IsInterface)
            {
                // Attempt to construct concrete type directly
                reg = (serviceType, Lifetime.Transient);
            }
            else
                throw new InvalidOperationException($"Service {serviceType} not registered");
        }

        var implType = reg.implementation;
        // Choose longest constructor (simple heuristic)
        var ctor = implType.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        var args = ctor.GetParameters()
            .Select(p => Resolve(p.ParameterType)).ToArray();

        var obj = Activator.CreateInstance(implType, args);

        if (reg.lifetime == Lifetime.Singleton)
            _singletons[serviceType] = obj;

        return obj;
    }
}
```

Usage:
```csharp
public interface IRepository { void Save(); }
public class Repo : IRepository { public void Save() => Console.WriteLine("Saved"); }

var c = new SimpleContainer();
c.Register<IRepository, Repo>(Lifetime.Singleton);

var repo = c.Resolve<IRepository>();
repo.Save();
```

Notes:
- Production containers add thread-safety, circular dependency detection, scope support, factory delegates, open-generic support, registration validation, disposal of IDisposable, and performance optimizations.
- Building a small container is a good learning exercise but prefer battle-tested libraries in production.

---

## 10. Service Locator — Pattern & Anti-pattern

Service Locator pattern: a central registry (static or passed) that returns services by type/name at runtime.

Example:
```csharp
public static class ServiceLocator
{
    public static IServiceProvider Provider { get; set; }
}

// Usage later
var repo = ServiceLocator.Provider.GetService<IRepository>();
```

Why it's considered an anti-pattern:
- Hides dependencies: consumers request services from the locator rather than declare them explicitly in constructor — reduces transparency and testability.
- Tight coupling to locator API (static access) makes unit testing harder and violates Dependency Inversion/Explicit Dependencies.
- Harder to reason about object graph and lifetimes.

When is Service Locator OK?
- Rarely. It may appear during migration, legacy code, or for framework-level integration where DI container isn't available. Prefer constructor injection and composition root.

---

## 11. Common Pitfalls & How to Avoid Them

- Injecting many dependencies (constructor with 6+ params): indicates class has too many responsibilities — refactor to smaller classes or apply Facade pattern.
- Capturing Scoped services in Singletons: leads to state leaks; enforce lifetime correctness.
- Overusing Service Locator: hide dependencies — prefer explicit injection.
- Circular dependencies: avoid by refactoring or using factory/deferred injection (Func<T> or IServiceProvider in limited cases).
- Registering IDisposable transient services that are resolved outside container scope — ensure container manages lifetime, or explicitly dispose.
- Resolving services from provider in random places across app; keep resolution centralized at composition root except in frameworks that provide proper scope (e.g., ASP.NET Core controllers).

---

## 12. Best Practices & Guidelines

- Register services in the Composition Root only.
- Favor constructor injection for required dependencies.
- Use interfaces for abstractions; prefer small focused interfaces.
- Use Microsoft.Extensions.DependencyInjection for ASP.NET Core apps; only add third-party container when features needed.
- Keep constructor parameter count small; use factories to create complex aggregates.
- Prefer Scoped for per-request data and Singleton for stateless shared services.
- Use logging and health checks as decoupled services.
- Validate registrations at startup when possible (some containers support Verify()).
- Avoid service location and global static access to service provider.
- When using third-party DI, learn lifetime semantics (Autofac lifetime scopes vs MS DI scopes).
- Use open generic registrations for generic repositories or handlers.

---

## 13. Comprehensive Q&A — Interview-style (short)

Q: What is Dependency Injection?  
A: A pattern to provide dependencies from outside a class (constructor/property/ method), promoting loose coupling and testability.

Q: What is IoC container?  
A: A library that performs registration and resolution of dependencies (construct object graphs, manage lifetimes, and provide instances).

Q: What is Composition Root?  
A: The single location in application startup where object graph/wiring is configured (registrations happen here).

Q: When to use Scoped lifetime?  
A: For services that should be created once per scope (e.g., per HTTP request in ASP.NET Core), such as DbContext.

Q: How to resolve circular dependencies?  
A: Refactor to remove cycle, use factories (Func<T>), or inject IServiceProvider (last resort) to resolve lazily.

Q: Why avoid Service Locator pattern?  
A: It hides dependencies, makes testing harder and violates explicit dependency principles.

Q: How to register open generic types?  
A: In Microsoft DI: services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

---

## 14. References & Further Reading

- Microsoft docs — Dependency injection in .NET: https://learn.microsoft.com/dotnet/core/extensions/dependency-injection  
- Microsoft.Extensions.DependencyInjection API docs  
- Autofac docs — https://autofac.org/  
- "Dependency Injection Principles, Practices, and Patterns" (book) — Mark Seemann & Steven van Deursen  
- "Clean Architecture" — Robert C. Martin (Uncle Bob) — for architectural implications  
- Articles: "Composition Root" — Mark Seemann blog

---

Prepared as a focused C#/.NET reference on Dependency Injection, IoC containers, techniques, patterns, and code samples to help implement DI safely in real-world apps.  