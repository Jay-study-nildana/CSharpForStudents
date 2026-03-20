using System;

class Composition_vs_Inheritance_Refactor
{
    // IPrinter interface
    public interface IPrinter { void Print(string text); }

    // Simple printer implementation
    public class ConsolePrinter : IPrinter
    {
        public void Print(string text) => Console.WriteLine(text);
    }

    // Logger dependency
    public interface ILogger { void Log(string msg); }
    public class ConsoleLogger : ILogger { public void Log(string msg) => Console.WriteLine("[LOG] " + msg); }

    // Composition: LoggerPrinter composes an IPrinter and an ILogger
    public class LoggerPrinter : IPrinter
    {
        private readonly IPrinter _inner;
        private readonly ILogger _logger;
        public LoggerPrinter(IPrinter inner, ILogger logger) { _inner = inner; _logger = logger; }
        public void Print(string text)
        {
            _logger.Log("About to print");
            _inner.Print(text);
            _logger.Log("Done printing");
        }
    }

    static void Main()
    {
        IPrinter p = new ConsolePrinter();
        ILogger logger = new ConsoleLogger();

        // Compose behavior instead of creating an inheritance hierarchy like LoggedConsolePrinter : ConsolePrinter
        IPrinter lp = new LoggerPrinter(p, logger);
        lp.Print("Hello via composed logger/printer");

        Console.WriteLine("Composition allows flexible behavior without rigid inheritance trees.");
    }
}