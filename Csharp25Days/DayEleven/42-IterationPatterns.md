# Day 11 — Iteration Patterns (C# / .NET)

Objectives
- Learn common iteration patterns in C# and when to use them.
- Understand performance and safety trade-offs (complexity, allocations, mutation).
- See concrete snippets: index loops, foreach/enumerators, LINQ, yield/async streams, parallel loops, Span<T>.

Overview
Iteration is how you process collection elements. Choose the pattern by the collection type, operations you need (indexing, removal, filtering), performance needs, and concurrency. Most iterations are O(n), but constant factors, allocations, and safety differ.

Index-based loops (arrays, List<T>)
- Good when you need indices, want to mutate elements by index, or iterate backwards to remove items.
- Cache `Count` for List<T> if computing it is expensive in your context.

```csharp
int[] arr = {1,2,3,4};
for (int i = 0; i < arr.Length; i++)
{
    Console.WriteLine(arr[i]); // O(1) access
}

// Removing items safely: iterate backwards
var list = new List<int>{1,2,3,4,5};
for (int i = list.Count - 1; i >= 0; i--)
{
    if (list[i] % 2 == 0) list.RemoveAt(i);
}
```

foreach and IEnumerator
- `foreach` is idiomatic, concise, and disposes the enumerator automatically. Works for any `IEnumerable<T>`.
- Avoid modifying the underlying collection during `foreach` (throws `InvalidOperationException` for many collections).

```csharp
var names = new List<string>{"Alice","Bob"};
foreach (var name in names)
    Console.WriteLine(name);

// Manual enumerator (rarely needed)
using var e = names.GetEnumerator();
while (e.MoveNext())
{
    var name = e.Current;
    // process name
}
```

LINQ and functional-style iteration
- Clear and expressive for transformations, filtering, and projections.
- Easier to read but may allocate and produce deferred execution — consider materializing (`ToList`) when needed.

```csharp
var top = names.Where(n => n.Length > 3).OrderBy(n => n).Select(n => n.ToUpper()).ToList();
```

Iterator blocks (`yield return`) and async streams (`IAsyncEnumerable<T>`)
- `yield return` builds an enumerator for streaming results without materializing the entire sequence.
- `IAsyncEnumerable<T>` allows async iteration (await foreach) for I/O-bound streams.

```csharp
IEnumerable<int> CountTo(int n)
{
    for (int i = 1; i <= n; i++) yield return i;
}

// Async stream
async IAsyncEnumerable<string> ReadLinesAsync(StreamReader r)
{
    while (!r.EndOfStream)
        yield return await r.ReadLineAsync();
}
```

Parallel and concurrent iteration
- `Parallel.For` / `Parallel.ForEach` help when work per item is CPU-heavy and independent.
- Use concurrent collections (e.g., `ConcurrentDictionary`) for thread-safe mutations.

```csharp
Parallel.ForEach(dataPartition, item =>
{
    Process(item); // CPU-bound work
});
```

Span<T>, Memory<T>, and slicing
- `Span<T>` provides zero-allocation, stack-friendly iteration over contiguous memory (arrays, stackalloc, native). Use in performance-critical inner loops.

```csharp
Span<int> s = stackalloc int[3] {1,2,3};
for (int i = 0; i < s.Length; i++) Console.WriteLine(s[i]);
```

Iterating dictionaries, sets, and other structures
- Iterate `Dictionary<TKey,TValue>` with `foreach (var kv in dict)` to avoid extra indexers.
- `HashSet<T>` iteration yields unique items but order is unspecified.

```csharp
var dict = new Dictionary<string,int>{{"a",1},{"b",2}};
foreach (var kv in dict) Console.WriteLine($"{kv.Key} => {kv.Value}");
```

Performance & complexity notes
- Most iterations are O(n). Key differences are allocations, branching, and per-item cost (e.g., IEnumerator boxing for value types prior to C# optimizations).
- `foreach` is generally as fast as `for` for reference-type collections; `for` may be slightly faster for List<T> in hot loops.
- LINQ trades simplicity for some overhead; benchmark if performance-critical.
- Avoid modifying collections while iterating; prefer backward `for` loops for removal or build a new collection.

Best practices
- Prefer `foreach` for readability unless you need indices or removals.
- Use `for` backwards when removing items from a list.
- Use `Span<T>` in tight loops to avoid allocations.
- Use `IAsyncEnumerable<T>` for asynchronous streamed data.
- For multi-threaded iteration with mutation, use concurrent collections and careful synchronization.

Homework
Pick a capstone entity and show two iteration examples:
1) Read all items and compute an aggregate (e.g., sum) — choose pattern and state complexity.
2) Remove items matching a predicate — show safe iteration and explain why you chose it.

Be prepared to justify your pattern by operation semantics, complexity, and memory behavior.