using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Strategy defines a family of algorithms, encapsulates each one, and makes them interchangeable.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/strategy"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class StrategyController : ControllerBase
    {
        // Handles HTTP GET requests to "api/strategy/{type}"
        // The 'type' parameter determines which strategy to use
        [HttpGet("{type}")]
        public IActionResult ExecuteStrategy(string type)
        {
            var context = new Context(); // Create a context to hold the strategy

            // Select and set the strategy based on the 'type' parameter
            context.SetStrategy(type.ToLower() switch
            {
                "a" => new ConcreteStrategyA(), // Use Strategy A
                "b" => new ConcreteStrategyB(), // Use Strategy B
                _ => throw new ArgumentException("Invalid strategy type") // Invalid type throws an exception
            });

            // Execute the selected strategy and return the result
            return Ok(context.ExecuteStrategy());
        }
    }

    // Defines the interface for all strategies
    public interface IStrategy
    {
        // Method to execute the strategy's algorithm
        string Execute();
    }

    // Concrete implementation of Strategy A
    public class ConcreteStrategyA : IStrategy
    {
        // Returns a string indicating Strategy A was executed
        public string Execute() => "Strategy A executed.";
    }

    // Concrete implementation of Strategy B
    public class ConcreteStrategyB : IStrategy
    {
        // Returns a string indicating Strategy B was executed
        public string Execute() => "Strategy B executed.";
    }

    // Context class that uses a strategy
    public class Context
    {
        private IStrategy _strategy; // Holds the current strategy

        // Sets the strategy to be used
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        // Executes the current strategy's algorithm
        public string ExecuteStrategy() => _strategy.Execute();
    }
}