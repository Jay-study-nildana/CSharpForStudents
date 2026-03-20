# Delegates (C# / .NET)

Purpose
- Explain what delegates are and why they matter.
- Show common delegate forms (custom delegate types, Func/Action/Predicate), lambda expressions, multicast delegates, and practical patterns.
- Give examples students can use in the capstone to implement callbacks and decoupled handlers.

What is a delegate?
- A delegate is a type-safe object that references one or more methods with a specific signature. Think of a delegate as a typed function pointer.
- Delegates enable higher-order programming (passing behavior as data), callbacks, and the event pattern in .NET.

Declaring and using a custom delegate
```csharp
// Declare a delegate type
public delegate int Transformer(int x);

// Use it
public class Demo
{
    public static int Square(int x) => x * x;

    public void Run()
    {
        Transformer t = Square;         // method group conversion
        int result = t(5);              // invoke the delegate -> 25
        Console.WriteLine(result);
    }
}
```

Built-in delegates: Func, Action, Predicate
- Func<TResult> / Func<T, TResult> family: returns a value.
- Action<T...>: returns void.
- Predicate<T>: returns bool (same as Func<T, bool> but more semantic).
```csharp
Func<int,int> square = x => x * x;
Action<string> log = s => Console.WriteLine(s);
Predicate<int> isEven = n => n % 2 == 0;

int r = square(6);     // 36
log("hello");          // prints hello
bool even = isEven(4); // true
```

Anonymous methods and lambda expressions
- Lambdas are the concise way to create delegates inline:
  - `(x) => x * x` — expression-bodied lambda.
  - `(x) => { /* body */ return y; }` — statement-bodied lambda.
- Lambdas frequently used with LINQ and higher-order functions.
```csharp
var numbers = new[] {1,2,3,4};
var squares = numbers.Select(n => n * n);
```

Higher-order functions (passing delegates as parameters)
- A higher-order function accepts a delegate, returning behavior customization without inheritance.
```csharp
public static IEnumerable<T> Filter<T>(IEnumerable<T> source, Predicate<T> predicate)
{
    foreach (var item in source)
        if (predicate(item)) yield return item;
}

// Usage
var evens = Filter(new[] {1,2,3,4,5}, n => n % 2 == 0);
```
- This enables decoupling: algorithms are generic and behavior is supplied.

Multicast delegates and invocation list
- Delegates can reference multiple methods. Invoking a multicast delegate calls each method in order.
- For delegates that return values, only the last return value is seen by the caller.
```csharp
Action onDone = () => Console.WriteLine("Step1");
onDone += () => Console.WriteLine("Step2");
onDone(); // prints Step1 then Step2
```
- Removing handlers: `onDone -= handler;`

Events vs delegates (brief)
- An event is a language construct that wraps a delegate and restricts invocation to the declaring class:
  - `public event EventHandler<EventArgs> SomethingHappened;`
- Use events for publisher/subscriber scenarios to provide encapsulation and prevent external code from replacing the invocation list.

Closures and captured variables
- Lambdas can capture local variables; captured variables live on the heap as part of a closure.
```csharp
List<Func<int>> makers = new();
for (int i = 0; i < 3; i++)
{
    int capture = i;                // capture a fresh variable per iteration
    makers.Add(() => capture * 10);
}
Console.WriteLine(makers[1]());     // 10
```
- Avoid surprising behavior by capturing loop variables directly — prefer copying into a local `capture` as shown.

Thread-safety and invocation best practices
- Delegate invocation can raise `NullReferenceException` if `null`. Use null-conditional operator:
  - `handler?.Invoke(this, args);`
- For multithreaded safety, copy the delegate reference first:
```csharp
var snapshot = MyEvent;
snapshot?.Invoke(this, args); // snapshot prevents race between null-check and invocation
```
- Events themselves are thread-safe for subscription/unsubscription but invocation needs care.

Common pitfalls
- Relying on multicast delegates’ return values (only last result counts).
- Excessive capturing creating many allocations (closures). Lambdas that capture nothing are allocation-free in many cases.
- Using service locators inside delegates — prefer well-scoped factories or pass dependencies explicitly.

Practical patterns for your capstone
- Callbacks for asynchronous work: accept a `Func<Task>` or `Action` to run after an operation.
- Strategy injection: pass a `Func<TInput,TResult>` for pluggable business rules (e.g., validation or pricing).
- Event-driven notifications: expose an `event EventHandler<OrderEventArgs> OrderPlaced` so other components subscribe without tight coupling.
- Middleware-style pipeline: `Func<RequestContext, Task>` chain to build request processing pipelines.

Example — publisher/subscriber for notifications
```csharp
public class OrderPlacedEventArgs : EventArgs { public Guid OrderId { get; set; } }

public class OrderService
{
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    protected virtual void OnOrderPlaced(Guid orderId)
        => OrderPlaced?.Invoke(this, new OrderPlacedEventArgs { OrderId = orderId });

    public void PlaceOrder(Guid orderId)
    {
        // place order...
        OnOrderPlaced(orderId); // notify subscribers
    }
}
```
- Subscribers (email, audit, analytics) can register handlers without OrderService knowing them.

Homework (short)
1. Refactor one capstone class that currently calls a helper via `new` to accept a `Func<T>` or `Action` callback instead. Explain the testability gain in 2–3 lines.
2. Design two events for your capstone (e.g., `OrderPlaced`, `UserRegistered`) and list who will subscribe and why.

Summary
- Delegates are the foundation for callbacks, higher-order functions, and events in C#. Use Func/Action for common needs, lambdas for concise handlers, and events to implement publisher/subscriber patterns.
- Be mindful of closures, multicast semantics, and thread-safety. Prefer explicit dependency passing and small, focused delegates to keep code decoupled and testable.

Further reading
- C# language specification: delegates and events
- Microsoft docs: Delegates, Events, and Lambda expressions
- Patterns: Event aggregator, Strategy, Middleware (pipeline)