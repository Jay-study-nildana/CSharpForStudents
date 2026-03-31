## BenchmarkDotNet (Quick Guide for .NET Microbenchmarks)

Short answer
------------
BenchmarkDotNet is a popular, production-ready .NET library for writing and running microbenchmarks. It runs your benchmark code under controlled conditions (multiple iterations, warmups, isolated processes) and produces statistical and allocation-aware reports. Use it to compare micro-implementations and measure small operations reliably.

Why use it
----------
- Isolates benchmarks from noise (JIT, warmup, background processes).
- Produces statistical summaries (mean, error, std dev, outliers).
- Reports memory allocations and GC info.
- Supports multiple runtimes and exporters (Markdown, HTML, CSV).
- Automates best practices: Release builds, process isolation, warmups.

Quick example
-------------
1) Create a project and add the package:
```bash
dotnet new console -n Benchmarks
cd Benchmarks
dotnet add package BenchmarkDotNet
```

2) Replace `Program.cs` with this minimal benchmark:

```csharp
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

[MemoryDiagnoser] // include memory allocation statistics
public class ConcatBenchmarks
{
    private string[] parts;

    [Params(10, 100)] // run benchmarks for different input sizes
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        parts = new string[N];
        for (int i = 0; i < N; i++) parts[i] = "x";
    }

    [Benchmark(Baseline = true)]
    public string StringConcat()
    {
        var s = "";
        foreach (var p in parts) s += p; // intentionally naive
        return s;
    }

    [Benchmark]
    public string StringBuilderConcat()
    {
        var sb = new StringBuilder();
        foreach (var p in parts) sb.Append(p);
        return sb.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<ConcatBenchmarks>();
    }
}
```

3) Run the benchmarks (Release build, no debugger):
```bash
dotnet run -c Release
```

What you’ll get
---------------
BenchmarkDotNet runs warmup and measurement iterations, in isolated processes, and prints/exports results under:
`bin/Release/netX/BenchmarkDotNet.Artifacts/results`. Output includes timing and allocation stats; use the Markdown/HTML exports for sharing.

Important features & attributes
-------------------------------
- `[Benchmark]` — marks a benchmark method.
- `[Params]` — run with multiple parameter values.
- `[GlobalSetup]`, `[GlobalCleanup]` — run once for setup/teardown.
- `[IterationSetup]`, `[IterationCleanup]` — setup/cleanup per iteration.
- `[MemoryDiagnoser]` — measure allocations and GC counts.
- `BenchmarkRunner.Run<T>()` — execute the benchmark class.
- Exporters / Jobs / Diagnosers to configure environments and output formats.

Best practices & tips
---------------------
- Run in Release configuration and without the debugger attached.
- Use `[GlobalSetup]` for expensive setup work — don't do setup inside measured code.
- Avoid I/O in the measured code unless intentionally measuring I/O.
- Prevent dead-code elimination: use return values, store results, or call `GC.KeepAlive(result)`.
- Scale the workload if the method is too fast (tiny operations are noisy).
- Use statistical output (error/stddev) to judge significance.

Common pitfalls
---------------
- Running in Debug mode or with the debugger attached — misleading results.
- Not warming up: BenchmarkDotNet handles warmup, but manual ad-hoc microbenchmarks can mislead.
- Letting the JIT optimize-away work — ensure the result is used.
- Misinterpreting small differences without checking statistical significance.

Advanced usage
--------------
- Run across multiple .NET runtimes (Core, Framework) and OSes.
- Use custom configs to control iterations, exporters, diagnosers, and loggers.
- Collect profiler traces or flamegraphs with diagnosers.
- Customize `Job` attributes for GC or JIT settings.

Where results are saved
-----------------------
BenchmarkDotNet writes results and diagnostics to:
`bin/Release/netX/BenchmarkDotNet.Artifacts/results`
You’ll find human-readable console output and exported `*.md`, `*.csv`, `*.html` artifacts.

Summary
-------
BenchmarkDotNet is the standard tool for safe, repeatable microbenchmarking in .NET. It automates warmup, isolation, and statistics so you can compare implementations with confidence and capture allocation/GC behavior.
