# Dependency Injection and Modularity — C# .NET

This document explains the relationship between Dependency Injection (DI) and modular application design in .NET. It covers core DI concepts, how DI promotes modularity, practical patterns for composing modules, patterns for registration and configuration, testing benefits, common pitfalls, and classroom demo ideas.

---

## Overview

Dependency Injection is a design technique and a runtime mechanism where an external component (a DI container) provides dependencies to a class rather than the class creating them. Modularity is the design principle of dividing a system into separate, interchangeable components or modules with well-defined interfaces. DI and modularity complement each other: DI decouples implementations from consumers, enabling modules to be developed, tested, and evolved independently.

---

## Core concepts

- Service abstraction: Consumers depend on interfaces or abstractions (e.g., IOrderService) instead of concrete classes.
- Composition root: A single place where the application wires services together (typically in Program.cs or Startup.cs).
- Lifetime management: DI container manages lifetime (transient, scoped, singleton).
- Inversion of Control (IoC): Modules delegate creation and configuration of dependencies to the container.

---

## How DI promotes modularity

1. Loose coupling
   - By programming against interfaces, modules avoid concrete implementation dependencies. Modules can be swapped without changing consumers.

2. Clear boundaries
   - DI encourages defining small service interfaces and grouping related services inside modules, clarifying responsibilities.

3. Easier testing
   - You can inject test doubles (mocks, stubs) for external dependencies, enabling isolated unit tests for each module.

4. Configurable composition
   - The composition root controls which module implementations are used at runtime (e.g., different database providers, feature flags, or plugin implementations).

5. Plugin and extensibility models
   - DI makes it simple to discover and register plugins dynamically (via assembly scanning, configuration files, or explicit registration).

---

## Practical patterns for modular DI

1. Module as IServiceCollection extension
   - Each module exposes an extension method to register its services. The composition root calls these methods to assemble the app.

   Example:
   ```csharp
   public static class OrdersModule
   {
       public static IServiceCollection AddOrdersModule(this IServiceCollection services, IConfiguration config)
       {
           services.AddScoped<IOrderRepository, SqlOrderRepository>();
           services.AddScoped<IOrderService, OrderService>();
           // Module-specific configuration
           services.Configure<OrderOptions>(config.GetSection("Orders"));
           return services;
       }
   }
   ```

2. Composition root
   - Keep wiring and configuration in one place. Avoid newing concrete types deep in the codebase.

   Example:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   builder.Services.AddOrdersModule(builder.Configuration);
   builder.Services.AddPaymentsModule(builder.Configuration);
   var app = builder.Build();
   ```

3. Interface-based module boundaries
   - Define module entry points (facades) as interfaces to reduce surface area and hide implementation details.

   Example:
   ```csharp
   public interface IOrdersFacade
   {
       Task PlaceOrderAsync(OrderDto order);
   }
   ```

4. Assembly scanning for plugin registration
   - For plugin systems, load assemblies and register types that implement known interfaces.

   Example (simple):
   ```csharp
   var pluginTypes = pluginAssembly.GetTypes()
       .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);
   foreach (var t in pluginTypes)
       services.AddTransient(typeof(IPlugin), t);
   ```

5. Options and configuration per module
   - Use IOptions<T> to supply module-specific configuration so modules remain self-contained.

---

## Testing and maintainability

- Unit testing: Inject mock implementations of module interfaces to test business logic in isolation.
- Integration testing: Use a test composition root that registers fakes or in-memory providers for external dependencies (e.g., in-memory EF Core).
- Clear seams: Modules with a single façade/interface are easier to substitute and test.

---

## Performance and deployment considerations

- Registration time vs. runtime: Registration is a startup cost; keep runtime DI resolution fast by avoiding heavy logic in factories.
- Deployment: Modules can be packaged as NuGet packages or separate projects, enabling independent release cycles.
- Versioning: Define clear API surface (interfaces) and avoid breaking changes in module contracts; use adapter layers when evolving interfaces.

---

## Common pitfalls and how to avoid them

1. Overly large service registries
   - Problem: Composition root becomes a giant list of registrations.
   - Solution: Group registrations into module extension methods and delegate responsibility.

2. Captive dependency across module boundaries
   - Problem: A long-lived service depends on a short-lived (scoped) service, causing subtle lifecycle bugs.
   - Solution: Follow proper lifetimes and, if needed, create scopes explicitly using IServiceScopeFactory inside long-lived services.

3. Poorly defined interfaces
   - Problem: Bloated interfaces expose too much and couple modules tightly.
   - Solution: Prefer small, focused interfaces and explicit façades.

4. Hidden global state
   - Problem: Singletons storing mutable state create cross-module coupling.
   - Solution: Store per-request or per-user state in scoped services, and keep singletons immutable or thread-safe.

5. Tight coupling to DI container specifics
   - Problem: Scattering container-specific code or service-locator usage makes modules harder to reuse.
   - Solution: Keep container-specific code in the composition root and use constructor injection inside modules.

---

## Classroom demo ideas

1. Small modular app with three modules: Orders, Payments, Notifications
   - Each module has its own IServiceCollection extension and IConfiguration section.
   - Demonstrate swapping Payment provider (e.g., Stripe vs. Fake) by changing composition root registration.

2. Plugin system
   - Load plugin assemblies at runtime and register implementations of an IPlugin interface.
   - Show how new plugins can be added without recompiling the host.

3. Testing demo
   - Show unit tests for a module with mocked repositories, and an integration test with an in-memory provider.

4. Composition root live coding
   - Start with tight coupling (new inside classes), then refactor to DI with a composition root and module registration.

---

## Quick reference matrix

- Where to create instances:
  - Composition root (Program.cs/Startup) — registration and wiring.
- Module registration:
  - Use IServiceCollection extension methods per module.
- Configuration:
  - Use IOptions<T> and module-specific configuration sections.
- Plugin registration:
  - Assembly scanning or explicit configuration-driven registration.

---

## Conclusion

Dependency Injection is a foundational tool for building modular, testable, and extensible .NET applications. By organizing code into modules with clear interfaces, using module registration extension methods, keeping a single composition root, and following lifetime rules, you can design systems that are easy to maintain, evolve, and test. Use demos (provider swaps, plugins, tests) to make these ideas concrete for students.
