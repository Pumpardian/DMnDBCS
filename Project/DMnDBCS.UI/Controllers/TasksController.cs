using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.TaskComments;
using DMnDBCS.UI.Services.Tasks;
using DMnDBCS.UI.Services.TaskStatuses;
using DMnDBCS.UI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace DMnDBCS.UI.Controllers
{
    public class TasksController(ITasksService tasksService, ITaskCommentsService taskCommentsService, ITaskStatusesService taskStatusesService,
        IJwtService jwtService, IUsersService usersService) : Controller
    {
        private readonly ITasksService _taskService = tasksService;
        private readonly ITaskCommentsService _taskCommentsService = taskCommentsService;
        private readonly ITaskStatusesService _taskStatusesService = taskStatusesService;
        private readonly IUsersService _usersService = usersService;
        private readonly IJwtService _jwtService = jwtService;

        // GET: TasksController
        public async Task<ActionResult> Index()
        {
            var response = await _taskService.GetTasksForExecutorAsync(int.Parse(_jwtService.GetUserId()!));
            if (!response.IsSuccessful)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // GET: TasksController/Details/5
        public async Task<ActionResult> Details(int id, bool isFromProject = false)
        {
            ViewBag.FromProject = isFromProject;

            var taskResponse = await _taskService.GetByIdAsync(id);
            if (!taskResponse.IsSuccessful)
            {
                return NotFound(taskResponse.ErrorMessage);
            }

            var commentResponse = await _taskCommentsService.GetTaskCommentsByTaskIdAsync(id);
            if (!commentResponse.IsSuccessful)
            {
                return NotFound(commentResponse.ErrorMessage);
            }
            ViewBag.Comments = commentResponse.Data;

            return View(taskResponse.Data);
        }

        // GET: TasksController/Create
        public async Task<ActionResult> Create(int projectId)
        {
            var usersResponse = await _usersService.GetAllInProjectAsync(projectId);
            if (!usersResponse.IsSuccessful)
            {
                return NotFound(usersResponse.ErrorMessage);
            }
            ViewBag.Users = usersResponse.Data;

            var statusesResponse = await _taskStatusesService.GetAllAsync();
            if (!statusesResponse.IsSuccessful)
            {
                return NotFound(statusesResponse.ErrorMessage);
            }
            ViewBag.Statuses = statusesResponse.Data;

            return View(new Domain.Entities.Task() { ProjectId = projectId, CreationDate = DateOnly.FromDateTime(DateTime.Today) });
        }

        // POST: TasksController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Domain.Entities.Task task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(task);
                }

                if (task.CreationDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(task.CreationDate), "Creation date cannot be later than today");
                    return View(task);
                }

                if (task.CompletionDate is not null && task.CompletionDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(task.CompletionDate), "Completion date cannot be later than today");
                    return View(task);
                }

                if (task.CompletionDate is not null && task.CreationDate > task.CompletionDate)
                {
                    ModelState.AddModelError(nameof(task.CompletionDate), "Completion date cannot be earlier than creation date");
                    return View(task);
                }

                await _taskService.CreateAsync(task);

                return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TasksController/Edit/5
        public async Task<ActionResult> Edit(int id, bool isFromProject = false)
        {
            ViewBag.FromProject = isFromProject;

            var response = await _taskService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            var usersResponse = await _usersService.GetAllInProjectAsync(response.Data.ProjectId);
            if (!usersResponse.IsSuccessful)
            {
                return NotFound(usersResponse.ErrorMessage);
            }
            ViewBag.Users = usersResponse.Data;

            var statusesResponse = await _taskStatusesService.GetAllAsync();
            if (!statusesResponse.IsSuccessful)
            {
                return NotFound(statusesResponse.ErrorMessage);
            }
            ViewBag.Statuses = statusesResponse.Data;

            return View(response.Data);
        }

        // POST: TasksController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Domain.Entities.Task task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(task);
                }

                if (task.CreationDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(task.CreationDate), "Creation date cannot be later than today");
                    return View(task);
                }

                if (task.CompletionDate is not null && task.CompletionDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(task.CompletionDate), "Completion date cannot be later than today");
                    return View(task);
                }

                if (task.CompletionDate is not null && task.CreationDate > task.CompletionDate)
                {
                    ModelState.AddModelError(nameof(task.CompletionDate), "Completion date cannot be earlier than creation date");
                    return View(task);
                }

                await _taskService.UpdateAsync(task);

                if (!(bool) TempData["FromProject"]!)
                {
                    return RedirectToAction("Index", "Tasks");
                }

                return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TasksController/Delete/5
        public async Task<ActionResult> Delete(int id, bool isFromProject = false)
        {
            ViewBag.FromProject = isFromProject;

            var response = await _taskService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: TasksController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, int projectId)
        {
            try
            {
                await _taskService.DeleteAsync(id);

                return RedirectToAction("Details", "Projects", new { id = projectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
