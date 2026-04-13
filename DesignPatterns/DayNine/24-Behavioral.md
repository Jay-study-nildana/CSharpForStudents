# Day 9 — Behavioral Patterns: Observer & Mediator (C#)

## Objectives
- Understand the Observer pattern as a publish–subscribe/eventing model in C#.
- Compare push vs pull observer styles and event buses vs direct observer lists.
- Learn the Mediator pattern to centralize and simplify communication among many objects.
- Apply both patterns in a demo and lab: build a notification/event system and a mediator coordinating collaborators.
- Homework: short essay comparing Observer vs Event Aggregator (testing implications).

## Overview
The Observer pattern decouples event producers (subjects) from consumers (observers) so many observers can react to state changes. The Mediator pattern centralizes complex interactions among multiple objects into a single coordinator, reducing coupling and simplifying control flow. Together they help design scalable, testable systems for event-driven UI and workflow coordination.

## Key Lecture Points
- **Push vs Pull observers:**
  - *Push*: subject sends event payload to observers. Simple and efficient when observers need the data immediately.
  - *Pull*: subject notifies observers that "something changed"; observers pull the specific data they need. Useful when observers need different parts of state or when payloads are large.
- **Event buses vs direct observer lists:**
  - *Direct lists*: subject keeps a list of observers and notifies them. Good for small, well-defined relationships.
  - *Event bus / Event aggregator*: a central pub/sub bus decouples producers and consumers more strongly; supports dynamic routing, topic filtering, and cross-cutting concerns (logging, throttling).
- **Mediator to reduce coupling:**
  - When many objects interact in non-trivial ways, a mediator centralizes the coordination logic: components talk to the mediator, not to each other.
  - The mediator becomes the single place for business rules or orchestration, which simplifies components and makes them easier to test.

## Demo Topic (recommended)
- Build a notification/event system (Observer) for application-level events and a Mediator to coordinate UI components (e.g., toolbar, editor, status panel) so they remain loosely coupled while enabling complex workflows.

## Code snippets (C#)

### 1) Push-style Observer (subject pushes payload)
```csharp
// PushObserver.cs
public class SubjectPush<T>
{
    private readonly List<Action<T>> _handlers = new();
    public void Subscribe(Action<T> handler) => _handlers.Add(handler);
    public void Unsubscribe(Action<T> handler) => _handlers.Remove(handler);
    public void Notify(T payload)
    {
        foreach (var h in _handlers) h(payload);
    }
}

// Usage
var subject = new SubjectPush<(string user, string message)>();
subject.Subscribe(p => Console.WriteLine($"Push received: {p.user}: {p.message}"));
subject.Notify(("Alice", "Hello"));
```

### 2) Pull-style Observer (subject signals change; observers pull state)
```csharp
// PullObserver.cs
public class SubjectPull
{
    private readonly List<Action> _observers = new();
    private int _count = 0;
    public void Subscribe(Action observer) => _observers.Add(observer);
    public void Increment()
    {
        _count++;
        Notify();
    }
    public int GetState() => _count;
    private void Notify()
    {
        foreach (var o in _observers) o();
    }
}

// Usage
var subj = new SubjectPull();
subj.Subscribe(() => Console.WriteLine($"Pulled state: {subj.GetState()}"));
subj.Increment(); // observers pull the new state
```

### 3) Event Bus / Aggregator sketch (topic-based)
```csharp
// EventBus.cs
public class EventBus
{
    private readonly Dictionary<string, List<Action<object>>> _topics = new();
    public void On(string topic, Action<object> cb)
    {
        if (!_topics.ContainsKey(topic)) _topics[topic] = new List<Action<object>>();
        _topics[topic].Add(cb);
    }
    public void Off(string topic, Action<object> cb)
    {
        if (_topics.ContainsKey(topic)) _topics[topic].Remove(cb);
    }
    public void Emit(string topic, object payload = null)
    {
        if (_topics.ContainsKey(topic))
            foreach (var cb in _topics[topic]) cb(payload);
    }
}
```

### 4) Mediator coordinating UI components
```csharp
// Mediator.cs
public interface IComponent { void Notify(string @event, object data = null); }

public class UIMediator
{
    private readonly Dictionary<string, IComponent> _components = new();
    public void Register(string name, IComponent comp) => _components[name] = comp;
    public void Send(string sender, string @event, object data = null)
    {
        // Centralized routing: e.g., error -> status panel, save -> toolbar
        if (@event == "save")
            _components["status"]?.Notify("show", new { text = "Saving..." });
        if (@event == "saved")
        {
            _components["toolbar"]?.Notify("enable", "publish");
            _components["status"]?.Notify("show", new { text = "Saved" });
        }
        // ... other rules
    }
}
```
This keeps components simple: they only talk to the mediator, which encodes orchestration logic.

## Lab (in-class exercise)
- **Task 1:** Design a small notification system using either:
  - Direct Observer on a Subject (use push or pull depending on payload size), or
  - An Event Bus for topic routing (subscribe to "user:login", "doc:updated", etc.).
- **Task 2:** Implement a Mediator to coordinate three UI collaborators (Editor, Autosave, StatusBar). Diagram message flows that show:
  - Which component sends which event to the mediator.
  - How the mediator translates and forwards messages to appropriate components.
- **Deliverables:** code (small repo or single file), a sequence diagram (or ASCII/PNG), and a short README describing push/pull choice.

## Design considerations & trade-offs
- **Testing:** Observers are easy to unit test in isolation by stubbing the subject or bus. Mediator centralizes orchestration, which simplifies component tests but requires focused tests for mediator logic (it can grow complex).
- **Debugging:** Event buses add indirection — useful for decoupling, but they can make causal traces harder to follow. Use structured topics and logging to mitigate.
- **Performance:** Push avoids redundant pulls when payloads are small. Pull avoids sending large payloads to observers that don't need them.
- **Scalability:** Event buses scale better across modules and boundaries (e.g., cross-module events). Direct observer lists are simpler and faster for tightly-coupled, known relationships.

## Homework (short essay)
Write a 300–500 word essay comparing Observer vs Event Aggregator (Event Bus). Focus on:
- Advantages and disadvantages of each (decoupling, discoverability, routing, filtering).
- Testing implications: mocking a subject vs mocking a bus; how you verify sequences and side effects.
- When you’d prefer one over the other in real systems (e.g., small component relationships vs application-wide eventing).

## Further reading and pointers
- Gamma et al., “Design Patterns” — Observer section.
- Articles on event-driven architectures and UI/Flux implementations for real-world mediator/event-bus trade-offs.
