using LoggingWithEFCoreSQLite.DB;
using Microsoft.AspNetCore.Mvc;

namespace LoggingWithEFCoreSQLite.Controllers
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
        private readonly LoggerDBContext _dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, LoggerDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            List<LogItem> logItems = new List<LogItem>();
            logItems.Add(new LogItem() { Level = LogLevel.Information, Message = "Reached GetWeatherForecast", Timestamp = DateTime.UtcNow });

            var arraytoReturn = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            logItems.Add(new LogItem() { Level = LogLevel.Information, Message = "arraytoReturn has been created", Timestamp = DateTime.UtcNow });

            _dbContext.LogItems.AddRange(logItems);
            _dbContext.SaveChanges();

            return arraytoReturn;
        }

        [HttpGet]
        [Route("GetLogs")]
        public IActionResult GetLogs()
        {
            _logger.LogInformation("GetLogs called");

            var logs = _dbContext.LogItems.ToList();
            _logger.LogInformation($"Fetched {logs.Count} log items.");

            return Ok(logs);
        }

        [HttpPost]
        [Route("CreateLog")]
        public IActionResult CreateLog([FromBody] LogItem logItem)
        {
            _logger.LogInformation("CreateLog called");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid log item data received.");
                return BadRequest(ModelState);
            }

            logItem.Timestamp = DateTime.UtcNow;
            _dbContext.LogItems.Add(logItem);
            _dbContext.SaveChanges();

            _logger.LogInformation("Log item created successfully.");
            return CreatedAtAction(nameof(GetLogs), new { id = logItem.Id }, logItem);
        }
    }
}
