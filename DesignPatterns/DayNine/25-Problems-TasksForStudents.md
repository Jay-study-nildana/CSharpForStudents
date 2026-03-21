# Day 9 — Behavioral: Observer & Mediator — Problems (C# / .NET)

Instructions
- Solve each problem using idiomatic C#/.NET (console apps or small libraries are fine).
- Each problem title below is the required filename prefix for its solution file (append `.cs`).
- Aim for clean, testable code and a short demo in `Main()` showing expected behavior.
- Comment code where helpful and keep examples minimal but clear.

Problems
1. 01_PushObserverNotification
   - Implement a push-style Observer (Subject pushes typed payload to subscribers).
   - Requirements: generic `Subject<T>` with `Subscribe`, `Unsubscribe`, `Notify`.
   - Demo: publish a notification object containing `User` and `Message`.

2. 02_PullObserverState
   - Implement a pull-style Observer where observers are signalled and then pull state.
   - Requirements: `Subject` with `Subscribe(Action)` and `GetState()`; state should be an object representing a counter or similar.
   - Demo: increment state and show observers pulling the latest state.

3. 03_EventBusWithTopics
   - Build a topic-based EventBus (Event Aggregator).
   - Requirements: subscribe/unsubscribe by topic string, publish with optional payload.
   - Demo: two topics and multiple subscribers receiving topic-specific events.

4. 04_SubscriberFiltering
   - Extend an EventBus to support predicate-based filtering per subscriber.
   - Requirements: subscriber can provide a Func<object, bool> filter; only matching payloads are delivered.
   - Demo: publish mixed payloads and show only filtered subscribers receive appropriate ones.

5. 05_WeakReferenceObservers
   - Implement observer storage using weak references to avoid memory leaks when subscribers are not explicitly unsubscribed.
   - Requirements: a `WeakSubject<T>` that holds weak refs and auto-cleans dead subscribers.
   - Demo: demonstrate a subscriber being garbage-collected and no longer receiving notifications.

6. 06_MediatorForUIComponents
   - Implement a Mediator for UI-like components: `Editor`, `Toolbar`, `StatusBar`.
   - Requirements: components register with a `UIMediator`; components call `mediator.Send(...)` and mediator routes messages and orchestrates UI updates.
   - Demo: simulate `save` -> mediator shows "Saving..." on status, then `saved` -> enable toolbar button and show "Saved".

7. 07_MediatorWorkflowOrchestrator
   - Build a Mediator that orchestrates a multi-step workflow across collaborators (`Validator`, `Processor`, `Notifier`).
   - Requirements: mediator enforces order and handles error routing.
   - Demo: run workflow with success and failure path.

8. 08_ObservableCollectionCustom
   - Implement a minimal `ObservableCollection<T>` that raises `CollectionChanged` events (add/remove).
   - Requirements: event args should include action and item; include thread-safety for changes.
   - Demo: subscribe to collection changes and add/remove items.

9. 09_EventAggregatorWithTests
   - Implement an Event Aggregator and include a small set of assertion-style checks (no external test runner required).
   - Requirements: publish/subscribe and a `TestSimulator` that asserts expected sequences (throw `InvalidOperationException` on failure).
   - Demo: simulate publisher raising events and validate order and payload.

10. 10_DecoupledComponentsUsingMediator
    - Design a mediator-driven example where components are completely decoupled (no direct references).
    - Requirements: components register with mediator only; mediator contains rules for complex scenarios (e.g., save -> autosave + status + telemetry).
    - Demo: show components reacting via mediator without referencing each other.

Hints & Constraints
- Use .NET standard library types (delegates, EventHandler<T>, WeakReference) where appropriate.
- Keep each solution contained in one `.cs` file and runnable as a small console demo.
- Prioritize clarity and correctness over advanced abstractions.
- For weak reference behavior, include comments describing how to force or demonstrate garbage collection in a deterministic way for the demo.

Grading Criteria (suggested)
- Correct use of push vs pull semantics where required.
- Clear mediator that centralizes orchestration and reduces coupling.
- Demonstrated handling of subscription lifecycles and memory-safety for weak observers.
- Simple, reproducible demos for each problem.

Deliverables
- One `.md` (this file) describing the problems.
- Ten `.cs` solution files named exactly as the problem titles above with `.cs` extension.