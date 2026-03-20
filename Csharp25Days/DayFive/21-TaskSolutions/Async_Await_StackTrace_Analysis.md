Problem: Async_Await_StackTrace_Analysis

Solution (explain & steps)

1. Understand async traces:
   - Async methods may show `Task` and lambda frames; the original async method appears with `at MyApp.Service.DoWorkAsync(...)` if PDBs are present.
   - If the trace contains `lambda_method`, look for compiler-generated methods; find the async method by searching for callers.

2. Debug settings:
   - Ensure PDBs are present and optimize settings are disabled for the debug build (enable "Enable Just My Code" and "Use Managed Compatibility Mode" only if needed).
   - In Visual Studio: Debug → Options → Debugging → check "Enable .NET Framework source stepping" only if stepping into framework required.

3. Find original source:
   - Open the Call Stack window. Expand "async" frames using the "Show external code" or "Show async frames" option (checkbox or context menu) to see await points and original methods.
   - Double-click the frame to navigate to source; examine the await sites and variables.

4. If source lines are missing:
   - Rebuild with full debug info: `dotnet build -c Debug -p:DebugType=full`.
   - Reproduce and get stack trace with line numbers.

5. When debugging:
   - Set breakpoints at suspected `await` boundaries (before and after awaits).
   - Use "Step Into" to step into continuations when needed.

Notes
- async/await can spread execution after await; inspect state at both sides of the await and any captured closures.