using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Strategy defines a family of algorithms, encapsulates each one, and makes them interchangeable.
namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrategyController : ControllerBase
    {
        [HttpGet("{type}")]
        public IActionResult ExecuteStrategy(string type)
        {
            var context = new Context();

            context.SetStrategy(type.ToLower() switch
            {
                "a" => new ConcreteStrategyA(),
                "b" => new ConcreteStrategyB(),
                _ => throw new ArgumentException("Invalid strategy type")
            });

            return Ok(context.ExecuteStrategy());
        }
    }

    // IStrategy.cs
    public interface IStrategy
    {
        string Execute();
    }

    // ConcreteStrategyA.cs
    public class ConcreteStrategyA : IStrategy
    {
        public string Execute() => "Strategy A executed.";
    }

    // ConcreteStrategyB.cs
    public class ConcreteStrategyB : IStrategy
    {
        public string Execute() => "Strategy B executed.";
    }

    // Context.cs
    public class Context
    {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public string ExecuteStrategy() => _strategy.Execute();
    }
}
