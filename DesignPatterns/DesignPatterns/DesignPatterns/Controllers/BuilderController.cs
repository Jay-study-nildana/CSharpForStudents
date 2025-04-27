using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Builder separates the construction of a complex object from its representation.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuilderController : ControllerBase
    {
        [HttpPost]
        public IActionResult BuildCar([FromBody] CarRequest request)
        {
            var builder = new CarBuilder()
                .SetEngine(request.Engine)
                .SetWheels(request.Wheels)
                .SetColor(request.Color);

            var car = builder.Build();
            return Ok(car.ToString());
        }
    }

    // Car.cs
    public class Car
    {
        public string Engine { get; set; }
        public int Wheels { get; set; }
        public string Color { get; set; }

        public override string ToString() => $"Car with {Engine} engine, {Wheels} wheels, and {Color} color.";
    }

    // ICarBuilder.cs
    public interface ICarBuilder
    {
        ICarBuilder SetEngine(string engine);
        ICarBuilder SetWheels(int wheels);
        ICarBuilder SetColor(string color);
        Car Build();
    }

    // CarBuilder.cs
    public class CarBuilder : ICarBuilder
    {
        private readonly Car _car = new();

        public ICarBuilder SetEngine(string engine)
        {
            _car.Engine = engine;
            return this;
        }

        public ICarBuilder SetWheels(int wheels)
        {
            _car.Wheels = wheels;
            return this;
        }

        public ICarBuilder SetColor(string color)
        {
            _car.Color = color;
            return this;
        }

        public Car Build() => _car;
    }

    public class CarRequest
    {
        public string Engine { get; set; }
        public int Wheels { get; set; }
        public string Color { get; set; }
    }
}
