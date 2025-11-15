using DMnDBCS.UI.Services.Auth;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.Projects;
using DMnDBCS.UI.Services.Tasks;
using DMnDBCS.UI.Services.UserRoles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMnDBCS.UI.Controllers
{
    public class ProjectsController(IProjectsService projectsService, ITasksService tasksService,
        IProjectResourcesService projectResourcesService, IUserRolesService userRolesService, ITokenAccessor tokenAccessor) : Controller
    {
        private readonly IProjectsService _projectsService = projectsService;
        private readonly ITasksService _tasksService = tasksService;
        private readonly IProjectResourcesService _projectResourcesService = projectResourcesService;
        private readonly IUserRolesService _userRolesService = userRolesService;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;

        // GET: ProjectsController
        public async Task<ActionResult> Index()
        {
            var response = await _projectsService.GetProjectListAsync();
            if (!response.IsSuccessful)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // GET: ProjectsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var projectResponse = await _projectsService.GetByIdAsync(id);
            if (!projectResponse.IsSuccessful)
            {
                return NotFound(projectResponse.ErrorMessage);
            }

            var taskResponse = await _tasksService.GetTasksForProjectAsync(id);
            if (!taskResponse.IsSuccessful)
            {
                return NotFound(taskResponse.ErrorMessage);
            }
            ViewBag.Tasks = taskResponse.Data;

            var resourceResponse = await _projectResourcesService.GetProjectResourcesByProjectIdAsync(id);
            if (!resourceResponse.IsSuccessful)
            {
                return NotFound(resourceResponse.ErrorMessage);
            }
            ViewBag.Resources = resourceResponse.Data;

            var memberResponse = await _userRolesService.GetRolesForProjectByIdAsync(id);
            if (!memberResponse.IsSuccessful)
            {
                return NotFound(memberResponse.ErrorMessage);
            }
            ViewBag.Members = memberResponse.Data;

            return View(projectResponse.Data);
        }

        // GET: ProjectsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectsController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(project);
                }

                if (project.StartDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(project.StartDate), "Start date cannot be later than today");
                    return View(project);
                }

                if (project.EndDate is not null && project.EndDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(project.EndDate), "End date cannot be later than today");
                    return View(project);
                }

                if (project.EndDate is not null && project.StartDate > project.EndDate)
                {
                    ModelState.AddModelError(nameof(project.EndDate), "End date cannot be earlier than start date");
                    return View(project);
                }

                await _projectsService.CreateAsync(project);

                return RedirectToAction("Index", "Projects");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _projectsService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: ProjectsController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Project project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(project);
                }

                if (project.StartDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(project.StartDate), "Start date cannot be later than today");
                    return View(project);
                }

                if (project.EndDate is not null && project.EndDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(nameof(project.EndDate), "End date cannot be later than today");
                    return View(project);
                }

                if (project.EndDate is not null && project.StartDate > project.EndDate)
                {
                    ModelState.AddModelError(nameof(project.EndDate), "End date cannot be earlier than start date");
                    return View(project);
                }

                await _projectsService.UpdateAsync(project);

                return RedirectToAction("Index", "Projects");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _projectsService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: ProjectsController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _projectsService.DeleteAsync(id);

                return RedirectToAction("Index", "Projects");
            }
            catch
            {
                return View();
            }
        }
    }
}
