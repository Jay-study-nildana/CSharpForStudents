High-level summary
- Title: C# & .NET Fundamentals — 30 days × 2 hours (in-person)
- Audience: Complete freshers / first-time programmers
- Primary outcome: Students will be able to design, implement, test, and maintain non-web C# applications (console or desktop-level programs) that use good language practices, OOP, collections, LINQ, async I/O, and basic persistence. They will also be comfortable using the .NET toolchain (IDE, debugging, Git, NuGet) and writing unit tests.
- Format per 2-hour session (suggested):
  - 10–15 min recap + learning goals
  - 45–55 min focused concept explanation + demonstration (no required code shown in the plan)
  - 30–35 min guided practice / hands-on exercise (pair or small group)
  - 10–15 min Q&A + assignment & reflection

Prerequisites and instructor setup (verify on Day 1)
- No programming prerequisites; basic computer literacy expected.
- Instructor checklist (students should do this before or during Day 1):
  - Install .NET SDK (stable LTS) and choose an IDE: Visual Studio (Community) or VS Code with C# extension.
  - Install Git and create a GitHub/GitLab account (or local Git usage).
  - Choose a simple database option only if you want persistence beyond files (SQLite recommended) — optional.
  - Install a test runner extension or use built-in test tools in the IDE.
  - Provide course syllabus, capstone brief, grading rubric.

Course themes across 30 days
- Days 1–7: C# language essentials (syntax, types, control flow, methods)
- Days 8–13: OOP in depth (classes, inheritance, interfaces, design principles)
- Days 14–18: Collections, generics, LINQ, and data manipulation
- Days 19–22: Error handling, file I/O, serialization concepts, and debugging
- Days 23–26: Asynchronous programming fundamentals, events/delegates, and delegates/lambdas
- Days 27–29: Testing, code quality, project structure, NuGet and package management, simple architecture and separation of concerns
- Day 30: Capstone presentations, final practical exam, and course wrap-up

Daily plan (each day: objectives / in-class activity / guided exercise / homework)

Day 1 — Orientation & first C# program concepts
- Objectives: Course overview, environment checks, understand what .NET and C# are, program structure (conceptual), compile/run cycle.
- In-class: Walk students through IDE setup, project creation workflow conceptually, run a simple console program demonstration.
- Guided exercise: Students confirm environment setup and describe program parts in words (class, method, namespace, entry point).
- Homework: Write in plain text (or on paper) two small program descriptions they will implement later (e.g., greeting app, simple calculator).

Day 2 — Primitive types, variables, and expressions
- Objectives: Value and reference types, numeric types, boolean, char, string, implicit vs explicit typing, constants, basic operators.
- In-class: Discuss memory basics for value/reference types conceptually and demonstrate type conversion pitfalls.
- Guided exercise: Solve type-conversion and formatting problems on paper; design variable declarations for given tasks.
- Homework: Describe solutions to 8 small problems (math and string tasks), including expected outputs.

Day 3 — Control flow: branching & loops
- Objectives: if/else, switch, for, while, foreach patterns; choosing appropriate control flow for tasks.
- In-class: Compare and contrast loop choices; flowchart sample problems.
- Guided exercise: Work in pairs to pick control flow for sample problems and produce step-by-step pseudocode.
- Homework: Implement (or plan in pseudocode) 4 control-flow tasks and add comments about complexity.

Day 4 — Methods, parameter passing, return values, scope
- Objectives: Modularization via methods, parameter vs return value, overloading conceptually, variable scope and lifetime.
- In-class: Demonstrate decomposition of a problem into methods and discuss single responsibility.
- Guided exercise: Break a medium problem into smaller methods and sketch method signatures (no code required).
- Homework: Provide method-level pseudo-implementations for two problems.

Day 5 — IDE productivity and debugging basics
- Objectives: Using breakpoints, stepping, watches/inspectors, reading stack traces and diagnostics, basic Git workflow (clone, commit, push).
- In-class: Instructor demonstrates debugging workflow and a simple Git commit flow (conceptual).
- Guided exercise: Given a failing program description, students produce a step-by-step debugging plan to find likely causes.
- Assessment: Short formative quiz covering Days 1–5 concepts.
- Homework: Set up a Git repo skeleton (text plan) and prepare first commit message examples.

Day 6 — Introduction to Object-Oriented Programming (OOP)
- Objectives: Classes & objects, fields, properties, constructors, encapsulation and information hiding.
- In-class: Discussion-based examples of modeling real-world entities as classes.
- Guided exercise: Design classes (e.g., Book, Student) with properties and constructor semantics on paper.
- Homework: Write class descriptions and initialization examples in plain text.

Day 7 — Methods in classes, static vs instance members
- Objectives: Instance vs static members, responsibilities in class methods, basic cohesion ideas.
- In-class: Explain use-cases for static helpers vs instance behavior.
- Guided exercise: Convert earlier procedural method sketches into instance methods and plan tests.
- Homework: Create a small UML-like outline (text) showing classes and key methods.

Day 8 — Inheritance and polymorphism
- Objectives: Base and derived classes, virtual methods, overriding, conceptual polymorphism benefits.
- In-class: Demonstrate replacing conditional logic with polymorphic behavior conceptually.
- Guided exercise: Given a conditional-heavy design, sketch a class hierarchy that simplifies it.
- Homework: Short write-up comparing when to use inheritance vs composition.

Day 9 — Interfaces and abstract classes
- Objectives: Interfaces as contracts, abstract classes, multiple-implementation patterns, decoupling designs for testability.
- In-class: Discuss interface-first design and dependency inversion at a high level.
- Guided exercise: Design interfaces for a small domain and outline implementing classes.
- Homework: Define three interfaces for a mini-domain and describe use cases.

Day 10 — Structs, records, and immutability concepts
- Objectives: When to use structs vs classes, immutability benefits, introduction to simple record-like data shapes and tuples conceptually.
- In-class: Performance and semantic differences (value semantics vs reference semantics).
- Guided exercise: Choose types (struct vs class) for given scenarios and explain rationale.
- Homework: Revisit earlier class designs and propose which DTOs (data carriers) should be immutable.

Day 11 — Collections: arrays, lists, dictionaries, sets
- Objectives: Core collection types, choosing the right collection, iteration patterns, complexity trade-offs at a conceptual level.
- In-class: Compare strengths/weaknesses and typical use-cases.
- Guided exercise: Design a data structure for word frequency and for fast lookups; justify choices.
- Homework: Plan collection usage for capstone entities and sketch sample operations.

Day 12 — Generics and type safety
- Objectives: Why generics matter, generic collections and simple generic types, avoiding boxing/unboxing and improving reusability.
- In-class: Examples showing code reuse via generics and safety benefits.
- Guided exercise: Create conceptual generic method signatures for sorting/filtering operations.
- Homework: Rewrite earlier pseudocode to use generics where appropriate (in description).

Day 13 — LINQ fundamentals (declarative data queries)
- Objectives: Declarative querying of collections, projection, filtering, ordering, grouping ideas (conceptual).
- In-class: Map imperative iteration approaches to declarative query equivalents in plain English.
- Guided exercise: Convert several collection-processing tasks to "LINQ-style" descriptions (what each stage does).
- Homework: Prepare three LINQ query descriptions for the capstone dataset.

Day 14 — Exception handling & defensive programming
- Objectives: Try/catch/finally semantics, exception types and custom exceptions concept, validation strategies and fail-fast principles.
- In-class: Error propagation and designing meaningful error messages; when not to use exceptions.
- Guided exercise: Identify failure points in sample scenarios and propose exception-handling strategies.
- Homework: Draft an error-handling policy for the capstone project.

Day 15 — File I/O basics & serialization concepts (JSON/XML)
- Objectives: Reading/writing text and binary files conceptually, structured data formats and serialization principles, when to choose file storage vs DB.
- In-class: Discuss file locking, atomic writes, and data integrity considerations.
- Guided exercise: Design file-based persistence for a simple app and explain backup/restore concepts.
- Assessment: Mid-course quiz covering OOP, collections, LINQ, exceptions, and file I/O.
- Homework: Produce a file layout plan (which entities stored where, format, and sample record description).

Day 16 — Project and solution structure; packages and NuGet
- Objectives: Organizing code into projects/assemblies, separation of concerns (UI/Domain/Data), introduction to NuGet and package management.
- In-class: Show conceptual multi-project layout and how packages are referenced and managed.
- Guided exercise: Sketch a multi-project solution for the capstone (e.g., Core, Infrastructure, CLI).
- Homework: Create a README plan describing solution structure and how to build.

Day 17 — Dependency Injection & inversion of control (concepts)
- Objectives: Why DI improves testability and modularity, constructor injection vs service locators, service lifetimes at a conceptual level.
- In-class: Explain practical examples of decoupling dependencies for unit testing.
- Guided exercise: Identify concrete dependencies in capstone and plan how to invert them.
- Homework: Write short refactoring notes for one capstone class to use constructor injection.

Day 18 — Logging, configuration, and secrets (conceptual)
- Objectives: Structured logging principles, log levels and correlation, configuration sources and secure secrets handling.
- In-class: Discuss configuration vs code and environment-based configuration conceptually.
- Guided exercise: Create a logging & configuration plan for capstone (what to log, log levels, sensitive fields).
- Homework: Create sample log entry descriptions for typical operations and errors.

Day 19 — Delegates, events, and functional basics (lambdas)
- Objectives: Delegates as typed function references, event patterns and publisher/subscriber basics, lambda expression ideas (short anonymous functions).
- In-class: Higher-order function concepts and when to prefer events for decoupling.
- Guided exercise: Design an event-driven notification flow for capstone (what events, who subscribes).
- Homework: Describe two callbacks or event handlers required by the capstone and their responsibilities.

Day 20 — Async programming fundamentals (conceptual)
- Objectives: Synchronous vs asynchronous reasoning, tasks/promises, non-blocking I/O benefits, common pitfalls (deadlocks, capturing context).
- In-class: Real-world analogies for async and when to use asynchronous methods.
- Guided exercise: Sketch async flows for file or remote data operations in capstone and note where await/async would be used (no code).
- Assessment: Quiz covering DI, logging, delegates, and async basics.
- Homework: Update capstone design to mark async boundaries and background processing needs.

Day 21 — Unit testing principles & test doubles
- Objectives: Why unit tests matter, arrange-act-assert pattern, basic mocking concepts and testability design.
- In-class: Show how to structure test projects and typical unit test lifecycles conceptually.
- Guided exercise: Write test case descriptions (plain English) for core business logic functions in capstone.
- Homework: Create test plan with at least 10 unit test cases and indicate where integration tests are needed.

Day 22 — Integration testing basics & test data management
- Objectives: Difference between unit and integration tests, test data isolation, using in-memory stores or test fixtures.
- In-class: Discuss deterministic tests, setup/teardown and seeding approaches conceptually.
- Guided exercise: Design an integration test scenario for a data-access flow in capstone and describe assertions.
- Homework: Prepare a test data plan (sample records and cleanup).

Day 23 — Code quality, naming, and refactoring techniques
- Objectives: Clean code principles, naming conventions, small refactor patterns (extract method, rename), code smells to watch for.
- In-class: Code review exercise using textual examples and suggested improvements.
- Guided exercise: Audit a short pseudo-code snippet for readability and propose refactors (in words).
- Homework: Perform a mini-review of your capstone design and list three refactors to improve clarity.

Day 24 — Performance basics & simple profiling concepts
- Objectives: Time vs space trade-offs, algorithmic thinking, basic profiling approaches and hot-spot identification conceptually.
- In-class: Discuss complexity (big-O) at a high level and common inefficiencies with collections.
- Guided exercise: Analyze capstone operation flows and identify potential performance bottlenecks and mitigations.
- Homework: Add performance considerations to capstone design (caching, batch processing).

Day 25 — Concurrency basics & thread-safety concepts
- Objectives: Shared-state hazards, race conditions, locks vs immutable strategies, when to use concurrency vs async.
- In-class: Discuss common concurrency pitfalls and simple approaches to make code safer.
- Guided exercise: Given a shared resource scenario, propose thread-safety approach for capstone (locks, immutable snapshots, message-queueing).
- Assessment: Quiz on testing, refactoring, performance, and concurrency basics.
- Homework: Document concurrency considerations and chosen strategy for capstone.

Day 26 — Simple architecture & separation of concerns
- Objectives: Layering, responsibility boundaries, interfaces for boundaries, DTO vs domain model separation.
- In-class: Show example diagrams of layered architecture for a console app and discuss pros/cons.
- Guided exercise: Finalize solution architecture for capstone with module responsibilities and boundaries.
- Homework: Final architecture document and a step-by-step plan to implement MVP of capstone.

Day 27 — Packaging, NuGet usage, and builds
- Objectives: Creating and consuming packages (conceptual), build configurations (Debug/Release), assembly versioning basics.
- In-class: Discuss how packages help reuse and how to manage dependencies responsibly.
- Guided exercise: Decide which parts of capstone could become reusable libraries and sketch packaging plan.
- Homework: Draft build and release checklist for capstone (how to produce artifacts and run tests).

Day 28 — Practical coding workshop #1 (implementation sprint)
- Objectives: Apply the fundamentals: implement core domain logic, collections, and tests (hands-on session).
- In-class: Instructor-led hands-on lab with checkpoints; students work in pairs or small groups.
- Guided exercise: Implement core features and write test cases (no instructor-provided code in plan).
- Homework: Continue implementation, commit progress, and prepare a demo plan.

Day 29 — Practical coding workshop #2 (polish, tests, and documentation)
- Objectives: Finish required features, add tests, finalize file persistence and config, and prepare demo.
- In-class: Lab focused on finalizing tests and documentation (README and usage instructions).
- Guided exercise: Run through checklist: tests passing, README completeness, sample data included.
- Homework: Final touches and practice the presentation/demo.

Day 30 — Capstone demos, practical exam, wrap-up, and next steps
- Objectives: Presentations/demos, practical assessment, course reflection, guidance on continuing learning.
- In-class: Each student/group presents and demonstrates their application (timeboxed), instructor & peer feedback, and final grading with rubric.
- Assessment: Final practical exam (deliverable + short oral Q&A about design choices).
- Wrap-up: Provide next-step learning paths and recommended resources.

Capstone project (Console/desktop fundamentals focus)
- Project examples: "Personal Task Manager (CLI)", "Personal Finance Tracker", or "Library Inventory CLI".
- Core (required) features:
  - At least two related domain entities with relations (e.g., Tasks and Categories)
  - Console UI or simple desktop interaction (menus, commands)
  - Persistent storage using files (JSON/CSV) or lightweight DB (SQLite optional)
  - Use of classes, collections, LINQ-style data processing, and async file I/O for heavier operations
  - Unit tests for core business logic (at least 8 meaningful unit tests)
  - Basic logging and error-handling policy
  - README with build/run/test instructions and sample data
- Stretch goals:
  - Implement events/notifications for certain state changes
  - Provide a small reusable library as a NuGet-style project in the solution
  - Add basic concurrency-safe features (e.g., queue processing in background)
- Milestones:
  - Day 11: Entity & collection design approved
  - Day 15: Persistence plan finalized
  - Day 21: Test plan approved
  - Days 28–29: Implementation & polishing
  - Day 30: Demo & submit deliverables

Assessment plan & schedule
- Formative: daily in-class exercises, pair-review, and homework (continuous feedback).
- Quizzes & checks:
  - Day 5 — Syntax & basics quiz
  - Day 15 — Mid-course quiz (OOP, collections, LINQ, I/O)
  - Day 20 — DI / async / logging quiz
  - Day 25 — Testing, performance, concurrency quiz
  - Day 30 — Final practical exam & project demo
- Grading weight suggestion:
  - Homework & daily participation: 25%
  - Quizzes: 20%
  - Mid-course practical tasks: 15%
  - Capstone project (implementation & tests): 35%
  - Final demo & oral exam: 5%

Capstone rubric (example, total 100)
- Core functionality & correctness: 40
- Code quality & design (clean code, separation of concerns): 15
- Tests (adequacy, clarity, and coverage of core logic): 15
- Persistence & data correctness (file layout, integrity, load/save): 10
- Error handling & logging: 10
- Documentation & demo quality (README, usage, sample data): 10

In-class activity examples (fundamentals focused)
- Pseudocode design sprints: plan algorithms and method decomposition before coding.
- Read–explain–refactor: students explain a short code excerpt (provided by instructor separately) and propose refactors in words.
- Test-case writing workshop: convert requirements into unit test cases written in plain English.
- Data-processing thought experiments: plan collection operations and LINQ-style transformations on sample datasets.

Resources (recommended reading & references)
- Microsoft Learn: C# and .NET fundamentals (official docs)
- “C# in a Nutshell” or another beginner-friendly C# book (pick one accessible to your students)
- LINQ and collections guides (MS Docs)
- Intro material on async programming and tasks (MS Docs)
- Unit testing guides for NUnit/xUnit/MSTest (pick a framework to teach)
- Git and source-control beginner guides
- Optional videos: short focused lessons on debugging and testing
- Instructor-provided handouts: daily objective checklist, capstone brief, rubric, and test-case templates

Teaching tips & pacing suggestions
- Slow down early (Days 1–10) to build a strong mental model of types, control flow, and OOP.
- Emphasize design and tests early; having clear unit-test descriptions reduces rework later.
- Use pair work for debugging sessions — this helps novices learn debugging strategies faster.
- Keep practical workshops on Days 28–29 flexible so slower groups can finish.
- If class moves faster, add deeper LINQ problems, mini-algorithmic challenges, or a small GUI/desktop extension.