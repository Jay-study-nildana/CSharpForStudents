# Day 5 — Debugging & Git Practice Problems

Instructions: For each problem below produce a short, actionable solution: the steps you'd take in the IDE/terminal, the commands or settings you would use, and any short code or configuration snippets required. Where appropriate include sample Git commands and example commit messages.

Problems:

1. NullReference_From_StackTrace
   - You are given this stack trace from a failing run. Identify the likely cause and list step-by-step debugging actions to find the root cause.
   - Stack trace sample:
     System.NullReferenceException: Object reference not set to an instance of an object.
        at MyApp.Services.OrderService.ProcessOrder(Order order) in /src/Services/OrderService.cs:line 45
        at MyApp.Controllers.OrderController.Post(OrderDto dto) in /src/Controllers/OrderController.cs:line 28

2. Conditional_Breakpoint_In_Loop
   - A loop executes thousands of iterations and the bug only shows when `index == 507`. Describe how to set a conditional or hit-count breakpoint to pause only at that iteration and how to inspect relevant state.

3. Inspect_Object_With_Watch_And_Immediate
   - During stepping, an object `user.Account` looks wrong. Show how you'd use Locals/Watch/Immediate windows to inspect and, if safe, modify values to test fixes without recompiling.

4. Async_Await_StackTrace_Analysis
   - An exception occurs in an async method and the stack trace shows `Task`/`lambda_method` frames. Explain how to interpret the trace and find the original async method and source line. Include debug settings to ensure source line numbers are available.

5. Collect_Diagnostics_Dotnet_Trace
   - A production service shows CPU spikes. Provide the commands for collecting a trace with `dotnet-trace` and a short plan for analyzing it locally (which tool(s) you would use).

6. Git_Repo_Skeleton_and_Initial_Commits
   - Create a short text plan for a repository skeleton (files and folders), a suggested `.gitignore` minimal content for a C#/.NET project, and three example initial commit messages.

7. Resolve_Git_Merge_Conflict
   - Two teammates modified the same file and `git pull` results in a conflict. Describe the sequence of Git commands and in-IDE steps to resolve the conflict, how to test locally, and how to finalize the merge and push.

8. VSCode_LaunchJson_For_DotNet_Console
   - Provide a `launch.json` snippet for debugging a .NET console app in VS Code (attach and run options) and explain how to set breakpoints and use the Debug view.

9. Debug_Failing_Unit_Test
   - A unit test fails with an assertion. Give the step-by-step approach to reproduce the failure locally, run the test under the debugger, inspect test inputs and mocks, and produce a minimal fix and commit message.

10. Intermittent_Crash_Debugging_Plan
    - The app crashes intermittently on customers' machines (no reproducible steps). Produce a prioritized debugging plan: minimal reproduction attempts, adding logging, collecting dumps, and what to ask customers. Include commands for `dotnet-dump` to collect and analyze a dump.