# Delegates, Anonymous Methods, and Delegate Enhancements in C#  
Interview Reference Guide for Developers

---

Note : Code samples available at the bottom of this page.

## Table of Contents

1. [Overview & Motivation](#overview--motivation)  
2. [Quick Version Timeline & Relevance](#quick-version-timeline--relevance)  
3. [Delegate Fundamentals](#delegate-fundamentals)  
   - What is a delegate?  
   - Delegate type and runtime representation  
4. [Declaring and Instantiating Delegates](#declaring-and-instantiating-delegates)  
   - Method group conversion, new operator  
   - Built-in generic delegates (Action, Func, Predicate, Comparison)  
5. [Single-cast Delegates](#single-cast-delegates)  
   - Invocation and null-checking  
   - Equality and identity semantics  
   - Use cases  
6. [Multicast Delegates](#multicast-delegates)  
   - Invocation list, ordering, and semantics  
   - Adding and removing handlers (`+=`, `-=`)  
   - Return values and exception behavior  
7. [Anonymous Methods](#anonymous-methods)  
   - Syntax and examples (delegate keyword)  
   - Comparison to lambdas  
   - Closures and captured variables  
   - Common pitfalls (captured loop variables)  
8. [Lambda Expressions and Local Functions (Related)](#lambda-expressions-and-local-functions-related)  
   - Lambdas vs anonymous methods vs local functions  
   - Expression trees: `Expression<TDelegate>`  
9. [Delegate Enhancements & Modern Features](#delegate-enhancements--modern-features)  
   - Built-in generic delegates (Action/Func) and Predicate/Comparison  
   - Covariance & contravariance for delegates  
   - Static lambdas (no capture) and performance benefits  
   - Function pointers (`delegate*<...>`) — unmanaged function pointers  
   - Target-typed new, enhanced method group conversions  
   - `Delegate.CreateDelegate`, `MethodInfo` bindings  
10. [Events and the Event Pattern (using delegates)](#events-and-the-event-pattern-using-delegates)  
    - `event` keyword semantics, raise pattern, custom add/remove  
    - Weak event patterns and memory leak considerations  
11. [Advanced Topics & Interop](#advanced-topics--interop)  
    - Multicast with asynchronous handlers and Task-returning delegates  
    - DynamicInvoke and reflection-based invocation costs  
    - Serialization, security, and AppDomain concerns  
12. [Performance Considerations](#performance-considerations)  
13. [Best Practices & API Design Guidelines](#best-practices--api-design-guidelines)  
14. [Common Mistakes & Anti-Patterns](#common-mistakes--anti-patterns)  
15. [Testing & Debugging Tips](#testing--debugging-tips)  
16. [Comprehensive Q&A — Developer & Interview Questions (with answers)](#comprehensive-qa--developer--interview-questions-with-answers)  
17. [Practical Exercises & Projects](#practical-exercises--projects)  
18. [References & Further Reading](#references--further-reading)

---

## 1. Overview & Motivation

Delegates are a foundational feature in C#. They are type-safe references to methods — enabling callbacks, event handlers, functional-style programming, and extensibility points. Multicast delegates (the core of C# events) let you route a single invocation to multiple subscribers. Anonymous methods and lambdas make it easy to create inline delegate implementations. Over the evolution of C#, the delegate model has been enhanced with generic delegates, variance, expression trees, static lambdas, and even function pointers for interop and performance-critical scenarios.

This guide covers theory, practical examples, pitfalls, and interview-style Q&A to prepare developers to design, implement, and reason about delegate-based APIs.

---

## 2. Quick Version Timeline & Relevance

- C# 1.0: Delegates introduced (type-safe function pointers).
- C# 2.0: Anonymous methods (`delegate` keyword) and generics (allow generic delegates).
- C# 3.0: Lambda expressions (concise syntax) and LINQ; expression trees (`Expression<TDelegate>`).
- C# 4.0+: Covariance/contravariance support for delegates and generic interfaces expanded.
- C# 9.0+: Function pointers (`delegate*<...>`), static lambdas (reduce closure allocations) — modern performance/interop features.
- Later versions added refined language support (target-typed new, improved inference).

Note: Feature availability depends on compiler and target framework. Always confirm the C# language version in your project.

---

## 3. Delegate Fundamentals

What is a delegate?
- A delegate is a reference type that encapsulates a method (a method pointer with a signature).
- Declared with the `delegate` keyword:
  ```csharp
  public delegate void MyHandler(string message);
  ```
- Delegates ensure the method signature (parameter types, return type) matches before assignment.

Runtime representation:
- Delegates derive from `System.Delegate` / `System.MulticastDelegate`.
- They are immutable — combining/removing returns new delegate instances (immutable functional values).
- A delegate instance stores the target object (for instance methods) and method pointer.

Why use delegates?
- Callback pattern
- Event subscriptions
- Higher-order functions (passing behavior)
- Functional composition

---

## 4. Declaring and Instantiating Delegates

Declaring:
```csharp
public delegate int Transformer(int x);
```

Instantiating:
- Method group conversion (implicit):
  ```csharp
  int Square(int x) => x * x;
  Transformer t = Square; // method group conversion
  ```
- Explicit `new`:
  ```csharp
  Transformer t2 = new Transformer(Square);
  ```
- Anonymous methods (delegate keyword) and lambdas (see later).

Built-in generic delegates:
- Action: `Action`, `Action<T>`, `Action<T1,T2>`, ... — returns void.
- Func: `Func<TResult>`, `Func<T, TResult>`, ... — last type parameter is return type.
- Predicate<T>: `Predicate<T>` equivalent to `Func<T, bool>`.
- Comparison<T>: `Comparison<T>` used by `List<T>.Sort`.

Prefer built-in delegates for new code instead of defining many small delegate types, unless the delegate is part of a public API where a named delegate improves readability.

Example:
```csharp
Func<int,int> increment = x => x + 1;
Action<string> logger = s => Console.WriteLine(s);
```

---

## 5. Single-cast Delegates

Single-cast delegates refer to exactly one method.

Invocation:
```csharp
Transformer t = x => x + 1;
int result = t(5);      // direct invoke
int result2 = t.Invoke(5); // explicit Invoke
```

Null checks:
- Invoking a null delegate throws `NullReferenceException`.
- Use safe invocation:
  ```csharp
  t?.Invoke(5);
  ```
  or copy-then-check pattern to avoid race conditions:
  ```csharp
  var handler = myDelegate;
  handler?.Invoke(args);
  ```

Equality semantics:
- Delegates are equal when they are the same delegate type and have identical invocation lists (same target and same method).
- For instance methods the target reference must be equal (same object instance); for static methods target is `null`.

Use cases:
- Single callback registration (e.g., provide a processing function).
- Passing behavior to algorithms (Strategy pattern via delegates).

---

## 6. Multicast Delegates

Multicast delegates hold an ordered invocation list of delegates. They are used to implement event subscription.

Add/remove handlers:
```csharp
MyHandler handlers = null;
handlers += HandlerOne;
handlers += HandlerTwo;
handlers -= HandlerOne;
```

Invocation order:
- Handlers are invoked in the order added (first in, first invoked).

Return values:
- If delegate has non-void return type, invoking a multicast delegate returns the result from the last handler in the invocation list.
- To get every return value:
  ```csharp
  var list = multicast.GetInvocationList();
  foreach (MyHandler handler in list)
  {
      var ret = ((Func<int,int>)handler)(arg); // cast per delegate type
      // collect ret
  }
  ```

Exception behavior:
- If any handler throws, invocation stops and exception propagates. To protect the rest of the handlers, iterate `GetInvocationList()` and wrap individual invokes in try/catch:
  ```csharp
  foreach (MyHandler del in handlers.GetInvocationList())
  {
      try { ((MyHandler)del)("msg"); }
      catch (Exception ex) { Log(ex); }
  }
  ```

Multicast and events:
- Multicast delegates enable one-to-many notifications: publishers fire an event, subscribers observe.

Thread-safety:
- `+=` and `-=` are not atomic; they are compiled to Combine/Remove + assignment operations. Use `Interlocked.CompareExchange` in custom add/remove accessors for atomicity in high-concurrency scenarios.

Example: safe raise copy pattern
```csharp
var handler = Changed;
if (handler != null) handler(this, EventArgs.Empty);
```

---

## 7. Anonymous Methods

Anonymous methods were introduced to allow inline delegate implementations without declaring a named method.

Syntax (anonymous method with `delegate` keyword):
```csharp
MyHandler h = delegate(string msg) {
    Console.WriteLine(msg);
};
```

Anonymous methods vs lambdas:
- Anonymous methods (`delegate`) predate lambdas and look more verbose.
- Lambdas (`x => x + 1` or `(x,y) => { ... }`) are preferred today; anonymous methods remain supported for backward compatibility.

Capturing variables (closures):
- Anonymous methods can capture local variables from the declaring scope:
  ```csharp
  int factor = 2;
  Func<int,int> mult = delegate(int x) { return x * factor; }; // captures factor
  factor = 5; // captured variable is shared
  Console.WriteLine(mult(3)); // prints 15, not 6
  ```
- Captured variables are hoisted into a compiler-generated closure class. The closure lifetime extends beyond method if the delegate escapes (e.g., stored in a field or returned).

Common pitfall — loop variable capture:
```csharp
List<Action> actions = new();
for (int i = 0; i < 3; i++)
{
    actions.Add(delegate { Console.WriteLine(i); }); // captures i, same variable
}
foreach (var a in actions) a(); // prints 3,3,3 in older naive code
```
Fix approaches:
- Capture a copy inside the loop:
  ```csharp
  for (int i = 0; i < 3; i++)
  {
      int copy = i;
      actions.Add(() => Console.WriteLine(copy));
  }
  ```
- In modern C# (C# 5+), the `foreach` loop variable capture semantics were changed to behave as expected for `foreach`, but caution still advised for `for` loops.

Anonymous method with statement block:
```csharp
Func<int,int> f = delegate(int x) {
    var r = x * 2;
    return r + 1;
};
```

Use cases:
- Short inline callbacks
- Event handlers in small scopes (though lambdas are more common)
- When you need a multiline body and prefer `delegate` syntax (rare)

---

## 8. Lambda Expressions and Local Functions (Related)

Lambda expressions:
- More concise and flexible; support expression-body and statement-body forms.
  ```csharp
  Func<int,int> inc = x => x + 1; // expression lambda
  Func<int,int> square = x => { var r = x*x; return r; }; // statement lambda
  ```
- Lambdas can be converted to delegates or expression trees (`Expression<TDelegate>`).

Expression trees:
- `Expression<Func<T, TResult>>` captures the Lambda as an expression structure instead of compiled code.
- Used by LINQ providers (Entity Framework) to translate code to SQL or other query languages.
- Example:
  ```csharp
  Expression<Func<Person, bool>> pred = p => p.Age > 30;
  ```

Local functions:
- Declared inside methods using `void Local() { }` or `int Local(int x) => x;`.
- Local functions can be used instead of delegates for encapsulation and may be converted to delegates when passed as a delegate target.
- Local functions can be `static` (no capture) to prevent closure allocation.

Comparisons:
- Anonymous methods: older, explicit `delegate` keyword.
- Lambdas: preferred for clarity and brevity.
- Local functions: good for named, reusable local logic; avoid delegate overhead if not needed.

---

## 9. Delegate Enhancements & Modern Features

This section covers language and runtime features introduced since C#'s early versions that enhance delegate ergonomics and performance.

Built-in generic delegates:
- Action, Func, Predicate, Comparison reduce the need to declare custom delegate types for simple callbacks.
- Example:
  ```csharp
  Func<int,int,int> add = (a,b) => a + b;
  Action<string> log = s => Console.WriteLine(s);
  ```

Covariance & Contravariance:
- Delegates support variance: `out` for return types (covariant) and `in` for parameter types (contravariant) where suitable.
- Example covariance:
  ```csharp
  Func<string> fs = () => "hello";
  Func<object> fo = fs; // allowed because string : object (covariant)
  ```
- Example contravariance:
  ```csharp
  Action<object> ao = o => Console.WriteLine(o);
  Action<string> asg = ao; // allowed: accepts string because object parameter can receive string
  ```

Static lambdas (no capture):
- Declared with the `static` modifier on lambda to signal no captures and allow additional compiler/runtime optimizations:
  ```csharp
  Func<int,int> f = static x => x + 1;
  ```
- Static lambdas cannot access local variables or `this`. They avoid closure allocations and can be faster.

Function pointers (unsafe / interop):
- `delegate*<T1,T2,...,TReturn>` is a low-level unmanaged function pointer syntax for high-performance interop (C# 9+).
- Example:
  ```csharp
  unsafe delegate*<int,int> ptr = &SomeUnmanagedFunction;
  int r = ptr(5);
  ```
- Use with caution: unsafe context, no runtime safety, appropriate for P/Invoke scenarios or C-style callbacks.

Target-typed new and delegate creation:
- Target-typed `new` helps in delegate creation and collection of delegates of specific typed targets:
  ```csharp
  List<Func<int,int>> list = new() { x => x + 1, x => x * 2 };
  ```

Delegate binding via reflection:
- `Delegate.CreateDelegate` allows constructing a delegate from `MethodInfo` and a target object — useful for dynamic frameworks, plugin loaders, or when method info is discovered at runtime.
  ```csharp
  var mi = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) });
  var d = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), mi);
  d("hello");
  ```

Expression trees and `Compile()`:
- Create expression trees with `Expression<TDelegate>` and compile them at runtime to delegates:
  ```csharp
  var expr = Expression.Lambda<Func<int,int>>(Expression.Add(param, constant), param);
  var func = expr.Compile();
  ```

Delegate Equality & Immutability:
- Since delegates are immutable, `+=`/`-=` produces new instances; comparing delegates compares invocation lists.
- Removing a handler removes the *last matching* occurrence.

---

## 10. Events and the Event Pattern (using delegates)

Use `event` to expose multicast delegate subscription safely:
```csharp
public event EventHandler<MyArgs> Changed;
protected virtual void OnChanged(MyArgs e) => Changed?.Invoke(this, e);
```

Why use `event` instead of public delegate?
- `event` restricts direct invocation and assignment by external code — external code can only `+=` or `-=` but cannot set or invoke the event.
- Protects encapsulation and prevents external callers from clearing or overwriting subscribers.

Custom add/remove:
- You can implement custom add/remove to control subscription logic or use `Interlocked.CompareExchange` to make add/remove atomic.

Weak events and memory leaks:
- Subscribers registered to long-lived publishers (especially static events) can be prevented from garbage collection if they don't unsubscribe.
- Solutions:
  - Strong discipline to `-=` in `Dispose()`.
  - Use `WeakReference` or weak event patterns (WeakEventManager, or custom weak subscription implementations) to avoid leaks.
  - Prefer instance-level publishers or use centralized event aggregator that keys by weak references.

Event invocation patterns:
- Use `?.Invoke()` with a local copy to avoid race conditions.
- Prefer `protected virtual OnX` method to allow derived classes to override raising semantics.

---

## 11. Advanced Topics & Interop

Asynchronous handlers:
- You may want to support `Func<Task>` or `Func<object, Task>` as event handler types for async subscribers.
- For multicast async invocation, you can run handlers concurrently:
  ```csharp
  var handlers = Changed?.GetInvocationList();
  var tasks = handlers.Select(h => ((Func<object,Task>)h)(args));
  await Task.WhenAll(tasks);
  ```
- Collect exceptions into `AggregateException` or flatten into `AggregateException` as needed.

Delegate.DynamicInvoke:
- Allows invoking with `object[]` arguments: `delegate.DynamicInvoke(args)`.
- Slower and uses reflection/boxing. Prefer direct invoke or strongly-typed call in performance-critical code.

Interop & P/Invoke:
- Passing delegates to native code as callbacks requires delegates to be pinned (prevent GC move) or marshaled appropriately.
- Example:
  ```csharp
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void NativeCallback(int x);
  [DllImport("lib.dll")]
  public static extern void RegisterCallback(NativeCallback cb);
  ```

Serialization:
- Serializing delegates is brittle because the target method/instance must exist with matching assembly types at deserialization. Usually avoid serializing delegates; persist configuration like method names and create delegates via reflection at runtime.

Security:
- Be cautious when invoking delegates that run untrusted code; consider security boundaries and sandboxing.

---

## 12. Performance Considerations

- Delegate invocation is fast — comparable to virtual calls — but not free.
- Closure allocations: capturing variables creates allocation for closure state. Use `static` lambdas or avoid captures for hot paths.
- `GetInvocationList()` allocates an array of delegates; avoid calling unnecessarily in hot loops.
- `DynamicInvoke` and reflection are orders of magnitude slower; avoid in performance-sensitive code.
- Function pointers (`delegate*`) can be faster for unmanaged interop or hot native-call loops but require `unsafe` and careful management.

Practical tips:
- Use `Action`/`Func` and avoid unnecessary delegate wrappers.
- For repeated invocation with no capture, cache the delegate instance instead of creating new lambdas repeatedly.
- Prefer `static` lambdas when the body does not need captures.

---

## 13. Best Practices & API Design Guidelines

- Use `event` for publish/subscribe and encapsulate invocations via `OnX` methods.
- Prefer `EventHandler` / `EventHandler<TEventArgs>` for standardization and tooling compatibility.
- Prefer built-in generic delegates (`Action`, `Func`) for concise API when no semantic name is necessary. Use named delegate types when the delegate itself is an important part of the API contract.
- Avoid exposing `Delegate` or `MulticastDelegate` types raw — use typed delegates or events.
- Document whether event handlers should be synchronous or async. If async, use Task-returning handlers and document invocation semantics (concurrent vs sequential).
- Avoid long-running work on event handlers; consider offloading to background tasks.
- Ensure subscribers unsubscribe (provide Unsubscribe or `Dispose` patterns) or use weak subscriptions.
- Make add/remove thread-safe for events expected to be used concurrently.
- Use local-copy `var handler = Changed; handler?.Invoke(this, e);` to avoid null-check race conditions.

---

## 14. Common Mistakes & Anti-Patterns

- Exposing delegate fields publicly (instead of event) — breaks encapsulation.
- Forgetting to unsubscribe from events (memory leaks).
- Assuming multicast delegate returns aggregate result (only last result is returned).
- Letting an exception in one handler stop remaining handlers without protection.
- Capturing mutable loop variables incorrectly in closures.
- Relying on delegate equality semantics for identity without understanding instance method targets.
- Using `DynamicInvoke` or reflection where static invocation is possible.
- Serializing delegates and expecting portability.

---

## 15. Testing & Debugging Tips

- Inspect `delegate.Target` and `delegate.Method` in debugger to understand what a delegate points to.
- Use `GetInvocationList()` to assert number/order of subscribers in unit tests:
  ```csharp
  var list = myEventField.GetInvocationList();
  Assert.Equal(2, list.Length);
  ```
- Test that event subscribers are invoked and that unsubscribing works.
- For closures, write tests that assert the captured value semantics (e.g., ensure each captured copy behaves as expected).
- To reproduce concurrency/event ordering issues, use targeted unit tests that subscribe/unsubscribe under load (use `Task.Run` and synchronization).
- For async event patterns, assert that exceptions are collected/handled properly.

---

## 16. Comprehensive Q&A — Developer & Interview Questions (with answers)

Q1: What is a delegate in C#?  
A: A delegate is a type-safe method reference (function pointer) that encapsulates a method with a specific signature. Instances can be invoked like methods.

Q2: Difference between single-cast and multicast delegates?  
A: Single-cast delegates reference a single method. Multicast delegates maintain an invocation list (multiple methods) and invoking them calls every method in order.

Q3: How do you safely raise events?  
A: Copy the event to a local variable then call `?.Invoke`, e.g., `var handler = Changed; handler?.Invoke(this, e);` to avoid race conditions. Consider wrapping each handler call in try/catch to prevent a faulty handler from stopping the rest.

Q4: What is an anonymous method and how does it compare to a lambda?  
A: Anonymous methods use the `delegate` keyword to define an inline unnamed method. Lambdas are a more concise syntax for the same concept; lambdas can produce either delegates or expression trees.

Q5: What is a closure?  
A: A closure is a delegate that captures variables from its enclosing scope. The captured variables are hoisted into a generated object, and their lifetime extends when the delegate outlives the scope.

Q6: Explain delegate covariance and contravariance.  
A: Covariance allows assigning a delegate that returns a more derived type to a delegate expecting a less derived return type. Contravariance allows assigning a delegate that accepts a less derived parameter to a delegate expecting a more derived parameter.

Q7: Why avoid `public Action` fields?  
A: Public delegate fields allow external code to overwrite or invoke the delegate directly. Using `event` restricts clients to `+=`/`-=` only, preserving encapsulation and preventing accidental invocation or clearing.

Q8: How can a multicast delegate return multiple values?  
A: Multicast delegates only return the last handler's return value. To collect all values, enumerate `GetInvocationList()` and invoke each delegate individually, collecting results.

Q9: What are static lambdas and when use them?  
A: `static` lambdas are lambdas marked `static` to indicate they do not capture enclosing scope. They prevent closure allocation and can be more performant. Use them when no captures are needed.

Q10: How to pass a C# delegate to native code?  
A: Declare the delegate with `[UnmanagedFunctionPointer]` if necessary, create an instance, and pass it to native API via P/Invoke. Ensure the delegate remains rooted (won't be garbage collected) for the native side — typically by storing it in a field.

Q11: What happens if one handler in a multicast delegate throws?  
A: The exception propagates and remaining handlers will not be invoked unless you manually iterate and handle exceptions for each invocation.

Q12: How do you create a delegate instance for a method discovered at runtime?  
A: Use `Delegate.CreateDelegate(delegateType, target, methodInfo)` or `methodInfo.CreateDelegate(typeof(Action), target)` to create a delegate bound to the method.

Q13: Should you use delegates or interfaces for callbacks?  
A: Use delegates for simple callbacks and functional-style programming. Prefer interfaces when you need richer contracts, state, or multiple related callback methods.

Q14: What is `DynamicInvoke` and why avoid it?  
A: `DynamicInvoke` invokes a delegate with parameters provided as `object[]`. It uses reflection, is slower, and loses compile-time type safety. Prefer typed invocation.

Q15: How do you avoid memory leaks with events?  
A: Unsubscribe event handlers when no longer needed, use weak subscriptions for long-lived publishers, avoid static events holding references to short-lived subscribers, or implement `IDisposable` to remove subscriptions during disposal.

---

## 17. Practical Exercises & Projects

1. Basic:
   - Create a `Logger` delegate and implement a logging system that accepts `Action<string>` handlers. Add console/file handlers and demonstrate subscribing/unsubscribing.
2. Intermediate:
   - Build a small event-based `DownloadManager` that exposes `event EventHandler<DownloadProgressEventArgs> ProgressChanged`. Subscribe multiple listeners, ensure thread-safety, and test exception-isolation (one listener throwing doesn't prevent others).
   - Demonstrate and fix the loop-capture closure pitfall by writing a test that shows incorrect behaviour and the corrected version.
3. Advanced:
   - Implement a plugin loader that discovers methods marked with a custom attribute and uses `Delegate.CreateDelegate` to register them as handlers for particular events.
   - Build a high-performance native interop scenario using function pointers (`delegate*`) to call unmanaged callbacks from C# in a tight loop; compare performance vs using delegates to call `DllImport` functions.
   - Create an async event dispatcher that supports both synchronous `EventHandler` and `Func<Task>` subscribers, invoking subscribers concurrently and collecting exceptions.
4. Testing:
   - Use unit tests to validate `GetInvocationList()` ordering and behaviour, unsubscribing, and exception handling patterns.
   - Measure closure allocation overhead with `BenchmarkDotNet` comparing capturing vs non-capturing lambdas and static lambdas.

---

## 18. References & Further Reading

- Microsoft Docs: Delegates (C# Programming Guide) — https://learn.microsoft.com/dotnet/csharp/programming-guide/delegates/
- Microsoft Docs: Events (C# Programming Guide) — https://learn.microsoft.com/dotnet/csharp/programming-guide/events/
- Microsoft Docs: Lambda expressions — https://learn.microsoft.com/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions
- Microsoft Docs: Expression Trees — https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/expression-trees/
- "CLR via C#" by Jeffrey Richter — deep-dive into runtime behaviors and delegates
- Blog posts by Eric Lippert and Joe Duffy on delegates, closures, and event patterns
- BenchmarkDotNet — microbenchmarking closures and delegate invocation overheads
- .NET runtime source (CoreCLR) for delegate implementation details (GitHub)

---

Prepared as a comprehensive reference for developers and interview preparation on delegates, anonymous methods, and modern delegate-related enhancements in C#. Use this guide for studying, API design reviews, or code-review checklists.

---

Code Samples are available here. Simply copy paste the code block verbatim to a new console project.

```


using System;

class Program
{
    // Custom delegate
    delegate int BinaryOperation(int x, int y);

    // Methods matching the delegate signature
    static int Add(int a, int b) => a + b;
    static int Multiply(int a, int b) => a * b;

    static void Main()
    {
        Console.WriteLine("Delegate examples (C# 12 / .NET 8):");

        // Create and invoke a custom delegate
        BinaryOperation op = Add;
        Console.WriteLine($"Add via delegate: {op(3, 4)}"); // 7

        op = Multiply;
        Console.WriteLine($"Multiply via delegate: {op(3, 4)}"); // 12

        // Built-in delegate types: Func and Action
        Func<int, int, int> funcMultiply = (a, b) => a * b;
        Console.WriteLine($"Func multiply: {funcMultiply(5, 6)}"); // 30

        Action<string> greeter = name => Console.WriteLine($"Hello, {name}!");
        greeter("Alice");
    }
}

```
```

using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Built-in generic delegate examples (Func, Action, Predicate, Comparison, Converter)\n");

        // Func<T, TResult> examples
        Func<int, int> square = x => x * x;
        Console.WriteLine($"square(5) = {square(5)}");

        Func<int, int, int> add = (a, b) => a + b;
        Console.WriteLine($"add(3, 4) = {add(3, 4)}");

        Func<int, string> intToString = i => $"Number {i}";
        Console.WriteLine(intToString(7));

        // Action<T> and Action examples (return void)
        Action<string> print = s => Console.WriteLine($"Print: {s}");
        print("Hello from Action");

        Action<int, int> printSum = (a, b) => Console.WriteLine($"{a} + {b} = {a + b}");
        printSum(10, 20);

        // Multicast Action (invokes both targets)
        Action multicast = () => Console.WriteLine("First");
        multicast += () => Console.WriteLine("Second");
        Console.WriteLine("\nMulticast Action output:");
        multicast();

        // Predicate<T> (returns bool)
        Predicate<int> isEven = x => x % 2 == 0;
        Console.WriteLine($"\nisEven(6) = {isEven(6)}");
        Console.WriteLine($"isEven(7) = {isEven(7)}");

        // Comparison<T> used by sorting APIs
        string[] names = { "Alice", "Bob", "Charlie", "Zoë" };
        Comparison<string> lengthComparison = (a, b) => a.Length - b.Length;
        Array.Sort(names, lengthComparison);
        Console.WriteLine("\nSorted by length:");
        foreach (var n in names) Console.WriteLine(n);

        // Converter<TInput, TOutput>
        Converter<string, int> parse = s => int.Parse(s);
        int parsed = parse("123");
        Console.WriteLine($"\nParsed value = {parsed}");

        // Small combined example: Func + Predicate
        Func<int[], int[]> filterAndSquare = arr =>
        {
            Predicate<int> positive = v => v > 0;
            int[] positives = Array.FindAll(arr, positive);
            for (int i = 0; i < positives.Length; i++) positives[i] = positives[i] * positives[i];
            return positives;
        };

        var result = filterAndSquare(new[] { -2, 0, 3, 4 });
        Console.WriteLine("\nFiltered & squared positives:");
        foreach (var v in result) Console.WriteLine(v);
    }
}

```
```

using System;

class Program
{
    // Custom delegate (for demonstration)
    delegate int BinaryOperation(int x, int y);

    static void Main()
    {
        Console.WriteLine("Anonymous method examples (C# 12 / .NET 8):\n");

        // 1) Anonymous method for a custom delegate
        BinaryOperation addAnon = delegate(int x, int y) { return x + y; };
        Console.WriteLine($"addAnon(3,4) = {addAnon(3,4)}");

        // 2) Anonymous method assigned to Func<T...>
        Func<int, int, int> multiplyAnon = delegate(int a, int b) { return a * b; };
        Console.WriteLine($"multiplyAnon(5,6) = {multiplyAnon(5,6)}");

        // 3) Anonymous method assigned to Action<T>
        Action<string> printAnon = delegate(string s) { Console.WriteLine($\"Print: {s}\"); };
        printAnon(\"Hello from Action (anonymous method)\");

        // 4) Predicate<T>
        Predicate<int> isOdd = delegate(int v) { return v % 2 != 0; };
        Console.WriteLine($\"isOdd(7) = {isOdd(7)}\");

        // 5) Multicast Action using anonymous methods
        Action multi = delegate { Console.WriteLine(\"First\"); };
        multi += delegate { Console.WriteLine(\"Second\"); };
        Console.WriteLine(\"\\nMulticast output:\");
        multi();

        // 6) Closure capture with anonymous method (captures 'factor')
        int factor = 10;
        Func<int, int> times = delegate(int x) { return x * factor; };
        Console.WriteLine($\"\\nBefore change factor=10 -> times(3) = {times(3)}\");
        factor = 20; // captured variable changed
        Console.WriteLine($\"After change factor=20 -> times(3) = {times(3)}\");

        // 7) Using anonymous method with Array.FindAll
        string[] names = { \"Alice\", \"Bob\", \"Andrew\", \"Cathy\" };
        var aNames = Array.FindAll(names, delegate(string s) { return s.StartsWith(\"A\"); });
        Console.WriteLine(\"\\nNames starting with 'A':\");
        foreach (var n in aNames) Console.WriteLine(n);

        // Short comparison: equivalent lambda for reference
        Func<int, int> squareLambda = x => x * x;
        Console.WriteLine($\"\\nLambda square(4) = {squareLambda(4)}\");
    }
}


```
```

using System;
using System.Threading;

class ProgressEventArgs : EventArgs
{
    public int Percentage { get; }
    public string? Message { get; }

    public ProgressEventArgs(int percentage, string? message = null)
    {
        Percentage = percentage;
        Message = message;
    }
}

class Downloader
{
    // Public event using the standard EventHandler<TEventArgs> pattern
    public event EventHandler<ProgressEventArgs>? ProgressChanged;

    // Protected virtual method to raise the event (allows derived classes to override)
    protected virtual void OnProgressChanged(ProgressEventArgs e)
    {
        // Thread-safe null check and invocation
        ProgressChanged?.Invoke(this, e);
    }

    // Simulate work and raise events
    public void Start()
    {
        for (int i = 0; i <= 100; i += 25)
        {
            Thread.Sleep(200); // simulate work
            OnProgressChanged(new ProgressEventArgs(i, $"Reached {i}%"));
        }

        // Final event indicating completion
        OnProgressChanged(new ProgressEventArgs(100, "Download complete"));
    }
}

class Program
{
    static void Main()
    {
        var downloader = new Downloader();

        // Subscriber 1: method group
        downloader.ProgressChanged += ReportProgress;

        // Subscriber 2: lambda capture (anonymous method)
        downloader.ProgressChanged += (sender, e) =>
        {
            if (e.Percentage >= 50)
                Console.WriteLine($"[Lambda] Halfway there: {e.Percentage}% - {e.Message}");
        };

        Console.WriteLine("Starting downloader...");
        downloader.Start();

        // Unsubscribe the method-based handler and run again to show subscription changes
        downloader.ProgressChanged -= ReportProgress;
        Console.WriteLine("\nRestarting downloader after unsubscribing ReportProgress...");
        downloader.Start();
    }

    // Named event handler
    static void ReportProgress(object? sender, ProgressEventArgs e)
    {
        Console.WriteLine($"[ReportProgress] {e.Percentage}% - {e.Message}");
    }
}


```