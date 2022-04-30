using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIWithDependencyInjection.Interfaces;
using WebAPIWithDependencyInjection.POCO;

namespace WebAPIWithDependencyInjection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {

        [HttpGet]
        [Route("SayMyName")]
        public ActionResult<string> GetName()
        {
            ISayMyName sayMyName = new SayMyNameOne();

            var response = sayMyName.IAmName();

            return response;
        }
    }
}
