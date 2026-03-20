using System;

class ILogger_and_Implementations
{
    // Problem: ILogger and two implementations including NullLogger (null object)
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[LOG] {message}");
    }

    public class NullLogger : ILogger
    {
        public void Log(string message) { /* no-op */ }
    }

    public class Worker
    {
        private readonly ILogger _logger;
        public Worker(ILogger logger) => _logger = logger;
        public void DoWork() { _logger.Log("Work started"); /* ... */ _logger.Log("Work finished"); }
    }

    static void Main()
    {
        var workerWithLog = new Worker(new ConsoleLogger());
        workerWithLog.DoWork();

        var workerNoLog = new Worker(new NullLogger()); // avoids null checks
        workerNoLog.DoWork();

        // NullLogger simplifies callers by avoiding if(logger != null) checks.
    }
}