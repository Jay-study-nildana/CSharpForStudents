What I’ve done: below is a one‑page, student‑facing primer (about 700 words) on the Compile / Run cycle for C# and .NET suitable for Day 1. It explains the steps from source code to execution, key artifacts you’ll see in the IDE or filesystem, common developer commands and options, and practical tips for debugging build vs runtime problems. Next I can produce a short worksheet with 6 guided questions or a checklist with the exact CLI commands to try.

Compile / Run Cycle — one‑page primer

Overview
When you write a C# program, it goes through a predictable pipeline before the computer actually performs the work you described. That pipeline — the compile/run cycle — converts human-readable source code into artifacts the .NET runtime can execute, checks for errors along the way, and provides feedback you use to correct problems. Understanding this cycle helps you find bugs faster and reliably produce runnable programs.

1) Edit (source code)
You start by writing C# source files (.cs). Each file contains declarations of types (classes, structs, interfaces) and methods. In a project, you arrange these files into folders and projects so your code is easier to manage. The text files are the only things you directly edit.

2) Restore dependencies
If your project depends on external libraries (NuGet packages), the first step in the build process is to restore those packages. This downloads the required libraries so the compiler and runtime can reference them. In the IDE this usually happens automatically; from the CLI it’s an explicit restore step.

3) Compile (source → intermediate code)
The C# compiler checks your source code for syntax and type errors. If the code is valid, the compiler translates it into an intermediate language (IL) and produces one or more assemblies (DLLs for libraries, EXEs for applications). Assemblies include IL plus metadata that describes types, members, and referenced assemblies.

Key points about compilation:
- Compilation catches many errors early (syntax mistakes, type mismatches).
- Warnings may be emitted even if compilation succeeds; they flag questionable code.
- Build configurations (Debug vs Release) control optimizations and whether debugging information is generated.

4) Build artifacts and folders
When a project is built, you’ll see generated folders such as obj/ (intermediate files) and bin/ (final assemblies). The bin/ folder holds the assemblies that the runtime loads when you run the program. Debug builds include additional symbol files (PDB) that let debuggers map running code back to source lines.

5) Run (runtime execution)
.NET loads the compiled assembly into the runtime (the Common Language Runtime, CLR). The runtime performs additional verification, the loader resolves dependencies, and code is prepared for execution. Depending on the platform and settings:
- JIT compilation: The runtime compiles IL to native machine code on demand (just‑in‑time) as methods are executed.
- AOT compilation: Some scenarios use ahead‑of‑time compilation to produce a native binary before running.

The runtime provides services during execution: garbage collection (automatic memory management), exception handling, security checks, and type safety enforcement.

6) Debug vs Release runs
- Debug mode: Builds include debug symbols and are optimized for stepping through code, inspecting variables, and setting breakpoints. They are slower but easier to debug.
- Release mode: Compiler optimizations are enabled, resulting in faster, smaller code. Debugging optimized code is harder because the optimizer can rearrange or inline code.

7) Errors you’ll encounter
- Compilation errors: Detected by the compiler (missing semicolons, undefined types, incorrect method signatures). These prevent an assembly from being produced.
- Warnings: Non-blocking issues (deprecated API usage, unreachable code suggestions). Address them to improve code quality.
- Runtime exceptions: Thrown while the program runs (null reference, index out of range, file not found). These must be handled by your code or will crash the program.
- Linking/loader errors: Occur when required assemblies are missing or incompatible (version mismatches).

8) Debugging workflow (practical)
- If compilation fails, read the compiler’s error messages and fix the indicated source lines.
- If the program compiles but misbehaves at runtime, run under the debugger: set breakpoints, step through execution, inspect variables and call stacks, and use watches/logs to pinpoint issues.
- Use logs and clear error messages to diagnose issues that only happen outside the debugger.

9) Running and publishing
- During development you typically run programs directly from the IDE or using a command that builds and runs. This is fast for iterative work.
- When you’re ready to distribute, a publish step packages the application and its dependencies so it can be deployed to another machine or environment.

10) Practical checklist for Day 1
- Confirm: you can create a project in the IDE and see files appear.
- Build: trigger a build and observe any compiler messages.
- Run: execute the program and note the output.
- Locate artifacts: identify the bin/ and obj/ folders and the produced assembly.
- Try a Debug run: set a breakpoint and step through a simple program.

Why this matters
Knowing the compile/run cycle turns confusing errors into actionable steps. If something fails, ask: did it fail at compile time (fix source), at dependency/restore time (fix packages or project file), or at runtime (use debugger/logs)? Mastering this loop is one of the most powerful ways to become effective at developing with C# and .NET.

What I did and what’s next
I prepared this one‑page primer describing each stage of the compile/run cycle and practical checks students can perform on Day 1. Next I can give you a short worksheet with three hands‑on tasks (explore the bin/ folder, reproduce a compiler error, and set a breakpoint) or a one‑page cheat sheet with the exact IDE/CLI commands to run. Which would you like?