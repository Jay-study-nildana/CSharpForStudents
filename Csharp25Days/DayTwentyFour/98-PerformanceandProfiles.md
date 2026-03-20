# Day 24 — Performance basics & simple profiling concepts

Objective: Understand time vs. space trade-offs, practice algorithmic thinking (big‑O intuition), and learn simple profiling approaches to find hot spots in .NET applications.

Why this matters
Performance is about making systems responsive and scalable. Good choices early (data structures, batching, caching) often save far more effort than last‑minute micro‑optimizations. Learn to reason about cost (time) and resource usage (space), find where the time is spent, and apply the right fix.

Key concepts (short)
- Time complexity (big‑O): How work grows with input size (n). Examples: O(1), O(log n), O(n), O(n log n), O(n²).
- Space complexity: Extra memory used by the algorithm (temporary buffers, caches).
- Time vs space trade‑off: You can often reduce time by using more memory (memoization, caches) or reduce memory by doing more work (recomputing values).
- Hot‑spot: A small part of code that consumes a disproportionate amount of time — fix this first.
- CPU‑bound vs I/O‑bound: Optimization strategies differ (algorithmic work vs. concurrency, buffering, or improving latency of external calls).

Algorithmic thinking — patterns to spot
- Replace O(n²) loops with hash-based lookups to get ~O(n).
- Choose data structures that match access patterns: Dictionary for fast lookup, List for iteration, Queue/Stack for FIFO/LIFO.
- Avoid repeated work inside loops; hoist invariants out of loops.
- Consider whether the problem needs exact answers or approximate data (sampling, approximate sets).

Common examples with C# snippets

1) Linear vs binary search (time complexity)
```csharp
// Linear search: O(n)
int IndexOfLinear<T>(List<T> list, T item)
{
    for (int i = 0; i < list.Count; i++)
        if (EqualityComparer<T>.Default.Equals(list[i], item)) return i;
    return -1;
}

// Binary search (sorted list): O(log n)
int IndexOfBinary<T>(List<T> sortedList, T item) where T : IComparable<T>
{
    int lo = 0, hi = sortedList.Count - 1;
    while (lo <= hi)
    {
        int mid = (lo + hi) >> 1;
        int cmp = sortedList[mid].CompareTo(item);
        if (cmp == 0) return mid;
        if (cmp < 0) lo = mid + 1; else hi = mid - 1;
    }
    return -1;
}
```
When the list is large and you need many lookups, keep data sorted (or use a HashSet/Dictionary) to reduce lookup time.

2) Memoization: trade memory for time
```csharp
// Recursive Fibonacci: exponential time, tiny memory
long FibRecursive(int n) => n <= 1 ? n : FibRecursive(n - 1) + FibRecursive(n - 2);

// Memoized Fibonacci: O(n) time, O(n) space
long FibMemo(int n)
{
    var cache = new long[n + 1];
    cache[0] = 0; if (n > 0) cache[1] = 1;
    for (int i = 2; i <= n; i++) cache[i] = cache[i - 1] + cache[i - 2];
    return cache[n];
}
```
Memoization uses extra space to avoid repeated work — good when the same subproblems recur.

3) Avoid allocating inside hot loops (use Span/ArrayPool)
```csharp
// Bad: allocates a new array each iteration
for (int i = 0; i < N; i++)
{
    var tmp = new byte[1024]; // causes GC pressure
    Process(tmp);
}

// Better: reuse a buffer from ArrayPool or a Span over a single buffer
var buffer = ArrayPool<byte>.Shared.Rent(1024);
try
{
    for (int i = 0; i < N; i++) Process(buffer);
}
finally { ArrayPool<byte>.Shared.Return(buffer); }
```
Reducing allocations reduces GC overhead and improves throughput.

Simple profiling approaches (conceptual)
1) Measurement-first: don't optimize blind. Use measurements to find hot spots.
2) Wall‑clock timing (Stopwatch): quick and dirty to compare two approaches.
3) Sampling profilers: periodically sample the call stack to find where CPU time accumulates — low overhead and good for hotspots.
4) Instrumentation profilers: measure entry/exit times for functions — higher overhead but gives exact counts and durations.
5) Allocation profiling: track object allocations to find GC pressure sources.
6) I/O profiling: measure latency of disk, network, and database calls (these often dominate).
7) Flame graphs and visualizations: help spot hot code paths and call stack distribution.
8) BenchmarkDotNet: reproducible microbenchmarks with statistical rigor for local method-level tests.

Practical .NET examples

A) Quick micro-timing with Stopwatch (useful for rough comparisons)
```csharp
var sw = Stopwatch.StartNew();
// operation to measure
DoWork();
sw.Stop();
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
```
Use multiple iterations, warm up the JIT, and avoid measuring cold-start costs. For microbenchmarks, prefer BenchmarkDotNet (it handles warmup, noise, and statistical analysis).

B) Use BenchmarkDotNet for method-level comparisons
```csharp
// Install BenchmarkDotNet and write benchmark classes.
// It runs many iterations, warms up, and reports allocations and statistical metrics.
```
(Students: we can demo a small BenchmarkDotNet example in-class.)

Finding hot spots: a simple workflow
- Reproduce the performance issue with a realistic workload.
- Measure end-to-end with timing and counters (latency, throughput).
- Use a sampling profiler to identify which methods consume the most CPU.
- Inspect allocation profiles to find high-allocation code.
- Focus on the top 10% of code that accounts for ~90% of time.
- Apply a targeted fix (algorithm change, cache, batch) and re-measure.

Performance fixes — common high-impact actions
- Algorithmic improvements: reduce complexity (O(n²) -> O(n log n) or O(n)).
- Use appropriate data structures: Dictionary for lookups, Queue for streaming.
- Caching/memoization: cache expensive computations or remote calls.
- Batch processing: group I/O or database updates into larger batches.
- Pagination, streaming: avoid loading all data into memory.
- Reduce allocations: reuse buffers, use Span<T>, prefer structs for small hot-value types.
- Indexes and query tuning: for DB-bound operations, add indexes or rewrite queries.
- Parallelism: use Task/Parallel where operations are independent (beware of contention).

Homework (apply to your capstone)
- Identify an operation that runs frequently or over large inputs.
- Measure current time and memory usage under realistic load.
- Propose 2 mitigations (one algorithmic, one system-level such as caching or batching).
- Implement and measure the improvement; report time and memory delta.

Quick checklist for reviews
- Is the algorithmic complexity appropriate for expected input sizes?
- Are there obvious repeated computations inside loops?
- Are large results streamed or paged rather than fully materialized?
- Are expensive remote calls batched or cached?
- Is GC pressure visible (many short-lived allocations)? Consider reusing buffers.
- Can profiling be automated as part of CI or a routine performance test?

Further reading and tools
- BenchmarkDotNet (method microbenchmarks)
- dotnet-counters, dotnet-trace, dotnet-dump (troubleshooting and tracing)
- Visual Studio Profiler / PerfView (sampling, flame graphs)
- “Systems Performance” by Brendan Gregg (profiling techniques and flame graphs)

Wrap-up
Think in terms of algorithms first: get the right complexity and data structure, then worry about allocations and I/O. Measure, then fix the hot spots with targeted changes like caching, batching, or better algorithms — always re-measure to confirm the effect.