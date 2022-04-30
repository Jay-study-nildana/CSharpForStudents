using LoggingDotNet6.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingDotNet6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SherlockHolmesController : ControllerBase
    {
        private readonly ILogger<SherlockHolmesController> _logger;
        private readonly ILogger<GenericHelper> _logger2;
        private GenericHelper genericHelper;

        public SherlockHolmesController(ILogger<SherlockHolmesController> logger,
            ILogger<GenericHelper> logger2)
        {
            _logger = logger;
            _logger2 = logger2;
            genericHelper = new GenericHelper(_logger2);

            _logger.LogInformation(1, "SherlockHolmesController has been constructed");
        }

        [HttpGet]
        [Route("GetHoldOfthem")]
        public ActionResult<string> Elementary()
        {
            var tempString = "Elementary, Watson";

            genericHelper.JustADumbFunctionCall();

            return tempString;
        }
    }
}
