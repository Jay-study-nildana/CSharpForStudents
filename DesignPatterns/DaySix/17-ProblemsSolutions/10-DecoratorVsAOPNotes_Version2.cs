// 10-DecoratorVsAOPNotes.cs
// Intent: Design notes comparing decorator chains vs AOP frameworks for cross-cutting concerns in .NET.
// DI/Lifetime: Decorators are explicit types; AOP applies aspects at method boundaries (weigh lifetimes accordingly).
// Testability: Decorators are easier to unit test; AOP requires testing both aspects and woven behavior.

using System;

/// <summary>
/// Decorator chains vs AOP frameworks — concise comparison and checklist
///
/// Pros: Decorator chains
/// - Explicit runtime composition; easy to reason about call order and side-effects.
/// - Type-safe and compatible with standard DI; easy to mock inner components for unit testing.
/// - No additional tooling or build-time weaving required.
/// - Fine-grained control over ordering by composition.
///
/// Cons: Decorator chains
/// - Boilerplate to create many small decorator classes.
/// - Composition wiring can be verbose for many concerns; ordering mistakes cause subtle bugs.
/// - Potential performance overhead from many small wrappers per call.
///
/// Pros: AOP frameworks (e.g., PostSharp, Castle DynamicProxy with interceptors, source generators)
/// - Declarative application of cross-cutting concerns; less boilerplate in code.
/// - Can apply aspects across many classes/methods automatically.
/// - Centralized definition of interception logic (logging, metrics, retries).
///
/// Cons: AOP frameworks
/// - Hidden runtime behavior; callers don’t see interception in the class, making debugging harder.
/// - Tooling/complexity: weaving/proxying can complicate build/test tooling and stack traces.
/// - Some techniques rely on dynamic proxies which may not intercept non-virtual methods or static calls.
/// - Testability: aspects may need separate tests; mocking interplay between aspects and SUT can be harder.
///
/// Decision checklist (use to choose approach)
/// 1) Do you need explicit, testable control over ordering and composition? => Prefer Decorators.
/// 2) Is reducing boilerplate across many classes a high priority and your team comfortable with tooling? => Consider AOP.
/// 3) Do you require interception of non-virtual/static methods? => AOP with IL weaving may be necessary.
/// 4) Is runtime transparency and debuggability important (novice teams)? => Prefer Decorators for clarity.
/// 5) Do you need to apply concerns across an entire service surface consistently without changing class code? => AOP can help.
///
/// Recommendation for teaching and small .NET apps:
/// - Start with Decorator pattern: explicit, easy to test, and teaches composition over inheritance.
/// - Introduce AOP only when the team is comfortable and you have a clear need to reduce repetitive wiring.
///
/// </summary>
public static class Notes { }