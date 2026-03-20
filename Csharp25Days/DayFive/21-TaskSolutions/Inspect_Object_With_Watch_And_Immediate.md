Problem: Inspect_Object_With_Watch_And_Immediate

Solution (practical steps)

1. Pause the program where the object is in scope (set a breakpoint just before the suspicious line).

2. Use Locals/Autos:
   - Inspect `user` and `user.Account` in Locals/Autos panes for nulls and fields.

3. Add expressions to Watch:
   - Add `user.Account.Balance`, `user.Account.IsActive`, `user.Account.Transactions.Count`.
   - Add computed expressions like `user.Account.Transactions.LastOrDefault()` (if allowed).

4. Use Immediate/Debug Console:
   - Evaluate expressions: `? user.Account` or `user.Account.Transactions.Count`.
   - Call read-only helper methods (or small methods you add) to produce structured output: `? FormatAccount(user.Account)`.

5. Mutate state temporarily (careful):
   - In Immediate you can assign: `user.Account.IsActive = true` to test behavior without recompile.
   - Avoid long-lived side-effects; restrict this to local dev/debugging.

6. If object is large:
   - Create a helper that returns a small DTO for inspection and call it in Immediate.
   - Or serialize subset: `Console.WriteLine(JsonSerializer.Serialize(user.Account, new JsonSerializerOptions{ WriteIndented = true }));`

Notes
- Immediate allows method invocation; prefer pure/read-only helpers to avoid masking bugs with test mutations.