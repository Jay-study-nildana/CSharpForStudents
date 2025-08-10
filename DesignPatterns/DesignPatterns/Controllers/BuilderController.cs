using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Builder pattern separates the construction of a complex object from its representation.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/builder"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class BuilderController : ControllerBase
    {
        // Handles HTTP POST requests to "api/builder"
        // Expects a CarRequest object in the request body
        [HttpPost]
        public IActionResult BuildCar([FromBody] CarRequest request)
        {
            // Uses the builder to set properties based on the request
            var builder = new CarBuilder()
                .SetEngine(request.Engine)
                .SetWheels(request.Wheels)
                .SetColor(request.Color);

            // Builds the final Car object
            var car = builder.Build();
            // Returns a string representation of the built car with HTTP 200 OK status
            return Ok(car.ToString());
        }
    }

    // Represents the product being built: a Car
    public class Car
    {
        public string Engine { get; set; } // Type of engine
        public int Wheels { get; set; }    // Number of wheels
        public string Color { get; set; }  // Color of the car

        // Returns a string describing the car's configuration
        public override string ToString() => $"Car with {Engine} engine, {Wheels} wheels, and {Color} color.";
    }

    // Interface defining the builder methods for constructing a Car
    public interface ICarBuilder
    {
        ICarBuilder SetEngine(string engine); // Sets the engine type
        ICarBuilder SetWheels(int wheels);    // Sets the number of wheels
        ICarBuilder SetColor(string color);   // Sets the car color
        Car Build();                         // Builds and returns the Car object
    }

    // Concrete builder implementation for constructing Car objects
    public class CarBuilder : ICarBuilder
    {
        private readonly Car _car = new(); // Holds the car being built

        // Sets the engine and returns the builder for chaining
        public ICarBuilder SetEngine(string engine)
        {
            _car.Engine = engine;
            return this;
        }

        // Sets the number of wheels and returns the builder for chaining
        public ICarBuilder SetWheels(int wheels)
        {
            _car.Wheels = wheels;
            return this;
        }

        // Sets the color and returns the builder for chaining
        public ICarBuilder SetColor(string color)
        {
            _car.Color = color;
            return this;
        }

        // Returns the fully constructed Car object
        public Car Build() => _car;
    }

    // DTO for receiving car configuration from the client
    public class CarRequest
    {
        public string Engine { get; set; } // Requested engine type
        public int Wheels { get; set; }    // Requested number of wheels
        public string Color { get; set; }  // Requested color
    }
}