// RetryTransientFailures.cs
// Problem: RetryTransientFailures
// Implement a retry helper that retries on transient exceptions (simulated with IOException).
// Approach: exponential backoff, rethrow original after exhausting retries.

using System;
using System.IO;
using System.Threading;

class RetryPolicy
{
    public static void Execute(Action action, int maxAttempts = 3, int initialDelayMs = 200)
    {
        int attempt = 0;
        int delay = initialDelayMs;
        while (true)
        {
            try { attempt++; action(); return; }
            catch (IOException) when (attempt < maxAttempts)
            {
                Console.WriteLine($"Transient failure, retrying attempt {attempt} after {delay}ms");
                Thread.Sleep(delay);
                delay *= 2;
            }
        }
    }
}

class RetryTransientFailures
{
    static int counter = 0;
    static void Flaky()
    {
        counter++;
        if (counter < 3) throw new IOException("Transient IO");
        Console.WriteLine("Succeeded on attempt " + counter);
    }

    static void Main()
    {
        try
        {
            RetryPolicy.Execute(Flaky, maxAttempts: 5);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Persistent failure: " + ex.Message);
        }
    }
}