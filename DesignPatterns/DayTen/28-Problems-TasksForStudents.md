# Day 10 — Behavioral: Command & Chain of Responsibility — Problems (C# / .NET)

Instructions
- Implement each problem in idiomatic C# as a single `.cs` file that compiles and runs as a minimal console demo.
- Name each solution file exactly as the problem title below with a `.cs` extension.
- Keep examples concise and focused; include a Main() method that demonstrates expected behavior.
- Prefer .NET built-ins (delegates, Task, System.Text.Json, etc.) and avoid external packages.

Problems
1. 01_BasicCommand
   - Implement a basic ICommand interface with Execute() and Unexecute().
   - Provide a Receiver (e.g., Document) and a concrete InsertTextCommand.
   - Demo: execute command and undo it, printing document state.

2. 02_UndoRedoCommandManager
   - Implement a CommandManager that supports ExecuteCommand, Undo, and Redo.
   - Use two stacks for undo/redo and show clearing redo on new Execute.
   - Demo: run a sequence of commands and show undo/redo transitions.

3. 03_CommandSerializationAndReplay
   - Design DTO-friendly command serialization (JSON) and a factory to rehydrate commands.
   - Demo: serialize a command, deserialize, replay it, and show state.

4. 04_CompositeMacroCommand
   - Implement a CompositeCommand (Macro) that aggregates multiple ICommand instances.
   - Ensure composite supports Undo (reverse-order) and is atomic from the invoker point-of-view.
   - Demo: compose several primitive commands and undo the macro.

5. 05_NonReversibleAndSafeUndo
   - Implement a non-reversible command (e.g., SendEmailCommand) and make CommandManager handle it safely.
   - Requirements: manager should detect lack of Unexecute and prevent or flag undo.
   - Demo: attempt undo and show safe behavior.

6. 06_CommandQueueAndWorker
   - Implement a persistent (in-memory) command queue and a worker that dequeues and executes commands.
   - Show queuing, worker processing, and simple retry on transient failure.
   - Demo: enqueue several commands and process them.

7. 07_ChainOfResponsibilityBasic
   - Implement a classic CoR with Handler base and concrete handlers for Validation, Authorization, and Processing.
   - Each handler can handle or pass along; return a result indicating handled/success/failure.
   - Demo: show a successful request and one that is rejected by validation.

8. 08_ChainShortCircuitAndLogging
   - Extend CoR to support short-circuiting and a logging handler that records which handlers ran.
   - Provide a test/demo that proves short-circuit stops later handlers from running.

9. 09_ChainWithAsyncHandlers
   - Implement a CoR where handlers can be async (Task-based).
   - Design chain execution to await each handler and short-circuit properly on handled/failure.
   - Demo: include an async handler that simulates I/O delay.

10. 10_CommandThroughHandlerChain
    - Combine Command and CoR: create commands that are routed through a handler chain for validation and processing before final execution.
    - Requirements: handlers may reject commands (short-circuit) before the command executes; show undo when command executed successfully.
    - Demo: attempt to run a command that is rejected and one that succeeds (with undo).

Hints & Grading
- Favor explicit, testable behavior (clear Console output demonstrating transitions).
- For serialization use System.Text.Json and DTOs.
- For async chains use async/await and Task.Delay to simulate I/O.
- Keep code self-contained and avoid external dependencies.