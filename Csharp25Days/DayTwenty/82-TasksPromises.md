# Tasks / Promises — C#/.NET Fundamentals (Day 20)

This guide explains the concept of tasks (the .NET “promise”), how they model asynchronous work, and practical patterns you’ll use in real apps. It's aimed at developers learning when to use asynchronous code and how to avoid common pitfalls.

## What is a Task (aka a Promise)?
A Task represents an ongoing operation that may complete now or later. It can:
- produce no value: `Task`
- produce a value: `Task<T>`

Think of a Task like a pizza order ticket (a promise): you get a ticket immediately and later either the pizza (result) or a notification that the order failed (exception).

Tasks are not threads. They describe work and completion; the runtime schedules actual work on threads when needed. Using tasks lets your code be non-blocking: consumers can await completion instead of blocking a thread.

## Basic examples

Non-blocking asynchronous method:

```csharp
// returns a Task<int> — a promise to provide an int later
async Task<int> GetRemoteCountAsync()
{
    // simulate I/O that doesn't tie up a thread
    await Task.Delay(500);
    return 42;
}

// consumer
async Task UseCountAsync()
{
    int count = await GetRemoteCountAsync(); // non-blocking; context resumes when complete
    Console.WriteLine(count);
}
```

CPU-bound work: use Task.Run to offload work to a thread-pool thread:

```csharp
int Compute()
{
    // expensive CPU-bound work
}

async Task<int> ComputeAsync()
{
    return await Task.Run(() => Compute()); // offload CPU work
}
```

Prefer `await` over `ContinueWith` for readability and correct exception propagation:

```csharp
// old-style continuation — harder to read
Task.Run(() => 1 + 1)
    .ContinueWith(t => Console.WriteLine(t.Result));

// preferred
int result = await Task.Run(() => 1 + 1);
Console.WriteLine(result);
```

## Coordination: WhenAll and WhenAny

Run tasks in parallel and await all results:

```csharp
Task<int> t1 = GetRemoteCountAsync();
Task<int> t2 = GetRemoteCountAsync();
int[] results = await Task.WhenAll(t1, t2);
// handle results[0], results[1]
```

`Task.WhenAny` returns the first task that completes; useful for timeouts or racing strategies.

## Creating manual promises: TaskCompletionSource

Wrap callback/event-style APIs as a Task:

```csharp
Task<string> WaitForMessageAsync()
{
    var tcs = new TaskCompletionSource<string>();

    void OnMessage(object s, MessageEventArgs e)
    {
        tcs.TrySetResult(e.Message);
        // unsubscribe event...
    }

    // start or subscribe...
    StartReceiving(OnMessage);
    return tcs.Task;
}
```

`TaskCompletionSource<T>` gives you a promise you can complete manually with `TrySetResult`, `TrySetException`, or `TrySetCanceled`.

## Cancellation and exceptions

Respect cancellation tokens and handle exceptions:

```csharp
async Task<int> LoadWithCancellationAsync(CancellationToken ct)
{
    ct.ThrowIfCancellationRequested();
    try
    {
        // pass token to cancellable operations
        await Task.Delay(5000, ct);
        return 1;
    }
    catch (OperationCanceledException)
    {
        // cleanup if needed
        throw;
    }
}
```

When tasks throw exceptions, `await` rethrows them so you can catch with normal try/catch:

```csharp
try
{
    int v = await GetRemoteCountAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

Note: when multiple tasks fault, the underlying `Task` may expose an `AggregateException`. Using `await` generally rethrows an exception you can catch; to inspect all inner exceptions, check the Task.Exception property.

## Common pitfalls

- Blocking on async code (deadlocks)
  - Avoid `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()` on tasks from a thread with a synchronization context (e.g., UI thread or ASP.NET classic). Example:

    ```csharp
    // BAD: might deadlock on UI thread
    var result = GetRemoteCountAsync().Result;
    ```

    A deadlock can occur when the awaited task tries to resume on the captured context, but the thread is blocked waiting for the result. Fix by making calling code async all the way, or use `ConfigureAwait(false)` inside library code.

- Capturing the SynchronizationContext
  - Use `ConfigureAwait(false)` in library code (not UI code) to avoid resuming on the original context:

    ```csharp
    await SomeIoOperationAsync().ConfigureAwait(false);
    ```

  - In application code where you need to update UI, do not use `ConfigureAwait(false)`.

- Overusing Task.Run
  - `Task.Run` is for CPU-bound work. Don’t wrap I/O-bound async methods with `Task.Run`—it wastes threads.

- Fire-and-forget without handling exceptions
  - If you start tasks and don’t await them, ensure you handle exceptions and lifetime (e.g., keep references, log failures, or use safe wrappers).

## Best practices summary

- Prefer async/await for clarity and proper exception flow.
- "Async all the way": propagate async to callers rather than blocking.
- Use `Task` and `Task<T>` for representing asynchronous operations.
- Use `TaskCompletionSource<T>` to adapt non-task APIs.
- Use `CancellationToken` to allow cooperative cancellation.
- Use `ConfigureAwait(false)` in libraries to avoid unnecessary context capture.
- Use `Task.WhenAll` and `Task.WhenAny` for concurrent task coordination.
- Avoid `.Result`/`.Wait()` on tasks in contexts with SynchronizationContext.

## Short diagnostics checklist (for debugging)
- If code deadlocks: search for `.Result`/`.Wait()` usage and replace with `await`.
- If UI freezes: ensure long-running CPU work uses `Task.Run`.
- If exceptions disappear silently: ensure tasks are awaited or observed (log Task.Exception).

Further reading: async/await patterns, ConfigureAwait behavior, and TaskParallelLibrary (TPL) details.
