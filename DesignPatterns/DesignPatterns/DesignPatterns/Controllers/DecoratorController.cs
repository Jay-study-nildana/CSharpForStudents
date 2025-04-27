using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//The Decorator dynamically adds behavior to an object without modifying its structure.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecoratorController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDecoratedOperation()
        {
            IComponent component = new ConcreteComponent();
            IComponent decoratedComponent = new ConcreteDecorator(component);
            return Ok(decoratedComponent.Operation());
        }
    }

    // IComponent.cs
    public interface IComponent
    {
        string Operation();
    }

    // ConcreteComponent.cs
    public class ConcreteComponent : IComponent
    {
        public string Operation() => "ConcreteComponent";
    }

    // Decorator.cs
    public abstract class Decorator : IComponent
    {
        protected readonly IComponent _component;

        protected Decorator(IComponent component)
        {
            _component = component;
        }

        public virtual string Operation() => _component.Operation();
    }

    // ConcreteDecorator.cs
    public class ConcreteDecorator : Decorator
    {
        public ConcreteDecorator(IComponent component) : base(component) { }

        public override string Operation() => $"{base.Operation()} + Decorated";
    }
}
