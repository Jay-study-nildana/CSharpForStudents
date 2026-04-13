12-Day Curriculum (2 hours per session)

Day 1 — Introduction, Patterns vocabulary, SOLID and DI foundations
- Objectives: What patterns are and why they matter; pattern anatomy (intent, problem, solution, tradeoffs); short UML primer; SOLID principles recap; Dependency Inversion and Dependency Injection concepts and lifetime choices.
- Lecture points: pattern catalog types (creational/structural/behavioral), how to choose a pattern, anti-patterns to avoid.
- Demo topic: identify coupling in a small example and show a high-level refactor to use DI and interfaces.
- Lab: examine a simple, tightly-coupled app; create interfaces and rewire high-level dependencies conceptually (class diagrams + pseudo-steps).
- Homework: read two short articles on SOLID & DI; prepare questions about dependency lifetimes.

Day 2 — Creational: Singleton (with caveats) & Factory Method
- Objectives: When to use/avoid Singleton; thread-safety/lifetime alternatives; factory method intent and use for polymorphic creation.
- Lecture points: Singleton pitfalls, DI-managed singletons; factory method vs direct constructor calls vs factory object.
- Demo topic: choosing between creating objects directly, factories, and DI/resolution in an app.
- Lab: design a factory method for creating interchangeable service implementations; evaluate if Singleton is appropriate and propose alternatives.
- Homework: short write-up comparing factory method and DI for a given use case.

Day 3 — Creational: Abstract Factory & Builder
- Objectives: Abstract Factory for families of related objects; Builder for constructing complex objects step-by-step (immutable construction, fluent APIs).
- Lecture points: use cases for each, tradeoffs, mapping to configuration/plug-in scenarios.
- Demo topic: high-level design of an Abstract Factory for platform-specific components and using a Builder to compose a complex DTO/domain object.
- Lab: design an Abstract Factory interface and a Builder API for a sample domain; sketch UML and usage flow.
- Homework: prepare a short example scenario where Builder improves readability/maintainability.

Day 4 — Data access patterns: Repository & Unit of Work
- Objectives: Decouple domain logic from data access via Repository; coordinate multiple repositories and transaction boundaries with Unit of Work.
- Lecture points: repository interface design, query vs persistence responsibilities, mapping to ORMs/EF, testing with mocks.
- Demo topic: refactor direct DAL calls into repositories and add a Unit of Work for transactional operations.
- Lab: define repository interfaces for a simple domain and design a unit-of-work contract; create test scenarios that would verify behavior.
- Homework: compare Repository+UoW vs direct ORM usage in small pros/cons list.

Day 5 — Structural: Adapter & Facade
- Objectives: Adapter to bridge incompatible interfaces; Facade to simplify and unify complex subsystems behind a simple API.
- Lecture points: when to adapt, when to wrap with a facade, layering and separation of concerns.
- Demo topic: conceptual adapter for a third-party API and a facade that aggregates multiple subsystem calls into one operation.
- Lab: design an adapter for a legacy service and sketch a facade for combining logging, metrics, and notification calls.
- Homework: write a one-page justification when you’d prefer a facade over direct subsystem access.

Day 6 — Structural: Decorator & Proxy
- Objectives: Decorator to add responsibilities dynamically; Proxy for controlling access or lazy-initialization of resources.
- Lecture points: composition vs inheritance, stacking decorators, differences between decorator and proxy patterns.
- Demo topic: high-level decoration of a service (e.g., caching, logging, validation) and proxy for remote/expensive resource access.
- Lab: create a decorator chain design for cross-cutting concerns and design a proxy that enforces access policy or lazy loads a heavy object.
- Homework: list pros/cons of decorator chains for cross-cutting concerns vs AOP frameworks.

Day 7 — Structural: Composite & Bridge
- Objectives: Composite for tree-like structures (uniform treatment of leaf and composite nodes); Bridge to decouple abstraction from implementation to allow independent variation.
- Lecture points: traversal and operations in composite, separation of abstraction and implementation in Bridge.
- Demo topic: menu/tree UI example with Composite; rendering abstraction separated from concrete renderers using Bridge.
- Lab: design a composite for a hierarchical domain (e.g., document sections or UI components) and sketch a bridge for multiple platform implementations.
- Homework: draw UML for both designs and describe how adding a feature would affect each.

Day 8 — Behavioral: Strategy & Template Method
- Objectives: Strategy for interchangeable algorithms/behaviors; Template Method for invariant algorithm skeleton with overridable steps.
- Lecture points: when to choose Strategy (runtime swap) vs Template Method (inheritance-based reuse), parameterization vs subclassing tradeoffs.
- Demo topic: strategy for choosing algorithms (e.g., pricing/discount), template method for multi-step processing pipelines.
- Lab: design several strategies for a behavior and a template-method base class for a domain process; create use-case diagrams.
- Homework: propose an example where Strategy enables A/B testing of algorithms.

Day 9 — Behavioral: Observer & Mediator
- Objectives: Observer for publish–subscribe / eventing models; Mediator to centralize and simplify communication among many objects.
- Lecture points: push vs pull observers, event buses vs direct observer lists, mediator to reduce coupling in complex interaction.
- Demo topic: notification/event system design and mediator for coordinating UI components or workflow steps.
- Lab: design an observer-based notification system and a mediator to coordinate a set of collaborators; diagram message flows.
- Homework: short essay: Observer vs Event Aggregator (advantages and testing implications).

Day 10 — Behavioral: Command & Chain of Responsibility
- Objectives: Command to encapsulate requests, support undo/redo, queueing; Chain of Responsibility to allow a request to be handled by different handlers in sequence.
- Lecture points: commands as first-class objects, serialization of commands, CoR vs pipeline/filter patterns.
- Demo topic: command history/undo stack and a validation/processing chain where each handler can short-circuit or pass along.
- Lab: design a command interface and a handler chain for request processing; produce test scenarios to validate flow and undo behavior.
- Homework: prepare test cases demonstrating undo/redo and chain short-circuit behavior.

Day 11 — Integration/refactor lab (apply multiple patterns)
- Objectives: Apply multiple learned patterns to improve a legacy-style codebase (or instructor-provided scenario).
- Session plan: quick instructor demo on spotting pattern candidates; teams pick an area to refactor and produce design notes (DI, Repository, Factory/Builder, Strategy, Observer, etc.).
- Lab: teams refactor/design (conceptual + incremental code steps you’ll implement later) and prepare short demo notes and design rationale.
- Deliverable: design doc + UML + test plan showing applied patterns and intended benefits.
- Homework: finalize team design doc and list mapping of each chosen pattern to a specific problem.

Day 12 — Presentations, review, assessment, next steps
- Objectives: Present team solutions, critique pattern usage, consolidate learning, and map to real-world .NET architecture scenarios.
- Session plan: each team presents (10–12 min) their design choices and tradeoffs; instructor and peers Q&A; final review of pattern selection heuristics.
- Assessment: rubric-driven feedback (correctness of intent, justification for patterns, testability, decoupling achieved, clarity of design).
- Wrap-up: recommended further resources, suggested follow-up projects (e.g., plugin architecture, microservice patterns), and guidance on implementing patterns in production .NET code.

Assessment & Grading (suggested)
- Daily lab checklist (pass/fail or points): Did the student identify the problem, propose an appropriate pattern, produce UML, and write tests or test plans?  
- Mid-course refactor (Day 11 design doc): scored on design clarity, correctness, and pattern justification.  
- Final presentation (Day 12): scored on rationale, demonstration of tradeoffs, and ability to answer questions.  
- Optional: automated unit tests for labs when you’re ready to add code.

