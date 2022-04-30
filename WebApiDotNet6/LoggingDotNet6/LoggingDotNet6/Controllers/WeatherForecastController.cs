using LoggingDotNet6.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LoggingDotNet6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILogger<GenericHelper> _logger2;
        private GenericHelper genericHelper;

        //Note : I am planning to use the logger in the GenericHelper helper as well
        //so need to get that instantiated here as well. Otherwise, Generic Helper
        //wont be able to use logging facilities.
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ILogger<GenericHelper> logger2)
        {
            _logger = logger;
            _logger2 = logger2;
            genericHelper = new GenericHelper(_logger2);

            _logger.LogInformation(1, "WeatherForecastController has been constructed");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("GetWeatherForecast has been called");

            genericHelper.JustADumbFunctionCall();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

    }
}