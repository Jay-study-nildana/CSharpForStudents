Problem: Conditional_Breakpoint_In_Loop

Solution (steps & examples)

1. Identify the loop and the index variable (e.g., `for (int index = 0; index < n; index++)`).

2. Set a conditional breakpoint:
   - Visual Studio: click gutter on the target line → right-click breakpoint → Conditions → enter `index == 507`.
   - VS Code: click gutter to set breakpoint → right pane Breakpoints → Add Conditional Breakpoint → expression `index === 507` (or `index == 507` depending on debugger).
   - For C#: use `index == 507`.

3. Alternative: use hit count:
   - Use a hit-count breakpoint if the conditional is costly or not accessible (`Hit count` = 507).

4. When breakpoint hits:
   - Inspect locals and watch expressions: `items[index]`, `current`, `previous`, `someFlag`.
   - Use Immediate window to evaluate `items[500..510]` or call helper methods to print state.

5. If the bug is timing-sensitive:
   - Use conditional breakpoint to break only when both `index == 507 && someFlag == true`.
   - Use "Break when thrown" for exceptions inside the loop.

6. Example immediate check:
   - Immediate: `?$"{index} => {items[index]?.ToString() ?? "null"}"` (or call a helper that returns formatted state).

Notes
- Conditional breakpoints cost a bit more runtime overhead but are perfect for large loops to avoid many hits.