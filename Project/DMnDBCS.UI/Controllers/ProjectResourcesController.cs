using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.UserRoles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace DMnDBCS.UI.Controllers
{
    public class ProjectResourcesController(IProjectResourcesService projectResourcesService, IUserRolesService userRolesService, IJwtService jwtService) : Controller
    {
        private readonly IProjectResourcesService _projectResourcesService = projectResourcesService;
        private readonly IUserRolesService _userRolesService = userRolesService;
        private readonly IJwtService _jwtService = jwtService;

        // GET: ProjectResourcesController/Create
        public async Task<ActionResult> Create(int projectId)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, projectId);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || (userroleResponse.Data.RoleName != "Admin" && userroleResponse.Data.RoleName != "Project Manager"))
            {
                return Unauthorized();
            }

            return View(new ProjectResource() { ProjectId = projectId });
        }

        // POST: ProjectResourcesController/Create
        [HttpPost]
        public async Task<ActionResult> Create(ProjectResource projectResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(projectResource);
                }

                if (string.IsNullOrEmpty(projectResource.Type))
                {
                    ModelState.AddModelError(nameof(projectResource.Type), "Type cannot be blank");
                    return View(projectResource);
                }

                await _projectResourcesService.CreateAsync(projectResource);

                return RedirectToAction("Details", "Projects", new { id = projectResource.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectResourcesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var response = await _projectResourcesService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, response.Data.ProjectId);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || (userroleResponse.Data.RoleName != "Admin" && userroleResponse.Data.RoleName != "Project Manager"))
            {
                return Unauthorized();
            }

            return View(response.Data);
        }

        // POST: ProjectResourcesController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ProjectResource projectResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(projectResource);
                }

                if (string.IsNullOrEmpty(projectResource.Type))
                {
                    ModelState.AddModelError(nameof(projectResource.Type), "Type cannot be blank");
                    return View(projectResource);
                }

                await _projectResourcesService.UpdateAsync(projectResource);

                return RedirectToAction("Details", "Projects", new { id = projectResource.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectResourcesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var response = await _projectResourcesService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, response.Data.ProjectId);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || (userroleResponse.Data.RoleName != "Admin" && userroleResponse.Data.RoleName != "Project Manager"))
            {
                return Unauthorized();
            }

            return View(response.Data);
        }

        // POST: ProjectResourcesController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, int projectId)
        {
            try
            {
                await _projectResourcesService.DeleteAsync(id);

                return RedirectToAction("Details", "Projects", new { id = projectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
