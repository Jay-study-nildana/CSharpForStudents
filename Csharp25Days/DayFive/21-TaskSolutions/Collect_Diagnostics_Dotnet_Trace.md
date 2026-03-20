Problem: Collect_Diagnostics_Dotnet_Trace

Solution (commands & plan)

1. Identify the process id:
   ```bash
   dotnet-trace ps
   # or
   ps aux | grep MyApp
   ```

2. Collect a trace (for example 60 seconds, using CPU sampling provider):
   ```bash
   dotnet-trace collect --process-id <pid> --providers Microsoft-DotNETCore-SampleProfiler --duration 00:01:00 --output cpu_trace.nettrace
   ```
   - Or a general trace:
   ```bash
   dotnet-trace collect --process-id <pid> --output app_trace.nettrace
   ```

3. Transfer trace file locally if collected on server.

4. Analyze trace:
   - Use PerfView (Windows) or dotnet-trace report/`dotnet-trace` convert tools:
     - PerfView: open `cpu_trace.nettrace` → analyze CPU stacks/flame graphs.
     - `dotnet-trace` + `dotnet-trace convert` to speedscope: `dotnet-trace convert --format speedscope cpu_trace.nettrace` and open in speedscope.app.
   - Look for top CPU hotspots and long-running methods.

5. Follow-up:
   - If trace points to a hot method, add more granular telemetry or micro-benchmarks.
   - Consider sampling GC and thread pool activity with dotnet-counters:
     ```bash
     dotnet-counters monitor --process-id <pid> System.Runtime --refresh-interval 1
     ```

Notes
- Traces can be large; set sensible duration and rotate if necessary. Ensure privacy when collecting traces from production.