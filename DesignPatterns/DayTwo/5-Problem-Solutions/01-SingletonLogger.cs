// 01-SingletonLogger.cs
// Problem: Implement a basic Singleton logger class with global access.
// Notes: This implementation is the simple (non-thread-safe) singleton.
// Common problems: not thread-safe; hidden global state; hard to replace in tests.

using System;

public class SingletonLogger
{
    private static SingletonLogger _instance; // naive, not thread-safe
    private SingletonLogger() { }

    public static SingletonLogger Instance => _instance ??= new SingletonLogger();

    public void Log(string message)
    {
        // Simple console output for demonstration
        Console.WriteLine($"[SingletonLogger] {message}");
    }
}

/*
Usage:
SingletonLogger.Instance.Log("Starting application");

Discussion:
- This is lazy but not thread-safe: two threads could create two instances.
- The global access ties code to a hidden global dependency, making testing harder.
*/