# Lambda Expressions (C# / .NET)

Purpose
- Explain what lambda expressions are and why they matter.
- Show syntax, common uses (LINQ, callbacks, higher-order functions), captures/closures, async lambdas, expression trees, and performance considerations.
- Provide concise C# examples students can use in their capstone.

What is a lambda expression?
- A lambda expression is an anonymous function you can use to create delegates or expression tree types. It is a concise way to pass behavior (a function) as data.
- Lambdas enable higher-order programming: pass functions into methods, return functions, or build pipelines without creating named classes.

Basic syntax
- Expression-bodied lambda:
```csharp
Func<int,int> square = x => x * x;
int r = square(5); // 25
```
- Statement-bodied lambda (multiple statements):
```csharp
Action<string> greet = name =>
{
    var now = DateTime.UtcNow;
    Console.WriteLine($"Hello {name} at {now:O}");
};
greet("Alice");
```
- Parameterless:
```csharp
Func<DateTime> now = () => DateTime.UtcNow;
```

Common built-in delegate types
- Func<T1,...,TResult> — returns a value.
- Action<T...> — returns void.
- Predicate<T> — returns bool (semantic alias for Func<T,bool>).

Lambdas with LINQ
- Lambdas are the idiomatic way to express queries and transforms.
```csharp
var numbers = new[] {1,2,3,4};
var evens = numbers.Where(n => n % 2 == 0).Select(n => n * 10).ToList();
// evens == [20, 40]
```

Expression trees vs delegate lambdas
- Expression trees (`Expression<Func<T,bool>>`) represent code as data and are used by LINQ providers (e.g., EF Core) to translate expressions to SQL.
```csharp
Expression<Func<User,bool>> predicate = u => u.IsActive && u.Age > 18;
// EF Core can translate `predicate` into SQL
```
- Use `Func<T,bool>` when you want executable code; use `Expression<Func<T,bool>>` when the consumer needs to inspect or translate the expression.

Captures and closures
- Lambdas can capture variables from the enclosing scope. The compiler lifts captured variables into a closure object.
```csharp
Func<int> makeCounter()
{
    int count = 0;
    return () => ++count; // captures `count`
}
var c = makeCounter();
Console.WriteLine(c()); // 1
Console.WriteLine(c()); // 2
```
- Be careful with loop variable capture (a common pitfall):
```csharp
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    // BAD: `i` is captured and will be 3 for all actions in older C# versions if not copied
    actions.Add(() => Console.WriteLine(i));
}
```
Fix by copying to a local:
```csharp
for (int i = 0; i < 3; i++)
{
    int copy = i;
    actions.Add(() => Console.WriteLine(copy));
}
```

Async lambdas
- Lambdas can be async and return Task/Task<T>:
```csharp
Func<Task> job = async () =>
{
    await Task.Delay(100);
    Console.WriteLine("Done");
};
await job();
```
- For event handlers that perform I/O prefer Task-based patterns (e.g., `Func<T,Task>` or `AsyncEventHandler<T>`).

Multicast and delegates
- Assign a lambda to an `Action` and add more handlers:
```csharp
Action onDone = () => Console.WriteLine("A");
onDone += () => Console.WriteLine("B");
onDone(); // prints A then B
```
- For delegates that return values, only the last return value is available to the caller; avoid relying on multicast return semantics.

Static lambdas and allocation avoidance
- Capturing lambdas allocate closure objects. If a lambda does not capture outer variables, it can be allocation-free in many cases.
- Recent C# supports `static` lambdas to guarantee no capture and avoid accidental closure allocations:
```csharp
Func<int,int> timesTwo = static x => x * 2;
```
- Use `static` when the lambda does not depend on instance/state to make intent explicit and improve performance.

When to use lambdas
- Event handlers and callbacks (small, local behavior).
- LINQ filtering and projection.
- Passing strategies or rules (e.g., `Func<Order,decimal> priceCalculator`).
- Building middleware/pipeline delegates (e.g., RequestDelegate).
- Registering small factory delegates in DI: `services.AddSingleton<Func<IRepo>>(sp => () => new Repo())`.

Best practices and readability
- Keep lambdas short and focused. If logic grows, extract a named method for readability and testability.
- Prefer expression-bodied lambdas for simple transforms; use statement-bodied for multiple steps.
- Name widely reused delegates or extract a small helper type instead of large ad-hoc lambdas spread across the codebase.

Performance considerations
- Captured variables produce closure objects — avoid capturing in hot paths.
- Repeated creation of identical lambdas can allocate; reuse static delegates when possible.
- Expression trees incur overhead; use them only when translation/inspection is required (ORMs, dynamic filters).

Common pitfalls
- Unexpected capture of loop variables — fix with local copy.
- Overusing lambdas for large behavioral logic — makes debugging and stack traces harder.
- Forgetting null checks when using multicast delegates/events — use snapshot invocation:
```csharp
var snapshot = MyEvent;
snapshot?.Invoke(this, args);
```

Capstone examples (practical)
- Validation rule injection:
```csharp
Func<Order,bool> rule = o => o.Total > 0 && o.Items.Any();
if (!rule(order)) throw new ValidationException();
```
- Retry policy with lambda (Polly example):
```csharp
var policy = Policy.Handle<Exception>()
                   .RetryAsync(3, onRetry: (ex, i) => logger.LogWarning("Retry {Attempt}", i));
await policy.ExecuteAsync(() => httpClient.GetAsync(uri));
```
- Mapping DTOs:
```csharp
Func<OrderEntity,OrderDto> map = e => new OrderDto(e.Id, e.Total, e.CreatedAt);
var dtos = entities.Select(map).ToList();
```

Homework (short)
1. Replace a small named method in your capstone with an equivalent lambda where it is used only once (e.g., a single small mapping function) and explain why the lambda improves locality/readability.
2. Find a `for` loop that registers callbacks in your codebase and demonstrate the loop-variable capture bug (if present), then fix it with a local copy. Submit before/after snippets and a 2–3 line explanation.

Summary
- Lambdas are concise anonymous functions that make code more expressive and enable functional styles in C#. They are central to LINQ, callbacks, and higher-order APIs.
- Be mindful of captures/allocations and prefer `static` lambdas when no capture is needed. Keep lambdas short and extract named methods for complex logic.

Further reading
- Microsoft docs: Lambda expressions
- C# language spec: anonymous functions and closures
- Articles on performance implications of captured variables and static lambdas
```*
