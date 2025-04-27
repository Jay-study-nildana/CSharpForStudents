using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Adapter allows incompatible interfaces to work together by converting the interface of a class into another interface the client expects.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdapterController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAdaptedRequest()
        {
            var adaptee = new Adaptee();
            ITarget adapter = new Adapter(adaptee);
            return Ok(adapter.GetRequest());
        }
    }

    // ITarget.cs
    public interface ITarget
    {
        string GetRequest();
    }

    // Adaptee.cs
    public class Adaptee
    {
        public string GetSpecificRequest() => "Specific request from Adaptee";
    }

    // Adapter.cs
    public class Adapter : ITarget
    {
        private readonly Adaptee _adaptee;

        public Adapter(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        public string GetRequest() => $"Adapter: {_adaptee.GetSpecificRequest()}";
    }
}
