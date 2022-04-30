using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIWithDependencyInjection.Interfaces;

namespace WebAPIWithDependencyInjection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameWithDIController : ControllerBase
    {
        ISayMyName _sayMyName;

        //this is the key part
        //this is where, the interface is married to the implementation class
        //as provided by the startup settings in Program.cs
        public NameWithDIController(ISayMyName sayMyName)
        {
            _sayMyName = sayMyName;
        }

        [HttpGet]
        [Route("SayMyName")]
        public ActionResult<string> GetName()
        {           
            var response = _sayMyName.IAmName();

            return response;
        }
    }
}
