Problem: Debug_Failing_Unit_Test

Solution (steps)

1. Reproduce:
   - Run the single failing test locally: `dotnet test --filter FullyQualifiedName~Namespace.Class.TestMethod` or use the test explorer.

2. Run under the debugger:
   - Visual Studio: Right-click test → Debug Selected Tests.
   - VS Code: Use the .NET Test Explorer extension or attach a debug session and call the test runner.

3. Inspect test inputs & mocks:
   - Check Arrange section of test; inspect mocked dependencies for incorrect setup.
   - Set breakpoint inside the test and inside the system-under-test (SUT) method.

4. Step and inspect:
   - Step through code until the assertion point.
   - Use Watches to inspect expected vs actual values.

5. Fix and verify:
   - If test expectation is wrong, update test with corrected expected value.
   - If SUT bug: fix the code, ensure behavior is correct for other tests.

6. Commit message example:
   - "Test: fix expected value for CalculateTax_WhenRateZero"
   - or "Fix: correct tax calculation when rate = 0; add unit test"

Notes
- If test uses flaky timing (async) add deterministic waits or use async-friendly testing patterns.