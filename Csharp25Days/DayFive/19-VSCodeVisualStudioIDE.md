# Day 5 — Debugging & Basic Git Workflow (C# / .NET in Visual Studio and VS Code)

Objectives: Learn practical debugging techniques (breakpoints, stepping, watches/inspectors, stack traces, diagnostics) and the basic Git flow (clone → commit → push) used in class.

This guide gives a compact, hands‑on workflow you can follow when a program fails and a few command examples to use in your terminal or IDE.

---

## Quick debugging workflow (high level)
1. Reproduce the problem with a simple, minimal input.
2. Run under the debugger and set a breakpoint near the suspected area.
3. Step through the code, inspect variables (Locals/Autos/Watch).
4. Read the call stack and any exceptions/stack traces.
5. Add logging or diagnostics if the behavior depends on timing or environment.
6. Fix, write a small test, commit the change, and push.

---

## Breakpoints — the basic tool
- Breakpoints pause execution at a line so you can inspect program state.
- Types of breakpoints:
  - Standard breakpoint: pause on a source line.
  - Conditional breakpoint: pause only when an expression is true.
  - Hit-count breakpoint: pause after N hits (useful in loops).
  - Exception breakpoint / "break on thrown": pause when a specific exception is thrown.

Visual Studio: Click the left gutter or press F9. Right-click a red dot to set conditions or hit count.  
VS Code: Click the gutter or press F9. Use the Breakpoints section in the Run view to add conditions.

Example conditional breakpoint in VS/VSCode:
- Condition: `userId == 0` — stops only when `userId` is zero.
- Hit count: `>= 100` — stops on the 100th hit.

---

## Stepping — moving through code
- Step Into (F11): go into the called method.
- Step Over (F10): run the method as a single unit and stop at the next line.
- Step Out (Shift+F11): finish current method and return to caller.
- Continue / Resume (F5): run until next breakpoint or program end.
- Run to Cursor (right-click line → Run to Cursor): useful for quick jumps.

Tip: Use Step Over to avoid stepping into framework code unless you need to. Use Step Into for your own methods or to inspect recursion.

---

## Watches & Inspectors — see values and expressions
- Locals/Autos: show local variables and values in the current scope.
- Watch window: add any expression (e.g., `items.Count`, `user.Name`) to evaluate constantly.
- Immediate/Debug Console: evaluate expressions, call methods, or change variables at runtime. Example: in Immediate window you can run `user.Name = "Test"` to mutate state (use with care).
- Tooltip hover: hover over a variable to see its current value.

Example use:
- Add `result.Count` to Watch to monitor size of a collection as you step through a loop.
- Use Immediate window to call helper functions that print structured state.

---

## Reading stack traces & call stacks
When an exception is thrown, the stack trace tells you the chain of method calls that led to the error. Learn to read it from top (most recent frame) to bottom (entry point).

Example stack trace:
System.NullReferenceException: Object reference not set to an instance of an object.
   at MyApp.Services.OrderService.ProcessOrder(Order order) in /src/Services/OrderService.cs:line 45
   at MyApp.Controllers.OrderController.Post(OrderDto dto) in /src/Controllers/OrderController.cs:line 28
   at lambda_method(Closure , Object , Object[] )

How to use it:
- Open the file and go to the reported line (45 in OrderService.cs) — that's where the exception was observed.
- Look at the call above it to understand what inputs or invariants might be violated.
- Examine local variables in the frame where the exception occurred (use the Call Stack window to select the right frame).

If compiled without PDBs or line info, the trace still shows method names — use those to find the failing code.

---

## Diagnostics beyond the debugger
- Logging: add structured logs (Console / ILogger) to capture runtime state for cases that can’t be reproduced interactively.
- Assertions: Guard preconditions with `Debug.Assert` or argument validation to fail early.
- Diagnostic tools:
  - dotnet-counters (live counters): `dotnet-counters monitor --process-id <pid>`
  - dotnet-trace (traces): `dotnet-trace collect --process-id <pid> --output trace.nettrace`
  - dotnet-dump (post-mortem): `dotnet-dump collect --process-id <pid>`
  (Use these when the bug is performance-related or hard to reproduce in the debugger.)

Visual Studio Diagnostic Tools: CPU, memory, and exception events are available in the Diagnostic Tools window when debugging.

---

## A short, repeatable debugging plan (for classroom exercise)
1. Reproduce and describe the failure in one sentence.
2. Open the stack trace or run to the failing area.
3. Set a breakpoint right before the error.
4. Step and inspect variables in Locals/Watch.
5. Add a conditional breakpoint if needed (e.g., when index == 5).
6. If state depends on external input, add logging and re-run.
7. Propose a fix and write a small test or scenario to verify.
8. Commit and push the fix (see Git steps below).

Bring your plan to your pair-work and swap with another pair for review.

---

## Basic Git workflow (clone → commit → push)
Use this simple flow for homework and class projects.

Clone a repo:
```bash
git clone https://github.com/<organization>/<repo>.git
cd <repo>
```

Create and switch to a feature branch:
```bash
git checkout -b fix/order-null-check
```

Stage changes and commit with a clear message:
```bash
git add src/Services/OrderService.cs
git commit -m "Fix: add null-check in ProcessOrder to prevent NullReferenceException"
```

If you want a multi-line commit message:
```bash
git commit
# This opens your editor; first line is a short summary, following lines are details.
```

Push your branch to origin:
```bash
git push -u origin fix/order-null-check
```

Common Git tips:
- Use small, focused commits (one logical change per commit).
- Write clear commit titles: Verb + subject (e.g., "Add", "Fix", "Refactor").
- Use `.gitignore` to exclude build artifacts (`bin/`, `obj/`, `.vs/`).
- If the remote changed, pull/rebase before pushing:
```bash
git fetch origin
git pull --rebase origin main
# resolve conflicts, then
git push
```

IDE support:
- Visual Studio: Team Explorer / Git Changes panel to stage/commit/push with UI.
- VS Code: Source Control view shows changed files and actions (stage, commit, push).

---

## Example commit messages (for homework)
- "Init: add project skeleton with README and .gitignore"
- "Feature: implement SumAndAverage and unit tests"
- "Fix: handle empty array in SumAndAverage (return average = 0.0)"
- "Docs: add debugging checklist to Day5 guide"

---

## Final notes (what I did and what’s next)
I created a compact Day 5 reference that walks you through practical debugging tasks (breakpoints, stepping, watches), how to interpret stack traces and use diagnostics, and the essential Git commands for a simple commit workflow. Use the debugging plan in class when we inspect failing programs, and apply the Git steps when you prepare your repository and initial commits.

Next: Bring a small failing example or a repository skeleton to class. We will debug it together using the steps above, and each pair will practice the clone→commit→push flow.