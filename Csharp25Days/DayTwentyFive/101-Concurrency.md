# Day 25 — Concurrency basics & thread-safety concepts

Objective: Learn the risks of shared mutable state, recognize race conditions, compare locks vs immutable strategies, and decide when to use concurrency (parallelism) versus async (asynchronous I/O).

Why this matters
Concurrent programs can run faster and handle more work, but shared mutable state introduces subtle bugs (race conditions, data corruption, deadlocks). Understanding simple patterns — locks, atomic operations, immutable snapshots, and message-based designs — helps you make safe choices for your capstone.

1) Shared-state hazards & race conditions (overview)
- Race condition: two or more threads access and modify the same data concurrently and outcome depends on interleaving.
- Lost updates, torn reads, stale data, and memory visibility (ordering) are common issues.
- Rule of thumb: any shared mutable variable accessed by multiple threads must be synchronized or made immutable.

Example — a classic race on a counter:
```csharp
// NOT thread-safe
public class Counter
{
    public int Value = 0;

    public void Increment() => Value++; // read-modify-write race
}
```
Multiple threads calling Increment concurrently can lose increments because `Value++` is not atomic.

2) Simple fixes: locks and atomic operations
- Locks (Monitor / lock keyword) make critical sections exclusive. Keep lock scope minimal.
- Interlocked provides atomic operations for common primitives with lower overhead than locks.

Lock example:
```csharp
private readonly object _sync = new object();
public void Increment()
{
    lock (_sync)
    {
        _value++;
    }
}
```

Atomic example with Interlocked:
```csharp
private int _value;
public void Increment() => Interlocked.Increment(ref _value);
```
Use Interlocked for simple counters and flags. Use locks when you need to protect multiple related fields or a sequence of operations as an atomic unit.

3) Locking pitfalls and guidelines
- Avoid locking on publicly accessible objects (use private readonly objects).
- Keep critical sections short and avoid blocking I/O while holding locks.
- Be mindful of lock ordering to avoid deadlocks (establish a global lock order).
- Consider ReaderWriterLockSlim when many readers and few writers are expected.

4) Immutable strategies: avoid locks by not sharing mutable state
Making data immutable removes synchronization needs: readers can access snapshots safely without locks.

Immutable snapshot approach:
```csharp
// Immutable state object
public record Settings(string ConnectionString, int MaxItems);

// Swap reference atomically
private volatile Settings _current = new Settings("...", 100);

public void Update(Settings s) => _current = s; // writers publish new immutable instance
public Settings Get() => _current;              // readers read safely without locks
```
Use System.Collections.Immutable for collections (ImmutableList<T>, ImmutableDictionary<TKey,TValue>) when many readers access a shared collection.

Copy-on-write is practical for read-mostly scenarios: create a modified copy and publish it; readers continue using the old snapshot until they read the new one.

5) Message-passing and actor-like patterns (avoid shared state)
Instead of sharing state, encapsulate mutable state in a single-threaded worker and interact via messages (queues). This eliminates races.

Example: producer / consumer using Channel<T>:
```csharp
var channel = Channel.CreateUnbounded<WorkItem>();

// Producer
async Task ProduceAsync()
{
    await channel.Writer.WriteAsync(new WorkItem(...));
}

// Consumer (single worker)
async Task ConsumeAsync()
{
    await foreach (var item in channel.Reader.ReadAllAsync())
    {
        Process(item); // single-threaded access to shared resources
    }
}
```
Message-based patterns are excellent for tasks that can be serialized or require consistent single-threaded access (e.g., updating an in-memory index).

6) Concurrent collections
For cases where many threads need to add or consume items, prefer built-in thread-safe collections:
- ConcurrentQueue<T>, ConcurrentStack<T>, ConcurrentBag<T>
- ConcurrentDictionary<TKey, TValue>

These collections implement efficient synchronization internally and avoid most manual lock mistakes.

7) When to use concurrency vs async
- Async (async/await) is for I/O-bound work (network, disk). It frees threads while waiting and improves scalability.
  Example: use async when calling HttpClient, database async APIs, or file I/O.
- Concurrency/parallelism (Task.Run, Parallel.For, PLINQ) is for CPU-bound work you want to parallelize on multiple cores.
  Example: CPU-intensive computations, image processing, or data transformations.

Mixing guidelines:
- Don't use Task.Run to wrap I/O work — use async APIs instead.
- Use async I/O to reduce thread usage; use concurrency to increase CPU throughput when you have spare cores.
- Beware of thread-pool starvation: long-running CPU work should use TaskCreationOptions.LongRunning or be scheduled carefully.

8) Practical examples and patterns

Producer–consumer with ConcurrentQueue:
```csharp
var queue = new ConcurrentQueue<WorkItem>();
var signal = new AutoResetEvent(false);

// Producer
queue.Enqueue(item);
signal.Set();

// Consumer
while (!cancellationRequested)
{
    if (queue.TryDequeue(out var item)) Process(item);
    else signal.WaitOne(100); // back-off
}
```

Safe lazy initialization using Lazy<T>:
```csharp
private readonly Lazy<Expensive> _instance = new Lazy<Expensive>(() => new Expensive(), isThreadSafe: true);
public Expensive Instance => _instance.Value;
```

9) Testing and diagnosing concurrency bugs
- Concurrency bugs are non-deterministic: add stress tests and long-running integration tests.
- Use logging with timestamps and thread IDs to inspect interleavings.
- Tools: Visual Studio Concurrency Visualizer, Thread Sanitizers (third-party), and concurrency-aware analyzers.
- Reproduce with controlled schedulers in tests where possible.

10) Quick checklist for your capstone
- Identify shared mutable state. Can it be made immutable or isolated?
- Use immutable snapshots or message queues for complex shared resources.
- Use Interlocked for simple counters; locks for multi-field invariants.
- Choose Concurrent collections over manual locking when appropriate.
- Use async for I/O-bound operations and concurrency for CPU-bound work.
- Keep critical sections short; avoid blocking while holding locks.
- Add stress and integration tests targeting concurrency scenarios.

Further reading & tools
- Microsoft docs: Threading in .NET, System.Threading.Channels
- Stephen Toub: articles on concurrency patterns
- Books: “Concurrency in C# Cookbook”, “The Art of Multiprocessor Programming”

Wrap-up
Prefer designs that avoid shared mutable state when possible (immutability, message passing). When sharing is necessary, choose the simplest correct synchronization (Interlocked, locks, or concurrent collections). Use async for I/O and concurrency for CPU work — and test with stress scenarios. Concurrency is powerful but requires discipline: document your strategy for the capstone and keep synchronization localized and well-tested.