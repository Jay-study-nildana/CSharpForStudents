# Synchronous vs Asynchronous (C# / .NET)

Purpose
- Understand the difference between synchronous (blocking) and asynchronous (non-blocking) code.
- Learn how Tasks and async/await work in .NET, common pitfalls (deadlocks, context capture), and best practices for I/O vs CPU-bound work.
- See concise C# examples: async/await, cancellation, Task.WhenAll, I/O examples, and a deadlock illustration.

Core idea: blocking vs non-blocking
- Synchronous (blocking): a call waits for the operation to finish on the same thread before continuing. The thread cannot do other work while blocked.
  - Example: `Thread.Sleep(1000)` — the thread is idle for 1 second.
- Asynchronous (non-blocking): the call returns quickly with a representation of the ongoing work (a Task). The thread can continue doing other work; the remainder executes after completion (via await or continuation).
  - Example: `await httpClient.GetAsync(url)` — current method yields while the I/O completes.

Tasks and async/await (brief)
- Task and Task<T> are .NET’s promises/futures for asynchronous work.
- `async` marks a method containing `await`. `await` yields control until the awaited Task completes, without blocking the thread.
```csharp
public async Task<string> FetchAsync(HttpClient http, string url)
{
    var resp = await http.GetAsync(url);           // non-blocking I/O
    resp.EnsureSuccessStatusCode();
    return await resp.Content.ReadAsStringAsync();
}
```
- The caller receives a `Task<string>` immediately and can `await` it or attach continuations.

When to use async
- I/O-bound operations: network, database, file I/O. Use async to free threads and increase scalability (especially in server apps).
- CPU-bound work: keep synchronous unless you need parallelism; offload to the threadpool with `Task.Run` if you must avoid blocking a UI thread.
```csharp
// CPU-bound offload (e.g., in a UI app)
public Task<int> ComputeAsync() => Task.Run(() => HeavyComputation());
```

Avoid synchronous waits (don’t block on Tasks)
- Calling `.Result` or `.Wait()` on a Task can cause deadlocks in contexts with synchronization (e.g., GUI or ASP.NET with SynchronizationContext).
- Bad pattern:
```csharp
// Deadlock-prone
var data = http.GetStringAsync(url).Result;
```
- Prefer `await` all the way up the call chain.

SynchronizationContext and ConfigureAwait
- In apps with a SynchronizationContext (legacy UI apps, older ASP.NET), `await` by default captures context and resumes on it. This can be unnecessary and cause deadlocks/overhead.
- Use `ConfigureAwait(false)` in library/low-level code to avoid capturing the context:
```csharp
var content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
```
- In ASP.NET Core there is no SynchronizationContext, so context capture is less of a concern; still using ConfigureAwait(false) in library code is good practice.

Deadlock example (why .Result can hang)
```csharp
// Simplified deadlock illustration (do not use)
public void BlockingEntry()
{
    // This blocks the thread that may be needed to run the continuation
    var t = AsyncMethod(); 
    t.Wait(); // or t.Result
}

public async Task AsyncMethod()
{
    await Task.Delay(100); // continuation may try to resume on the blocked thread
}
```
- Fix: make the entry point async or use `Task.Run` to avoid capturing the blocked context.

Composing tasks: WhenAll and WhenAny
- Run multiple independent async operations concurrently and await all:
```csharp
var tasks = urls.Select(u => http.GetStringAsync(u)).ToArray();
var results = await Task.WhenAll(tasks); // all downloads in parallel
```
- Use `Task.WhenAny` to process results as they arrive.

Cancellation and timeouts
- Support CancellationToken for cooperative cancellation; pass tokens into async APIs and check in long-running operations.
```csharp
public async Task<string> FetchWithCancellationAsync(HttpClient http, string url, CancellationToken ct)
{
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
    cts.CancelAfter(TimeSpan.FromSeconds(10)); // timeout
    var resp = await http.GetAsync(url, cts.Token).ConfigureAwait(false);
    resp.EnsureSuccessStatusCode();
    return await resp.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
}
```
- Propagate tokens from callers (composition root / request scope) so operations can be cancelled cleanly.

Exception handling
- Exceptions in async methods are captured in the returned Task and re-thrown when awaited. Always `await` or observe the Task to see exceptions.
```csharp
try
{
    await FetchAsync(http, url);
}
catch (HttpRequestException ex)
{
    // handle network error
}
```

Streaming and IAsyncEnumerable
- For streaming sequences use `IAsyncEnumerable<T>` with `await foreach` to process items as they arrive without buffering all in memory:
```csharp
public async IAsyncEnumerable<int> GenerateAsync()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

public async Task ConsumeAsync()
{
    await foreach (var n in GenerateAsync())
        Console.WriteLine(n);
}
```

Best practices summary
- Async all the way: prefer `async`/`await` over blocking APIs; expose async APIs when internal operations are asynchronous.
- Use async for I/O-bound work; use Task.Run only for CPU-bound work when you must avoid blocking UI or request threads.
- Avoid `.Result` / `.Wait()`; propagate async to callers.
- Use `ConfigureAwait(false)` in library code to avoid unnecessary context capture.
- Support CancellationToken and timeouts for long-running operations.
- Use `Task.WhenAll` for parallelism; understand that parallelism increases concurrency but also resource usage.
- Handle exceptions by awaiting tasks and using try/catch; consider retry and transient-fault handling for I/O.

Marking async boundaries in your capstone
- Identify I/O boundaries: database, HTTP calls, file I/O, message bus — these should be async.
- Background processing: use IHostedService / BackgroundService for long-running tasks; create scopes for per-operation services.
- Document which methods are async (naming conventions: `Async` suffix) and where tasks are awaited or awaited concurrently.

Homework (short)
- Inspect one capstone flow that does remote I/O (e.g., fetching updates or saving reports). Mark the async boundaries and rewrite one sequential call to run concurrently with Task.WhenAll. Provide a 6–10 line rationale about why this improves responsiveness or throughput.

Further reading
- Microsoft: async/await in C#
- Stephen Cleary: "Concurrency in C#" blog and Async in Depth articles
- Best practices for cancellation, ConfigureAwait, and streaming with IAsyncEnumerable
