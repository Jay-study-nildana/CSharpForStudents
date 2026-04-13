// SingletonLoggerDemo.cs
// Console demo of naive (not thread-safe) Singleton pattern.

using System;

// Top-level statements
while (true)
{
    Console.WriteLine("Enter a message to log (or 'exit' to quit):");
    var msg = Console.ReadLine();
    if (msg == "exit") break;
    SingletonLogger.Instance.Log(msg);
}

public class SingletonLogger
{
    private static SingletonLogger _instance; // naive, not thread-safe
    private SingletonLogger() { }

    public static SingletonLogger Instance => _instance ??= new SingletonLogger();

    public void Log(string message)
    {
        Console.WriteLine($"[SingletonLogger] {message}");
    }
}


