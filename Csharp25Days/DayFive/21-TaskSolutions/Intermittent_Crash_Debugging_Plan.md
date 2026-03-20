Problem: Intermittent_Crash_Debugging_Plan

Solution (prioritized plan)

1. Gather symptom data:
   - Collect error messages, stack traces, OS, runtime version, recent changes, and any user actions.

2. Add lightweight telemetry/logging:
   - Add structured logging around suspected hotspots with correlation IDs and context.
   - Log exceptions with full stack traces and relevant values (avoid sensitive data).

3. Reproduce attempts:
   - Try stress tests, increased load, and different environments (Windows/Linux) to reproduce.

4. Collect dumps when crash occurs:
   - On Linux: use `dotnet-dump` on the process id:
     ```bash
     dotnet-dump collect --process-id <pid> --output /tmp/app.dmp
     ```
   - On Windows: use Task Manager or `dotnet-dump` similarly.

5. Analyze the dump:
   - Local analysis:
     ```bash
     dotnet-dump analyze /tmp/app.dmp
     # then use commands like:
     # dumpheap, clrstack, threads, eeheap
     ```
   - Use `dotnet-dump` to inspect stack of the crashing thread and managed exceptions.

6. Use monitoring:
   - Enable core diagnostic counters: `dotnet-counters`, perf traces to spot memory pressure, thread pool starvation, or exceptions.

7. Ask customers for:
   - Exact steps, environment details, minimal reproduction (screenshots, logs with timestamps).
   - Permission to collect diagnostic files and a trace.

8. Mitigation:
   - Add guard clauses, null-checks, and safe defaults to avoid crashing.
   - Deploy a diagnostic build (with extra logging) to a small subset if safe.

Notes
- Prioritize non-invasive logging first; dumps are heavier but crucial for root-cause of intermittent native/CLR crashes.