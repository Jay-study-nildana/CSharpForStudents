using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace SerilogSQLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]
        public IActionResult LogInformation()
        {
            _logger.LogInformation("This is an informational log message.");
            return Ok("Information log recorded.");
        }

        [HttpGet("warning")]
        public IActionResult LogWarning()
        {
            _logger.LogWarning("This is a warning log message.");
            return Ok("Warning log recorded.");
        }

        [HttpGet("error")]
        public IActionResult LogError()
        {
            _logger.LogError("This is an error log message.");
            return Ok("Error log recorded.");
        }

        [HttpGet("logs")]
        public IActionResult GetAllLogs()
        {
            var logs = new List<string>();

            using (var connection = new SqliteConnection("Data Source=Logs.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Message FROM LogEntries
                    ORDER BY Timestamp DESC;
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(reader.GetString(0));
                    }
                }
            }

            return Ok(logs);
        }
    }
}
