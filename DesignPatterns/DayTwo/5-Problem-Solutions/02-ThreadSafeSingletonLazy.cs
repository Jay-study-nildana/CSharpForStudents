// ThreadSafeLoggerDemo.cs
// Console demo of thread-safe Singleton pattern using Lazy<T>.

using System;

/*
Why is thread safety essential?
- In multi-threaded applications, multiple threads might try to create the singleton at the same time.
- Without thread safety, this can result in multiple instances, breaking the singleton guarantee.
- Lazy<T> handles synchronization internally, ensuring only one instance is ever created, even under heavy concurrency.
*/

// Top-level statements
while (true)
{
    Console.WriteLine("Enter a message to log (or 'exit' to quit):");
    var msg = Console.ReadLine();
    if (msg == "exit") break;
    ThreadSafeLogger.Instance.Log(msg);
}

public class ThreadSafeLogger
{
    // Lazy<T> ensures that the instance is created only when needed (lazy initialization)
    // and guarantees thread safety by default. This means that even if multiple threads
    // try to access Instance at the same time, only one instance will be created.
    private static readonly Lazy<ThreadSafeLogger> _lazy =
        new Lazy<ThreadSafeLogger>(() => new ThreadSafeLogger());

    // Private constructor prevents external instantiation
    private ThreadSafeLogger() { }

    // The singleton instance is accessed via this property
    public static ThreadSafeLogger Instance => _lazy.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[ThreadSafeLogger] {message}");
    }
}
