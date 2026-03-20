# Day 12 — Generics & Type Safety (C# / .NET)

Objectives
- Why generics matter: type safety, reuse, and performance.
- Core generic collections and simple generic types you’ll write.
- How generics avoid boxing/unboxing and improve reuse.
- Examples (generic methods, classes, constraints) and practical guidance.
- Guided exercise: generic method signatures for sorting/filtering.
- Homework: rewrite earlier pseudocode using generics.

Why generics?
Generics let you write algorithms and data structures that work across many types while preserving compile-time type safety. Instead of using loosely typed containers (e.g., `ArrayList`) and casts that may fail at runtime, generics express intent (`List<int>`) and catch mismatches early. For value types (structs) generics also avoid boxing/unboxing, improving performance and reducing GC pressure.

Core generic collections in .NET
- `List<T>` — dynamic array with O(1) index access and amortized O(1) append.
- `Dictionary<TKey,TValue>` — hash table for average O(1) lookup/insert/delete by key.
- `HashSet<T>` — deduplicated items with fast membership checks.
- `SortedDictionary<TKey,TValue>` / `SortedSet<T>` — tree-based ordered collections with O(log n) operations.
- `ConcurrentDictionary<TKey,TValue>` — thread-safe hash map for parallel scenarios.
These collections are strongly typed: the element/key/value types are compile-time parameters, so no casts are necessary when retrieving elements.

Simple generic types and methods
You’ll commonly implement small generic helpers to improve reuse and clarity.

Generic class example: Pair<T>
```csharp
public class Pair<T>
{
    public T First { get; set; }
    public T Second { get; set; }
    public Pair(T first, T second) { First = first; Second = second; }
}
```

Generic method example: Swap<T>
```csharp
public static void Swap<T>(ref T a, ref T b)
{
    var tmp = a;
    a = b;
    b = tmp;
}
```

Generic constraints
Constraints narrow the types allowed and enable use of member APIs at compile time:
- `where T : class` — reference type
- `where T : struct` — value type (non-nullable)
- `where T : new()` — parameterless constructor required
- `where T : IComparable<T>` — supports comparisons
- `where T : notnull` — disallow null (C# 8+)
Example: a generic sort helper with comparability:
```csharp
public static IEnumerable<T> SortBy<T, TKey>(
    IEnumerable<T> source,
    Func<T, TKey> keySelector)
    where TKey : IComparable<TKey>
{
    return source.OrderBy(keySelector); // LINQ uses the constraint for intent
}
```

Avoiding boxing/unboxing
Boxing occurs when a value type (e.g., `int`) is converted to `object` — the runtime wraps it in a heap object. Unboxing extracts it back. These operations allocate and cost CPU cycles.

Non-generic (boxing) example — avoid this:
```csharp
var list = new System.Collections.ArrayList();
list.Add(1);             // boxes int to object
int x = (int)list[0];    // unboxes
```

Generic (no boxing) alternative — prefer this:
```csharp
var list = new List<int>();
list.Add(1);             // no boxing
int x = list[0];         // strongly-typed, no unboxing
```

Why generics avoid boxing: the CLR/JIT generates efficient native code for generic methods. For value-type instantiations the JIT produces specialized code paths, so operations avoid treating values as `object`.

Reusability & type safety
Generics let you express algorithms once and reuse them across types without losing static typing. This reduces duplicated code and runtime casting errors.

Generic merge example
```csharp
public static List<T> MergeSorted<T>(IList<T> a, IList<T> b, IComparer<T> comparer)
{
    comparer ??= Comparer<T>.Default;
    int i = 0, j = 0;
    var outList = new List<T>();
    while (i < a.Count && j < b.Count)
    {
        if (comparer.Compare(a[i], b[j]) <= 0) outList.Add(a[i++]);
        else outList.Add(b[j++]);
    }
    while (i < a.Count) outList.Add(a[i++]);
    while (j < b.Count) outList.Add(b[j++]);
    return outList;
}
```
This one method works for `int`, `string`, or custom domain types with an appropriate `IComparer<T>`.

Guided exercise — method signatures for sorting and filtering
Design general-purpose signatures to use in the guided exercise:

- Filter:
```csharp
IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T,bool> predicate)
```

- Sort by key:
```csharp
IEnumerable<T> SortBy<T, TKey>(IEnumerable<T> source, Func<T,TKey> keySelector)
    where TKey : IComparable<TKey>
```

- Top-K with optional comparer:
```csharp
IEnumerable<T> TopK<T>(IEnumerable<T> source, int k, IComparer<T>? comparer = null)
```

- Generic repository sketch:
```csharp
interface IRepository<T, TId>
{
    void Add(T item);
    T? Get(TId id);
    void Update(T item);
    bool Remove(TId id);
}
```

Practical tips: constraints, variance, and common pitfalls
- Prefer `IEnumerable<T>`/`IReadOnlyList<T>` in APIs to expose minimal required behavior.
- Use `IComparer<T>` or `Comparison<T>` to decouple ordering from types.
- Covariance/contravariance (e.g., `IEnumerable<Derived>` → `IEnumerable<Base>`) applies only to interfaces/delegates with the appropriate `in`/`out` annotations. Be careful when designing APIs where variance matters.
- Avoid overly complex generic parameter lists; if you have many type parameters consider refactoring or documenting clearly.
- Watch for `default(T)` behaviors: for reference types it’s `null`, for value types it’s the zeroed value — document expectations.

Performance notes and trade-offs
- Generics reduce runtime casts and avoid boxing for value types, leading to fewer allocations and better performance.
- The JIT produces separate native code for value-type instantiations; for reference types it may reuse code — this is generally efficient.
- LINQ and some higher-level generic helpers trade expressiveness for small allocations and indirections; in hot paths measure with BenchmarkDotNet.
- Use specialized structures (e.g., `Span<T>`, `Memory<T>`) when you need stack-friendly, zero-allocation access to memory; they are generic-friendly and avoid boxing.

Example: avoiding boxing in a generic aggregator
```csharp
public static TAccumulate Aggregate<TSource, TAccumulate>(
    IEnumerable<TSource> source,
    TAccumulate seed,
    Func<TAccumulate, TSource, TAccumulate> func)
{
    var acc = seed;
    foreach (var item in source) acc = func(acc, item);
    return acc;
}

// Using with value types keeps everything unboxed.
```

Best practices
- Prefer generic collections (`List<T>`, `Dictionary<K,V>`) over legacy non-generic ones.
- Use clear type parameter names (`TEntity`, `TKey`, `TValue`) in public APIs for readability.
- Add constraints when you rely on specific operations (comparison, construction).
- Start simple: use generics to express intent, then optimize with specialized types only after profiling.

Homework (apply generics)
Rewrite three earlier algorithms using generics:
1. MergeSortedLists → `MergeSorted<T>(IList<T>, IList<T>, IComparer<T>)`
2. CountDistinctWords → `CountDistinct<T>(IEnumerable<T>, IEqualityComparer<T>?)` — make case-insensitive string handling via `StringComparer.OrdinalIgnoreCase`.
3. TopKFrequentWords → `TopKByCount<T>(IEnumerable<T>, int k, IEqualityComparer<T>?)`

For each rewrite:
- Provide method signature and implementation sketch.
- State time and space complexity.
- Note whether boxing occurs for value types (it should not).

Further reading
- C# language specification: Generics chapter.
- Eric Lippert and Mads Torgersen blog posts on generics and CLR behavior.
- BenchmarkDotNet for measuring boxing/unboxing and method instantiation costs.

Summary
Generics are essential for safe, reusable, and efficient C# code. Use them for collections, algorithm abstractions, and public APIs where compile-time guarantees matter. They prevent many runtime errors, reduce allocations for value types, and let you write one correct implementation that works across many types.