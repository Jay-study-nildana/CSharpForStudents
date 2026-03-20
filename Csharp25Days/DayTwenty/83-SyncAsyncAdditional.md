# Non-blocking I/O Benefits & Common Pitfalls — C#/.NET (Day 20)

This one-page guide explains why non-blocking I/O matters, the concrete benefits you’ll get from using it correctly in .NET, and common pitfalls—especially deadlocks and capturing the synchronization context. Code snippets show both problematic patterns and recommended practices.

## What is non-blocking I/O?
Non-blocking I/O is a programming approach where an operation that waits for external resources (disk, network, database) does not hold a thread while waiting. Instead, the operation yields control and the runtime resumes execution when the I/O completes. In .NET this is usually expressed with Task, async/await, and APIs that return Task or Task<T>.

## Benefits of non-blocking I/O

- Responsiveness: UI threads remain free to respond to user input while background I/O completes.
- Scalability: Servers can handle many concurrent I/O operations without needing one thread per request — fewer threads means less context-switching and lower memory usage.
- Resource efficiency: Thread pool threads are not tied up waiting on slow I/O; they can serve other work.
- Simpler concurrency: async/await provides linear-style code while preserving non-blocking behavior and good exception flow.

Example: synchronous (blocking) vs non-blocking (async)

```csharp
// Blocking: ties up a thread while waiting
string FetchSync(string url)
{
    using var client = new HttpClient();
    return client.GetStringAsync(url).Result; // blocks the calling thread
}

// Non-blocking: returns quickly and resumes when I/O completes
async Task<string> FetchAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url); // does not block the calling thread
}
```

## Common pitfalls

1. Deadlocks due to blocking on async code
   - Problem: Blocking a thread (e.g., using .Result or .Wait()) that also needs to be the continuation target can cause a deadlock in contexts that capture a SynchronizationContext (UI thread, legacy ASP.NET).
   - Example that can deadlock on a UI thread:

```csharp
// On a UI thread:
// This can deadlock because GetRemoteAsync tries to resume on the UI context
// but the UI thread is blocked waiting for Result.
var result = GetRemoteAsync().Result;

async Task<string> GetRemoteAsync()
{
    await Task.Delay(1000); // will try to capture and resume on the captured context
    return "done";
}
```

   - Why: await by default captures the current SynchronizationContext and schedules the continuation back onto it. If that context (the UI thread) is blocked waiting, the continuation cannot run.

2. Capturing the SynchronizationContext unintentionally
   - Behavior: By default await captures context. For library code that doesn't need to interact with UI, this can be unnecessary and reduce scalability.
   - Remedy: Use ConfigureAwait(false) in library or low-level I/O code so continuations don't try to marshal back to the original context:

```csharp
// Library code: avoid capturing context
async Task<string> ReadRemoteLibraryAsync()
{
    using var client = new HttpClient();
    // ConfigureAwait(false) avoids resuming on the caller's context
    return await client.GetStringAsync("https://example.com").ConfigureAwait(false);
}
```

   - Note: In application code that updates UI after await, do not use ConfigureAwait(false) because you need continuation on the UI context.

3. Overuse of Task.Run for I/O-bound work
   - Mistake: Wrapping I/O-bound async operations in Task.Run wastes threads and negates benefits of non-blocking I/O.
   - Correct use:
     - Task.Run: CPU-bound, expensive synchronous computations that should run on a thread-pool thread.
     - Pure I/O: use naturally async APIs (HttpClient, FileStream with async, EF Core async, etc.) and await them.

4. Fire-and-forget tasks with no error handling
   - Unobserved exceptions from background tasks can crash apps or be lost.
   - Pattern: If you must fire-and-forget, explicitly log exceptions or register continuations to observe them.

```csharp
// Safe fire-and-forget helper (simple)
void FireAndForget(Task t)
{
    _ = t.ContinueWith(task =>
    {
        if (task.IsFaulted) Log(task.Exception);
    }, TaskScheduler.Default);
}
```

## Example: deadlock scenario in ASP.NET classic vs ASP.NET Core

- Classic ASP.NET has a SynchronizationContext that may cause deadlocks when mixing blocking calls with async code.
- ASP.NET Core does not use the old SynchronizationContext and is less prone to this particular deadlock, but blocking threads still hurts throughput.

Bad pattern (may deadlock or reduce throughput):

```csharp
// ASP.NET action - BAD: synchronous blocking
public string Get()
{
    // Blocks thread-pool thread; reduces throughput
    return GetDataAsync().Result;
}
```

Good pattern (async all the way):

```csharp
// ASP.NET Core action - GOOD: non-blocking
public async Task<string> Get()
{
    return await GetDataAsync();
}
```

## Debugging checklist for deadlocks & context issues

- Search for .Result, .Wait(), .GetAwaiter().GetResult() — these are red flags.
- Use ConfigureAwait(false) in library code that doesn't need the context.
- Ensure "async all the way" — callers propagate async instead of blocking.
- If the UI freezes, check for long-running CPU work on the UI thread; use Task.Run for CPU-bound work.
- In server apps, replace synchronous I/O calls with async equivalents to improve throughput.

## Short classroom exercise (capstone design)
Sketch your capstone’s data flow (file reads, HTTP calls, DB queries) and mark:
- Async boundaries: where to expose Task/Task<T> — typically at I/O entry points.
- Background processing: long-running, non-critical work (e.g., batch import) that should be executed by a background service or queue.
- Where ConfigureAwait(false) is appropriate (library/data access layers) and where you must resume on the context (UI update points).

## Best practices summary
- Prefer async/await for I/O-bound work.
- Make methods async all the way; avoid blocking on tasks.
- Use ConfigureAwait(false) in libraries; avoid it where you need the context (UI).
- Do not wrap I/O in Task.Run; use it only for CPU-bound work.
- Observe or log exceptions from fire-and-forget tasks.
- Use CancellationToken to support cooperative cancellation.

Quick reference: deadlocks often come from blocking on async code when SynchronizationContext is captured; avoid blocking — await instead.

References and further reading: official docs on async/await, ConfigureAwait, Task-based Asynchronous Pattern (TAP), and TaskCompletionSource.
