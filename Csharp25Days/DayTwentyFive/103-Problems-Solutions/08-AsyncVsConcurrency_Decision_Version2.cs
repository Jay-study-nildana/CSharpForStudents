// 08-AsyncVsConcurrency_Decision.cs
// (A) Fetch many URLs => use async/await with HttpClient.
// (B) Matrix multiplication => use Parallel.For for CPU-bound parallelism.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;

public class IOWork
{
    private static readonly HttpClient _http = new();

    // I/O-bound: use async to avoid blocking threads while awaiting network
    public async Task<List<string>> FetchUrlsAsync(IEnumerable<string> urls)
    {
        var tasks = urls.Select(url => _http.GetStringAsync(url));
        return (await Task.WhenAll(tasks)).ToList();
    }
}

public class CPUWork
{
    // CPU-bound: use parallelism to utilize multiple cores
    public static double[,] Multiply(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        var c = new double[n, n];
        Parallel.For(0, n, i =>
        {
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int k = 0; k < n; k++) sum += a[i, k] * b[k, j];
                c[i, j] = sum;
            }
        });
        return c;
    }
}

/*
Decision rationale:
- FetchUrlsAsync: network latency is I/O-bound—async reduces thread usage.
- Multiply: CPU-bound heavy compute benefits from multi-core parallelism.
*/