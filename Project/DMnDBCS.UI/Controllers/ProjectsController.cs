using DMnDBCS.UI.Services.Auth;
using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.Projects;
using DMnDBCS.UI.Services.Roles;
using DMnDBCS.UI.Services.Tasks;
using DMnDBCS.UI.Services.UserRoles;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class ProjectsController(IProjectsService projectsService, ITasksService tasksService, IProjectResourcesService projectResourcesService,
        IUserRolesService userRolesService, IRolesService rolesService, ITokenAccessor tokenAccessor, IJwtService jwtService) : Controller
    {
        private readonly IProjectsService _projectsService = projectsService;
        private readonly ITasksService _tasksService = tasksService;
        private readonly IProjectResourcesService _projectResourcesService = projectResourcesService;
        private readonly IUserRolesService _userRolesService = userRolesService;
        private readonly IRolesService _rolesService = rolesService;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly IJwtService _jwtService = jwtService;

        // GET: ProjectsController
        public async Task<ActionResult> Index()
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var response = await _projectsService.GetProjectListAsync();
            if (!response.IsSuccessful)
            {
                return NotFound(response.ErrorMessage);
            }
            
            foreach (var p in response.Data!)
            {
                var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, p.Id);
                if (userroleResponse.IsSuccessful && userroleResponse.Data != null)
                {
                    p.UserRole = userroleResponse.Data.RoleName;
                }
            }

            return View(response.Data);
        }

        // GET: ProjectsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var projectResponse = await _projectsService.GetByIdAsync(id);
            if (!projectResponse.IsSuccessful)
            {
                return NotFound(projectResponse.ErrorMessage);
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, id);
            if (userroleResponse.IsSuccessful && userroleResponse.Data != null)
            {
                projectResponse.Data!.UserRole = userroleResponse.Data.RoleName;
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

                if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
                {
                    return Unauthorized();
                }

                var rolesResponse = await _rolesService.GetAllAsync();
                if (!rolesResponse.IsSuccessful)
                {
                    return NotFound(rolesResponse.ErrorMessage);
                }

                await _projectsService.CreateAsync(project);
                await _userRolesService.CreateAsync(new UserRole()
                {
                    ProjectId = project.Id,
                    RoleId = rolesResponse.Data!.FirstOrDefault(r => r.Name == "Admin")!.Id,
                    UserId = loggedUserId
                });

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
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, id);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || userroleResponse.Data.RoleName != "Admin")
            {
                return Unauthorized();
            }

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
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, id);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || userroleResponse.Data.RoleName != "Admin")
            {
                return Unauthorized();
            }

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
