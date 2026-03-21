// 02-ThreadSafeSingletonLazy.cs
// Problem: Thread-safe singleton using Lazy<T> for lazy, safe initialization.

using System;

public class ThreadSafeLogger
{
    private static readonly Lazy<ThreadSafeLogger> _lazy =
        new Lazy<ThreadSafeLogger>(() => new ThreadSafeLogger());

    private ThreadSafeLogger() { }

    public static ThreadSafeLogger Instance => _lazy.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[ThreadSafeLogger] {message}");
    }
}

/*
Why safe: Lazy<T> ensures the value factory runs only once and is thread-safe by default.
*/