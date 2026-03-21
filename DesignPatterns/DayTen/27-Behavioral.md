# Day 10 — Behavioral Patterns: Command & Chain of Responsibility (C# / .NET)

Objectives
- Understand Command: encapsulate requests as objects to support undo/redo, queuing, logging, and (optionally) serialization.
- Understand Chain of Responsibility (CoR): build a chain of handlers where each may handle, transform, or pass a request.
- Learn practical trade-offs: commands as first-class objects vs function invocations; CoR vs pipeline/filter patterns.
- Demo & Lab: implement a command history with undo/redo and a processing chain where handlers can short-circuit.

Overview
The Command pattern turns actions into objects (commands) with an Execute method and — when needed — an Unexecute (or Undo) method. Commands make it easy to queue, log, serialize, and support undo/redo stacks. The Chain of Responsibility lets multiple handlers attempt to process a request; handlers can stop propagation (short-circuit) or pass it onward, enabling flexible validation/processing pipelines.

Lecture points (concise)
- Commands as first-class objects:
  - Define an ICommand with Execute(); add Undo()/Unexecute() when reversible.
  - Invoker (caller) holds and invokes commands; Receiver performs actual work.
  - Command objects can carry full context and be serialized for replay.
- Undo/redo:
  - Maintain two stacks: undoStack and redoStack. Execute pushes to undoStack and clears redoStack. Undo pops to redoStack and calls Unexecute.
- Serialization:
  - To serialize commands reliably, keep command state primitive or DTO-based; prefer explicit serialization-friendly DTOs rather than capturing live object references.
- Chain of Responsibility vs pipeline/filter:
  - CoR handlers receive a request and decide to handle or pass it. Useful for validation chains and flexible processing.
  - Pipeline/filter is often explicit and deterministic sequence; CoR is more dynamic and allows handlers to opt-in/out or short-circuit.
- Testing implications:
  - Commands are easy to unit-test by mocking receivers or asserting side effects.
  - CoR requires tests for ordering, short-circuiting, and interactions between handlers.

Code snippets (C#)

1) Basic Command with undo/redo
```csharp
// ICommand.cs
public interface ICommand
{
    void Execute();
    void Unexecute(); // For undo; can throw or be a no-op if not reversible
    string Description { get; }
}

// Receiver: performs work
public class Document
{
    public string Text { get; private set; } = "";
    public void Insert(int index, string text) {
        Text = Text.Insert(index, text);
    }
    public void Remove(int index, int length) {
        Text = Text.Remove(index, length);
    }
}

// Concrete Command
public class InsertTextCommand : ICommand
{
    private readonly Document _doc;
    private readonly int _index;
    private readonly string _text;
    public string Description => $"Insert '{_text}' at {_index}";
    public InsertTextCommand(Document doc, int index, string text) {
        _doc = doc; _index = index; _text = text;
    }
    public void Execute() => _doc.Insert(_index, _text);
    public void Unexecute() => _doc.Remove(_index, _text.Length);
}

// Invoker with undo/redo
public class CommandManager
{
    private readonly Stack<ICommand> _undo = new();
    private readonly Stack<ICommand> _redo = new();

    public void ExecuteCommand(ICommand cmd) {
        cmd.Execute();
        _undo.Push(cmd);
        _redo.Clear();
    }

    public void Undo() {
        if (_undo.Count == 0) return;
        var cmd = _undo.Pop();
        cmd.Unexecute();
        _redo.Push(cmd);
    }

    public void Redo() {
        if (_redo.Count == 0) return;
        var cmd = _redo.Pop();
        cmd.Execute();
        _undo.Push(cmd);
    }
}
```

Notes on serialization
- To serialize commands, design them as DTOs: store primitive properties (indexes, strings, enum names), not complex live references.
- Provide a command factory to rehydrate a DTO into a concrete ICommand at replay time. Example: store { commandType: "InsertText", index: 3, text: "hi" }.

2) Chain of Responsibility (validation/processing chain)
```csharp
// Request object
public class Request { public string Payload { get; set; } }

// Handler base
public abstract class Handler
{
    protected Handler? Next { get; private set; }
    public Handler SetNext(Handler next) { Next = next; return next; }

    // Return true if handled and stop chain; false to continue
    public abstract bool Handle(Request req);
}

// Concrete handlers
public class ValidationHandler : Handler
{
    public override bool Handle(Request req) {
        if (string.IsNullOrWhiteSpace(req.Payload)) {
            Console.WriteLine("Validation failed: empty payload. Short-circuiting.");
            return true; // handled (rejected)
        }
        return Next?.Handle(req) ?? false;
    }
}

public class LoggingHandler : Handler
{
    public override bool Handle(Request req) {
        Console.WriteLine($"Logging payload: {req.Payload}");
        return Next?.Handle(req) ?? false;
    }
}

public class ProcessingHandler : Handler
{
    public override bool Handle(Request req) {
        Console.WriteLine($"Processing: {req.Payload}");
        return true; // handled successfully, stop chain
    }
}

// Build and use
var validator = new ValidationHandler();
var logger = new LoggingHandler();
var processor = new ProcessingHandler();
validator.SetNext(logger).SetNext(processor);

var request = new Request { Payload = "Hello" };
validator.Handle(request);
```

CoR vs pipeline/filter
- CoR often returns whether it handled the request; sequence may be dynamic and handlers can short-circuit out.
- Pipeline/filter tends to be an explicit ordered sequence where each stage transforms or augments data; usually all stages run.

Demo & Lab suggestions
- Demo 1: Simple editor with CommandManager. Show insert/undo/redo operations and print document text each step.
- Demo 2: Validation-processing chain. Show successful processing and short-circuit on invalid input.
- Lab: Implement a serializable command (DTO + factory) and a handler chain with a handler that conditionally forwards to external async processing. Provide unit tests that assert undo/redo behavior and that invalid requests are short-circuited.

Testing tips
- Commands: mock or stub Receivers to assert Execute/Unexecute calls and state transitions. Test undo/redo sequences (execute A, execute B, undo -> expect A's state, redo -> expect B's state).
- CoR: unit-test handler order and short-circuit paths. Replace Next handlers with test doubles to observe whether they were called.

Homework
- Produce unit tests that:
  1) Execute a sequence of commands (A then B), undo twice, redo once — assert document state at each step.
  2) Build a handler chain with at least one validator, one logger, and a processing handler; test both success and validation-failure short-circuit scenarios.

Further reading
- Gamma et al., Design Patterns — Command and Chain of Responsibility.
- Articles on Persistent Command Logs and CQRS for practical command serialization patterns.
