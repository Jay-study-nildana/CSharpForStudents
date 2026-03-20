# Day 19 — Delegates, Events & Lambdas: Problem Set (10 exercises)

Instructions
- Solve each problem using C# to demonstrate the requested concept. Provide short, focused implementations that compile.
- Each solution file should be named exactly as the problem title plus `.cs` (the example solution files are provided below).
- For each problem include a tiny demonstration (console output, simple assertions, or comments showing expected behaviour).
- Submit code and a 1–3 line explanation for each solution describing why the approach is correct and how it improves decoupling/testability.

Problems

1) SimpleDelegateDeclaration
- Declare a custom delegate type that matches methods taking an int and returning int.
- Provide a method that squares a number and show assigning the method to the delegate and invoking it.

2) FuncActionPredicateUsage
- Demonstrate usage of `Func`, `Action`, and `Predicate` in place of custom delegates.
- Provide short examples showing mapping, side-effect action, and predicate filtering.

3) LambdaLINQFilter
- Use lambda expressions with LINQ to filter and transform a collection.
- Show a small example (e.g., filter even numbers and multiply by 10).

4) ClosureLoopCaptureBug
- Show the classic loop-variable capture bug when creating lambdas in a loop.
- Provide a corrected version that avoids the bug and explain why it works.

5) MulticastDelegateOrder
- Demonstrate a multicast delegate (Action) with multiple handlers.
- Show invocation order and how to remove a handler.

6) EventPatternRefactor
- Given a class that calls subscribers directly, refactor it to use the standard .NET event pattern:
  - `EventHandler<TEventArgs>` with a protected `OnX` method and safe invocation.
- Demonstrate subscribing and unsubscribing.

7) AsyncEventHandlers
- Implement an async event pattern using `delegate Task AsyncEventHandler<TEventArgs>(object?, TEventArgs)`.
- Show how to raise the async event and await subscribers in parallel and sequentially.

8) WeakEventPattern
- Implement a simple weak-event helper so subscribers using instance methods do not prevent GC of short-lived subscribers.
- Demonstrate subscribing, dropping the strong reference, forcing GC, and verifying the handler no longer runs.

9) EventAggregatorSimple
- Implement a minimal in-process `EventAggregator` with `Subscribe<T>`, `Unsubscribe<T>`, and `Publish<T>`.
- Show publishing an event and multiple subscribers reacting.

10) DelegateBasedStrategyInjection
- Implement a service that accepts a `Func<Order, decimal>` price-calculator delegate to compute final price; show two different strategies (e.g., standard and discount).
- Demonstrate swapping strategies without changing the service implementation.

Deliverables
- `Day19-Problems.md` (this file)
- 10 solution files:
  - SimpleDelegateDeclaration.cs
  - FuncActionPredicateUsage.cs
  - LambdaLINQFilter.cs
  - ClosureLoopCaptureBug.cs
  - MulticastDelegateOrder.cs
  - EventPatternRefactor.cs
  - AsyncEventHandlers.cs
  - WeakEventPattern.cs
  - EventAggregatorSimple.cs
  - DelegateBasedStrategyInjection.cs

Good luck — implement and test each example, and add a short explanation per problem.