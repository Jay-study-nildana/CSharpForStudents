# Day 20 — Async Fundamentals: Problems Set

Instructions: This set contains 10 problems focused on non-blocking I/O, common pitfalls (deadlocks, synchronization-context capture), and best practices in C#/.NET async programming. For coding problems, implement the specified behavior in C#. For conceptual problems, provide a short explanation (2–5 sentences). Each coding problem expects a short, focused solution suitable for classroom review.

Problems

1. Deadlock_BlockingOnResult  
   Description: Demonstrate a minimal example that can deadlock on a UI-like SynchronizationContext when calling an async method with `.Result` or `.Wait()`. Explain why the deadlock occurs and show how to fix it by making the caller async.

2. ConfigureAwait_InLibrary  
   Description: Create a small library-style async method that performs an HTTP request and uses `ConfigureAwait(false)`. Explain where and why `ConfigureAwait(false)` is appropriate and where it should be avoided.

3. TaskRun_ForIO_vsCPU  
   Description: Provide two methods: one that offloads CPU-bound work using `Task.Run`, and one that demonstrates the incorrect use of `Task.Run` wrapping an already asynchronous I/O-bound API. Explain why the second is problematic.

4. TaskCompletionSource_WrapCallback  
   Description: Wrap a callback-based API into a `Task<T>` using `TaskCompletionSource<T>`. Provide a simulation of a callback-style event and show how the `TaskCompletionSource<T>` is completed.

5. FireAndForget_ErrorHandling  
   Description: Implement a safe fire-and-forget helper that launches a background task and logs exceptions without crashing the application. Demonstrate its usage.

6. CancellationToken_Pattern  
   Description: Write an async method that accepts a `CancellationToken`, passes it to an I/O operation, and correctly handles `OperationCanceledException`. Show how the caller cancels the token.

7. WhenAll_WhenAny_CoordinateTasks  
   Description: Create an example that starts three independent remote fetch tasks and uses `Task.WhenAny` to proceed with the fastest result, then uses `Task.WhenAll` to gather all results and handle partial failures (some tasks may fault or cancel).

8. SynchronizationContext_UI_Update  
   Description: Simulate an async operation that updates a "UI" object after an await. Show how capturing the `SynchronizationContext` is necessary for UI update and how `ConfigureAwait(false)` would change behavior.

9. AsyncAllTheWay_API  
   Description: Provide a small Web API-style method (controller action) demonstrating "async all the way" — do not block on async operations. Contrast with a bad synchronous version that blocks and harms throughput.

10. FileIO_AsyncBoundariesDesign  
    Description: Given a small flow: read many small files, process data CPU-bound, and upload results. Sketch the async boundaries: which methods should be async, where to use Task.Run, how to use cancellation, and how to avoid blocking threads. Implement a minimal example that reads files asynchronously, processes on the thread pool, and uploads asynchronously (simulated).

Notes:
- For code problems keep each solution concise (≤ ~80 lines).
- Cover both correct and incorrect patterns where requested.
- Add brief comments explaining the key points (deadlock cause, ConfigureAwait use, Task.Run misuse, etc.).
- Name the solution files to exactly match the problem titles (see filenames provided alongside solutions).

Good luck!