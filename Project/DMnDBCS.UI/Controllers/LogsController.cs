using DMnDBCS.UI.Services.Logs;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class LogsController(ILogsService logsService) : Controller
    {
        private readonly ILogsService _logService = logsService;

        // GET: LogController
        public async Task<ActionResult> Index()
        {
            var logResponse = await _logService.GetLogListAsync();
            if (!logResponse.IsSuccessful)
            {
                return NotFound(logResponse.ErrorMessage);
            }

            return View(logResponse.Data);
        }
    }
}
