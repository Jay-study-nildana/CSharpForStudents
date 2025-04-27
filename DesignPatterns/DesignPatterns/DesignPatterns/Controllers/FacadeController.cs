using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Facade provides a simplified interface to a larger body of code, making it easier to use.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacadeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetFacadeOperation()
        {
            var facade = new Facade(new SubsystemA(), new SubsystemB());
            return Ok(facade.Operation());
        }
    }

    // SubsystemA.cs
    public class SubsystemA
    {
        public string OperationA() => "SubsystemA: Ready!";
    }

    // SubsystemB.cs
    public class SubsystemB
    {
        public string OperationB() => "SubsystemB: Go!";
    }

    // Facade.cs
    public class Facade
    {
        private readonly SubsystemA _subsystemA;
        private readonly SubsystemB _subsystemB;

        public Facade(SubsystemA subsystemA, SubsystemB subsystemB)
        {
            _subsystemA = subsystemA;
            _subsystemB = subsystemB;
        }

        public string Operation()
        {
            return $"{_subsystemA.OperationA()} {Environment.NewLine}{_subsystemB.OperationB()}";
        }
    }
}
