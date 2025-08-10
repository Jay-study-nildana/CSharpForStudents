using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Adapter allows incompatible interfaces to work together by converting the interface of a class into another interface the client expects.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/adapter"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class AdapterController : ControllerBase
    {
        // Handles HTTP GET requests to "api/adapter"
        [HttpGet]
        public IActionResult GetAdaptedRequest()
        {
            // Create an instance of the class with the incompatible interface
            var adaptee = new Adaptee();
            // Wrap the adaptee with an adapter to expose the expected interface
            ITarget adapter = new Adapter(adaptee);
            // Return the adapted request string with HTTP 200 OK status
            return Ok(adapter.GetRequest());
        }
    }

    // Defines the interface expected by the client
    public interface ITarget
    {
        // Method to get the request in the expected format
        string GetRequest();
    }

    // The class with an incompatible interface
    public class Adaptee
    {
        // Method that provides a specific request not matching the client's expected interface
        public string GetSpecificRequest() => "Specific request from Adaptee";
    }

    // The Adapter class implements the expected interface and translates calls to the adaptee
    public class Adapter : ITarget
    {
        private readonly Adaptee _adaptee; // Holds a reference to the adaptee

        // Constructor accepts an adaptee instance to adapt
        public Adapter(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        // Implements the expected method by internally calling the adaptee's method
        public string GetRequest() => $"Adapter: {_adaptee.GetSpecificRequest()}";
    }
}