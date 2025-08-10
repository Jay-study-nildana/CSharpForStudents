using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Singleton ensures a class has only one instance and provides a global point of access to it.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/singleton"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class SingletonController : ControllerBase
    {
        // Handles HTTP GET requests to "api/singleton"
        [HttpGet]
        public IActionResult GetSingletonMessage()
        {
            // Retrieves a message from the singleton instance
            var message = SingletonService.Instance.GetMessage();
            // Returns the message with HTTP 200 OK status
            return Ok(message);
        }
    }

    // Implements the Singleton design pattern
    public class SingletonService
    {
        // Holds the single instance of SingletonService, created lazily and thread-safely
        private static readonly Lazy<SingletonService> _instance = new(() => new SingletonService());

        // Provides global access to the singleton instance
        public static SingletonService Instance => _instance.Value;

        // Private constructor prevents external instantiation
        private SingletonService() { }

        // Returns a message indicating this is the singleton instance
        public string GetMessage() => "This is a Singleton instance!";
    }
}