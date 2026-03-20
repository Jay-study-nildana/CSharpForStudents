# Day 25 — Concurrency exercises (10 problems)

Purpose: Practice recognizing shared-state hazards and applying thread-safety strategies: locks, Interlocked, immutable snapshots, message-queueing, concurrent collections, and making the right decisions between async and concurrency. For each problem, refactor or implement the requested solution and briefly explain why your approach is safe.

Instructions for students:
- Spend ~10–15 minutes per problem.
- Keep solutions small and focused.
- For each answer, include a short justification (1–3 sentences) describing why the chosen approach prevents races or deadlocks.

Problems

1) 01-RaceCondition_Counter
Problem:
This counter is not thread-safe. Show two safe implementations: one using `lock`, and one using `Interlocked`. Explain trade-offs.

```csharp
public class Counter
{
    public int Value = 0;
    public void Increment() => Value++; // not thread-safe
}
```

2) 02-Interlocked_CorrectUsage
Problem:
You need to implement a thread-safe "compare and set" increment that only increments if value is less than a cap. Use `Interlocked.CompareExchange` to implement `TryIncrementIfBelow(int cap)`.

```csharp
public class CappedCounter { private int _value; public bool TryIncrementIfBelow(int cap) { /* ... */ } }
```

3) 03-LockScope_Minimize
Problem:
This method locks too much (holds lock across I/O). Refactor to minimize lock scope and avoid blocking while holding the lock.

```csharp
private readonly object _sync = new();
private List<string> _items = new();
public void AddAndPersist(string s)
{
    lock (_sync)
    {
        _items.Add(s);
        File.AppendAllText("log.txt", s + Environment.NewLine); // I/O under lock
    }
}
```

4) 04-Deadlock_DetectionAndFix
Problem:
Two locks may deadlock due to inconsistent ordering. Provide a fix that enforces lock order or uses `TryEnter` with timeout and explain why it prevents deadlocks.

```csharp
private readonly object _a = new(); private readonly object _b = new();
// Thread1:
lock(_a) { Thread.Sleep(10); lock(_b) { /*...*/ } }
// Thread2:
lock(_b) { Thread.Sleep(10); lock(_a) { /*...*/ } }
```

5) 05-ImmutableSnapshot_Config
Problem:
A configuration object is read by many threads and updated occasionally. Implement a safe immutable snapshot approach for reads without locks and atomic publish for updates.

6) 06-MessageQueue_ChannelPattern
Problem:
Refactor a shared-data worker to a single-consumer Channel-based design. Provide producer and single consumer sample code that ensures all updates happen on one thread.

```csharp
// currently multiple threads touch a shared in-memory index -> propose Channel-based consumer
```

7) 07-ConcurrentCollections_UseCase
Problem:
You have many threads adding and frequently reading a set of items. Demonstrate use of a `ConcurrentDictionary` or `ConcurrentBag` for this scenario and explain why it’s preferable to manual locking.

8) 08-AsyncVsConcurrency_Decision
Problem:
Given two tasks: (A) fetching many URLs and aggregating results, (B) large matrix multiplication across many cores — decide which should be async and which should use parallelism. Implement a small code example for each illustrating the right API (`async/await` for I/O, `Parallel.For`/`Task.Run` for CPU).

9) 09-StressTest_ReproConcurrencyBug
Problem:
Write a small deterministic stress test that attempts to reproduce a race condition on a non-thread-safe stack by running many producers/consumers concurrently. Then show a fixed version using a `ConcurrentStack<T>` or proper locking.

10) 10-ThreadPoolStarvation_LongRunningTasks
Problem:
Demonstrate how scheduling many long-running CPU-bound tasks with `Task.Run` can starve the thread-pool for async I/O continuations. Show using `TaskCreationOptions.LongRunning` or a dedicated thread for long-running work to avoid starvation, with brief explanation.

---

Deliverables
- For each problem, provide a short (10–30 line) C# solution file named exactly as the problem title with `.cs` extension.
- Include short comments in each solution explaining the safety trade-offs.