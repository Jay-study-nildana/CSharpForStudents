# Program structure (conceptual)

What a program is

A program is an organized collection of code and resources that the computer runs to perform tasks. In C# and .NET, a program is built from smaller pieces — files, types (classes or other constructs), methods, and configuration — that are organized into projects and assemblies. Conceptually, think of a program as a set of responsibilities (what it must do) and the parts that carry out those responsibilities.

Files, projects, and solutions

- File: A single text file contains code for one or more types. Files are the basic unit you edit.
- Project: A project groups related files and settings to build a single output (an executable program or a library). The project declares which files to include, which libraries it depends on, and how to build.
- Solution: A solution collects multiple projects that together form a larger application (for example, one project for core logic, another for tests, another for utilities).
In an IDE you will see these as a tree: solution → projects → folders → files.

Assemblies and outputs

When you build a project, the compiler turns the code into an assembly (a DLL for libraries or an EXE for applications). Assemblies are the deployable units that the .NET runtime executes or loads. The build process also produces support artifacts like intermediate files and debug symbols that help during testing and debugging.

Types and members (the building blocks)

- Types: The main reusable units in C# are types—commonly classes, but also structs, enums, records, and interfaces. A type models a concept or a role in your program (for example, a Book, a Calculator, or a Logger).
- Members: Types contain members—fields (data), properties (named accessors for data), constructors (initialization), methods (behaviors/actions), and events. Members define what the type knows and what it can do.
- Access modifiers: Members are declared with access levels (public, private, etc.) that control visibility; this enforces encapsulation—keeping internal details hidden and exposing a clear API.

Entry point and execution flow

Every executable program has a starting place—an entry point. When you run the program, control passes to that entry point and the runtime executes the sequence of instructions from there. Conceptually, the entry point sets up initial state, coordinates high-level tasks (for example, parsing input, invoking core logic), and triggers program shutdown. Programs may also have background or helper tasks that run during their lifetime.

Separation of concerns and structure

Good programs separate responsibilities into layers or modules:
- Presentation/interaction layer: accepts input and presents results (console I/O in a fundamentals class).
- Domain/core logic: contains the business rules and main algorithms.
- Data access/persistence: handles saving and loading data (files, if used in this course).

Each layer should depend on abstractions (interfaces) rather than concrete implementations where possible; this makes code easier to test and change.

Dependencies and packages

Programs commonly use external libraries to avoid reinventing common features. In .NET these are managed as packages (NuGet). A project lists its package dependencies so the build system can fetch and include them automatically.

Configuration and environment

Applications often read configuration (settings) from files or environment variables. Conceptually, separate configuration from code so the same program can run in different contexts without changing code.

Build, run, and debug cycle

- Build: The compiler checks and translates code into an assembly. Compilation catches many errors early.
- Run: The runtime loads the assembly and executes the entry point.
- Debug: Use the debugger to step through execution, inspect variables, and find logic errors. Debugging is a key skill—most learning happens by running and inspecting programs.

Practical things to spot in the IDE (Day‑1 checklist)

- Solution explorer: shows solution → projects → files. Locate the project that produces an executable.
- Entry point: identify which project is an application and where execution starts.
- Project settings: see dependencies, build output path, and frameworks targeted.
- Output folders: built assemblies go into bin/ and temporary files into obj/.
- Test project (if present): separate project for unit tests that exercise core logic.

Why structure matters

A clear structure makes programs easier to read, test, and modify. It reduces accidental coupling between unrelated parts and helps teams work together. For beginners, practicing small, well‑structured programs builds habits that scale as projects grow.

Next step

Use this conceptual map as you explore your first console project: identify files, types, the entry point, and which parts handle input, core logic, and data storage. If you’d like, I can now produce a one‑page worksheet with guided questions to help students identify these elements in a sample project.