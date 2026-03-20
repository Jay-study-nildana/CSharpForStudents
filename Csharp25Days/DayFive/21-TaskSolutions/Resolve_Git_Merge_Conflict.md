Problem: Resolve_Git_Merge_Conflict

Solution (commands & steps)

1. Scenario: `git pull` → conflict in `src/Service.cs`.

2. Steps to resolve:
   - Fetch and rebase (preferred) or merge:
     ```bash
     git fetch origin
     git rebase origin/main
     # conflict occurs
     ```
   - Open conflict markers in file(s): `<<<<<<< HEAD`, `=======`, `>>>>>>> branch`.
   - Use IDE merge tools:
     - Visual Studio: Team Explorer → Conflicts → Merge tool.
     - VS Code: open file; use editor inline actions (Accept Current / Incoming / Both / Compare).
   - Test locally:
     - Build: `dotnet build`
     - Run unit tests: `dotnet test`
   - Mark conflict resolved:
     ```bash
     git add src/Service.cs
     git rebase --continue
     # OR if merging:
     git commit -m "Merge: resolve conflict in Service.cs"
     ```
   - Push:
     ```bash
     git push origin my-branch
     ```

3. If changes from both sides needed:
   - Manually combine code (preserve both behaviors), add unit tests to cover combined behavior.

4. If unsure:
   - Create a temporary branch to experiment: `git checkout -b conflict-resolve-temp`
   - Do testing there; then merge back.

Notes
- Keep conflict resolution small and test; avoid commit messages that don't explain how conflict was resolved.