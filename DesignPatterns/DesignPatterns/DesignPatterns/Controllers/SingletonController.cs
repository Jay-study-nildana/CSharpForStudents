using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//The Singleton ensures a class has only one instance and provides a global point of access to it.

namespace DesignPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingletonController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSingletonMessage()
        {
            var message = SingletonService.Instance.GetMessage();
            return Ok(message);
        }
    }

    public class SingletonService
    {
        private static readonly Lazy<SingletonService> _instance = new(() => new SingletonService());

        public static SingletonService Instance => _instance.Value;

        private SingletonService() { }

        public string GetMessage() => "This is a Singleton instance!";
    }
}
