# Day 6 — Exercises: Decorator & Proxy (10 Problems)

Instructions
- Solve the 10 problems below. Each problem focuses on Decorator and Proxy patterns, composition vs inheritance, stacking decorators, access control, lazy initialization, DI registration, testing, and tradeoffs with AOP.
- Implement each solution in a separate C# file named exactly as the problem title (prefix with the two-digit ID). Example: Problem "01-Decorator_Basic.cs" → solution file "01-Decorator_Basic.cs".
- At the top of each solution file include a short comment summarizing: pattern intent, recommended DI registration/lifetime, and a testability note.
- Keep examples small and focused so they can be used in class demos or turned into runnable samples later.

Problems

1) 01-Decorator_Basic.cs  
Problem: Implement a simple Decorator around an `IService` with a `RealService` and a `LoggingDecorator`. Show usage where the client depends on `IService` only. Include a top comment with intent and DI/lifetime guidance.

2) 02-Decorator_StackingOrder.cs  
Problem: Demonstrate three decorators — `ValidationDecorator`, `CachingDecorator`, `LoggingDecorator` — and show how stacking order affects behavior. Provide two composition examples and short comments explaining differences.

3) 03-Decorator_Caching.cs  
Problem: Implement `CachingDecorator` that caches `GetData(int id)` results. Provide a fake inner service that counts calls; include a short test (conceptual) showing caching reduces inner calls.

4) 04-Decorator_Validation.cs  
Problem: Implement `ValidationDecorator` that validates inputs (e.g., id > 0) before delegating. Show the decorator rejecting invalid input without calling inner service. Include example usage and expected behavior.

5) 05-Proxy_LazyInitialization.cs  
Problem: Implement a `HeavyResourceProxy` that lazily initializes a `HeavyResource` (costly constructor) and implements `IHeavyResource`. Demonstrate that the real resource is only created on first method call.

6) 06-Proxy_Authorization.cs  
Problem: Implement a `ProtectionProxy` that checks an `IAuthorizationService` delegate before delegating to the real subject. Show testable usage where the authorization delegate can be switched.

7) 07-DecoratorAndProxy_DIRegistration.cs  
Problem: Provide an `IServiceCollection`-style registration snippet (comment) showing how to register real service, decorators, and proxies. Show both manual factory composition and using a decorator registration approach. Explain recommended lifetimes.

8) 08-Decorator_Tests.cs  
Problem: Provide example unit-test-style code (conceptual, not bound to a framework) that verifies: (a) `LoggingDecorator` logs messages, (b) `CachingDecorator` avoids repeated inner calls, (c) `ValidationDecorator` blocks invalid input. Use fakes/spies and assertions in comments.

9) 09-Proxy_RemoteStub.cs  
Problem: Implement a `RemoteServiceProxy` that represents a remote call and caches a connection handle. Provide a `RemoteStub` that simulates remote latency and show how proxy handles reconnection on failure. Include comments about retry/backoff placement.

10) 10-DecoratorVsAOPNotes.cs  
Problem: Write a concise design-note (text) explaining pros/cons of decorator chains vs AOP frameworks for cross-cutting concerns, include a short checklist to decide which approach to use in a .NET project.

Deliverables
- Day6_Exercises.md (this document)
- Ten C# solution files named exactly as the problem titles (01–10).