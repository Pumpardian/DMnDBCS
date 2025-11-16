using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.Logs;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class LogsController(ILogsService logsService, IJwtService jwtService) : Controller
    {
        private readonly ILogsService _logService = logsService;
        private readonly IJwtService _jwtService = jwtService;

        // GET: LogController
        public async Task<ActionResult> Index()
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var logResponse = await _logService.GetLogListAsync();
            if (!logResponse.IsSuccessful)
            {
                return NotFound(logResponse.ErrorMessage);
            }

            return View(logResponse.Data);
        }
    }
}
