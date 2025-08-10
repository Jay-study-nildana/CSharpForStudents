using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// The Facade provides a simplified interface to a larger body of code, making it easier to use.

namespace DesignPatterns.Controllers
{
    // Sets up the route for this controller as "api/facade"
    [Route("api/[controller]")]
    // Marks this class as an API controller, enabling features like automatic model validation
    [ApiController]
    public class FacadeController : ControllerBase
    {
        // Handles HTTP GET requests to "api/facade"
        [HttpGet]
        public IActionResult GetFacadeOperation()
        {
            // Create instances of the subsystems
            var facade = new Facade(new SubsystemA(), new SubsystemB());
            // Use the facade to perform a combined operation and return the result
            return Ok(facade.Operation());
        }
    }

    // Represents a subsystem with its own functionality
    public class SubsystemA
    {
        // Returns a string indicating SubsystemA is ready
        public string OperationA() => "SubsystemA: Ready!";
    }

    // Represents another subsystem with its own functionality
    public class SubsystemB
    {
        // Returns a string indicating SubsystemB is ready to go
        public string OperationB() => "SubsystemB: Go!";
    }

    // The Facade class provides a simplified interface to the subsystems
    public class Facade
    {
        // References to the subsystems
        private readonly SubsystemA _subsystemA;
        private readonly SubsystemB _subsystemB;

        // Constructor accepts subsystem instances to work with
        public Facade(SubsystemA subsystemA, SubsystemB subsystemB)
        {
            _subsystemA = subsystemA;
            _subsystemB = subsystemB;
        }

        // Combines operations from both subsystems into a single method
        public string Operation()
        {
            // Calls each subsystem's operation and combines their results
            return $"{_subsystemA.OperationA()} {Environment.NewLine}{_subsystemB.OperationB()}";
        }
    }
}