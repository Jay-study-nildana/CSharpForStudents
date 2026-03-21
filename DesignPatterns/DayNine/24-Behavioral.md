# Day 9 — Behavioral Patterns: Observer & Mediator

Objectives
- Understand the Observer pattern as a publish–subscribe/eventing model.
- Compare push vs pull observer styles and event buses vs direct observer lists.
- Learn the Mediator pattern to centralize and simplify communication among many objects.
- Apply both patterns in a demo and lab: build a notification/event system and a mediator coordinating collaborators.
- Homework: short essay comparing Observer vs Event Aggregator (testing implications).

Overview
The Observer pattern decouples event producers (subjects) from consumers (observers) so many observers can react to state changes. The Mediator pattern centralizes complex interactions among multiple objects into a single coordinator, reducing coupling and simplifying control flow. Together they help design scalable, testable systems for event-driven UI and workflow coordination.

Key Lecture Points
- Push vs Pull observers:
  - Push: subject sends event payload to observers. Simple and efficient when observers need the data immediately.
  - Pull: subject notifies observers that "something changed"; observers pull the specific data they need. Useful when observers need different parts of state or when payloads are large.
- Event buses vs direct observer lists:
  - Direct lists: subject keeps a list of observers and notifies them. Good for small, well-defined relationships.
  - Event bus / Event aggregator: a central pub/sub bus decouples producers and consumers more strongly; supports dynamic routing, topic filtering, and cross-cutting concerns (logging, throttling).
- Mediator to reduce coupling:
  - When many objects interact in non-trivial ways, a mediator centralizes the coordination logic: components talk to the mediator, not to each other.
  - The mediator becomes the single place for business rules or orchestration, which simplifies components and makes them easier to test.

Demo Topic (recommended)
- Build a notification/event system (Observer) for application-level events and a Mediator to coordinate UI components (e.g., toolbar, editor, status panel) so they remain loosely coupled while enabling complex workflows.

Code snippets
Below are compact examples in TypeScript to illustrate push vs pull observers and a simple Mediator coordinating UI components.

1) Push-style Observer (subject pushes payload)
```typescript
// PushObserver.ts
type Handler<T> = (payload: T) => void;

class SubjectPush<T> {
  private handlers: Handler<T>[] = [];
  subscribe(h: Handler<T>) { this.handlers.push(h); }
  unsubscribe(h: Handler<T>) {
    this.handlers = this.handlers.filter(x => x !== h);
  }
  notify(payload: T) {
    for (const h of this.handlers) h(payload);
  }
}

// Usage
const subject = new SubjectPush<{user: string, message: string}>();
subject.subscribe(p => console.log(`Push received: ${p.user}: ${p.message}`));
subject.notify({ user: 'Alice', message: 'Hello' });
```

2) Pull-style Observer (subject signals change; observers pull state)
```typescript
// PullObserver.ts
type Observer = () => void;

class SubjectPull {
  private observers: Observer[] = [];
  private state: { count: number } = { count: 0 };

  subscribe(o: Observer) { this.observers.push(o); }
  increment() {
    this.state.count++;
    this.notify();
  }
  getState() { return this.state; }

  private notify() {
    for (const o of this.observers) o();
  }
}

// Usage
const subj = new SubjectPull();
subj.subscribe(() => {
  const s = subj.getState();
  console.log(`Pulled state: ${s.count}`);
});
subj.increment(); // observers pull the new state
```

3) Event Bus / Aggregator sketch (topic-based)
```typescript
// EventBus.ts
type Callback = (payload?: any) => void;

class EventBus {
  private topics = new Map<string, Callback[]>();

  on(topic: string, cb: Callback) {
    const list = this.topics.get(topic) ?? [];
    list.push(cb);
    this.topics.set(topic, list);
  }

  off(topic: string, cb: Callback) {
    const list = this.topics.get(topic) ?? [];
    this.topics.set(topic, list.filter(x => x !== cb));
  }

  emit(topic: string, payload?: any) {
    for (const cb of this.topics.get(topic) ?? []) cb(payload);
  }
}
```

4) Mediator coordinating UI components
```typescript
// Mediator.ts
interface Component { notify(event: string, data?: any): void; }

class UIMediator {
  private components = new Map<string, Component>();

  register(name: string, comp: Component) {
    this.components.set(name, comp);
  }

  send(sender: string, event: string, data?: any) {
    // Centralized routing: e.g., error -> status panel, save -> toolbar
    if (event === 'save') {
      this.components.get('status')?.notify('show', { text: 'Saving...' });
    }
    if (event === 'saved') {
      this.components.get('toolbar')?.notify('enable', 'publish');
      this.components.get('status')?.notify('show', { text: 'Saved' });
    }
    // ... other rules
  }
}
```
This keeps components simple: they only talk to the mediator, which encodes orchestration logic.

Lab (in-class exercise)
- Task 1: Design a small notification system using either:
  - Direct Observer on a Subject (use push or pull depending on payload size), or
  - An Event Bus for topic routing (subscribe to "user:login", "doc:updated", etc.).
- Task 2: Implement a Mediator to coordinate three UI collaborators (Editor, Autosave, StatusBar). Diagram message flows that show:
  - Which component sends which event to the mediator.
  - How the mediator translates and forwards messages to appropriate components.
- Deliverables: code (small repo or single file), a sequence diagram (or ASCII/PNG), and a short README describing push/pull choice.

Design considerations & trade-offs
- Testing: Observers are easy to unit test in isolation by stubbing the subject or bus. Mediator centralizes orchestration, which simplifies component tests but requires focused tests for mediator logic (it can grow complex).
- Debugging: Event buses add indirection — useful for decoupling, but they can make causal traces harder to follow. Use structured topics and logging to mitigate.
- Performance: Push avoids redundant pulls when payloads are small. Pull avoids sending large payloads to observers that don't need them.
- Scalability: Event buses scale better across modules and boundaries (e.g., cross-module events). Direct observer lists are simpler and faster for tightly-coupled, known relationships.

Homework (short essay)
Write a 300–500 word essay comparing Observer vs Event Aggregator (Event Bus). Focus on:
- Advantages and disadvantages of each (decoupling, discoverability, routing, filtering).
- Testing implications: mocking a subject vs mocking a bus; how you verify sequences and side effects.
- When you’d prefer one over the other in real systems (e.g., small component relationships vs application-wide eventing).

Further reading and pointers
- Gamma et al., “Design Patterns” — Observer section.
- Articles on event-driven architectures and UI/Flux implementations for real-world mediator/event-bus trade-offs.

---
What I did: I composed a one-page Markdown document (1000–1500 words) that includes objectives, lecture points, demo/lab/homework, design notes, and code snippets (push/pull observer examples, event bus, mediator). What's next: I can adjust tone/length to fit your syllabus, convert examples to another language (Python/Java), or produce an accompanying sequence diagram (ASCII or PNG). Which would you like me to do next?