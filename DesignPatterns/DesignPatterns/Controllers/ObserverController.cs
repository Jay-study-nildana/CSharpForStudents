using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Observer defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/observer"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class ObserverController : ControllerBase
    {
        // Handles HTTP POST requests to "api/observer"
        // Expects a message string in the request body
        [HttpPost]
        public IActionResult NotifyObservers([FromBody] string message)
        {
            // Create the subject (the object being observed)
            var subject = new Subject();
            // Create two observers and attach them to the subject
            var observer1 = new ConcreteObserver("Observer1");
            var observer2 = new ConcreteObserver("Observer2");

            subject.Attach(observer1);
            subject.Attach(observer2);

            // Notify all attached observers with the provided message
            subject.Notify(message);

            // Return a confirmation response
            return Ok("Observers notified.");
        }
    }

    // Defines the interface for observers that want to be notified of changes
    public interface IObserver
    {
        // Called by the subject to notify the observer of a change
        void Update(string message);
    }

    // Defines the interface for the subject that manages observers
    public interface ISubject
    {
        // Attach an observer to the subject
        void Attach(IObserver observer);
        // Detach an observer from the subject
        void Detach(IObserver observer);
        // Notify all attached observers with a message
        void Notify(string message);
    }

    // Concrete implementation of the subject
    public class Subject : ISubject
    {
        // List of observers currently attached to the subject
        private readonly List<IObserver> _observers = new();

        // Adds an observer to the list
        public void Attach(IObserver observer) => _observers.Add(observer);

        // Removes an observer from the list
        public void Detach(IObserver observer) => _observers.Remove(observer);

        // Notifies all observers by calling their Update method
        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    // Concrete implementation of an observer
    public class ConcreteObserver : IObserver
    {
        private readonly string _name; // Name to identify the observer

        // Constructor sets the observer's name
        public ConcreteObserver(string name)
        {
            _name = name;
        }

        // Called when the subject notifies observers; writes the message to the console
        public void Update(string message)
        {
            Console.WriteLine($"Observer {_name} received message: {message}");
        }
    }
}