using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Observer defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObserverController : ControllerBase
    {
        [HttpPost]
        public IActionResult NotifyObservers([FromBody] string message)
        {
            var subject = new Subject();
            var observer1 = new ConcreteObserver("Observer1");
            var observer2 = new ConcreteObserver("Observer2");

            subject.Attach(observer1);
            subject.Attach(observer2);

            subject.Notify(message);

            return Ok("Observers notified.");
        }
    }

    // IObserver.cs
    public interface IObserver
    {
        void Update(string message);
    }

    // ISubject.cs
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }

    // Subject.cs
    public class Subject : ISubject
    {
        private readonly List<IObserver> _observers = new();

        public void Attach(IObserver observer) => _observers.Add(observer);

        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    // ConcreteObserver.cs
    public class ConcreteObserver : IObserver
    {
        private readonly string _name;

        public ConcreteObserver(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"Observer {_name} received message: {message}");
        }
    }
}
