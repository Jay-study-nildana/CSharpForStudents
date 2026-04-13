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