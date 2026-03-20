using System;

class Decorator_LoggingDecorator
{
    // Problem: IService with a logging decorator
    public interface IService { void Execute(); }

    public class RealService : IService
    {
        public void Execute() => Console.WriteLine("RealService: executing business logic.");
    }

    public class LoggingServiceDecorator : IService
    {
        private readonly IService _inner;
        public LoggingServiceDecorator(IService inner) => _inner = inner;
        public void Execute()
        {
            Console.WriteLine("[LOG] Before execute");
            _inner.Execute();
            Console.WriteLine("[LOG] After execute");
        }
    }

    static void Main()
    {
        IService real = new RealService();
        IService logged = new LoggingServiceDecorator(real);
        logged.Execute();

        // Decorator composes behavior (logging) without changing RealService and enables testable composition.
    }
}