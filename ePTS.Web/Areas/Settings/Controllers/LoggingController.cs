using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Serilog.Events;

namespace ePTS.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class LoggingController : Controller
    {
        private readonly LoggingLevelSwitch _levelSwitch;

        public LoggingController(LoggingLevelSwitch levelSwitch)
        {
            _levelSwitch = levelSwitch;
        }

        public IActionResult SetLoggingLevel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLoggingLevel(string level)
        {
            if (Enum.TryParse<LogEventLevel>(level, out var logEventLevel))
            {
                _levelSwitch.MinimumLevel = logEventLevel;
                return Ok($"Logging level set to {level}");
            }
            return BadRequest("Invalid log level");
        }
        
    }
}
