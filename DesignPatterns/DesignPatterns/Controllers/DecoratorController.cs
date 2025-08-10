using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Decorator dynamically adds behavior to an object without modifying its structure.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/decorator"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class DecoratorController : ControllerBase
    {
        // Handles HTTP GET requests to "api/decorator"
        [HttpGet]
        public IActionResult GetDecoratedOperation()
        {
            // Create the base component
            IComponent component = new ConcreteComponent();
            // Wrap the base component with a decorator to add new behavior
            IComponent decoratedComponent = new ConcreteDecorator(component);
            // Return the result of the decorated operation with HTTP 200 OK status
            return Ok(decoratedComponent.Operation());
        }
    }

    // Defines the interface for components that can be decorated
    public interface IComponent
    {
        // Method to perform an operation; can be extended by decorators
        string Operation();
    }

    // Concrete implementation of the component
    public class ConcreteComponent : IComponent
    {
        // Returns a string representing the base operation
        public string Operation() => "ConcreteComponent";
    }

    // Abstract decorator class that implements IComponent and wraps another IComponent
    public abstract class Decorator : IComponent
    {
        // Reference to the component being decorated
        protected readonly IComponent _component;

        // Constructor accepts a component to decorate
        protected Decorator(IComponent component)
        {
            _component = component;
        }

        // Delegates the operation to the wrapped component
        public virtual string Operation() => _component.Operation();
    }

    // Concrete decorator that adds new behavior to the component
    public class ConcreteDecorator : Decorator
    {
        // Passes the component to the base decorator
        public ConcreteDecorator(IComponent component) : base(component) { }

        // Extends the base operation with additional behavior
        public override string Operation() => $"{base.Operation()} + Decorated";
    }
}