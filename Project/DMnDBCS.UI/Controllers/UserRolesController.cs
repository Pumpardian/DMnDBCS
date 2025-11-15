using DMnDBCS.Domain.Entities;
using DMnDBCS.UI.Services.UserRoles;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class UserRolesController(IUserRolesService userRolesService) : Controller
    {
        private readonly IUserRolesService _userRolesService = userRolesService;

        // GET: UserRolesController/Create
        public ActionResult Create(int projectId)
        {
            ViewBag.ProjectId = projectId;

            return View();
        }

        // POST: UserRolesController/Create
        [HttpPost]
        public async Task<ActionResult> Create(UserRole userRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userRole);
                }

                await _userRolesService.CreateAsync(userRole);

                return RedirectToAction("Details", "Projects", new { id = userRole.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRolesController/Edit/5
        public async Task<ActionResult> Edit(int userId, int projectId)
        {
            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(userId, projectId);
            if (!userroleResponse.IsSuccessful)
            {
                return NotFound(userroleResponse.ErrorMessage);
            }

            return View();
        }

        // POST: UserRolesController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UserRole userRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userRole);
                }

                await _userRolesService.UpdateAsync(userRole);

                return RedirectToAction("Details", "Projects", new { id = userRole.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRolesController/Delete/5
        public async Task<ActionResult> Delete(int userId, int projectId)
        {
            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(userId, projectId);
            if (!userroleResponse.IsSuccessful)
            {
                return NotFound(userroleResponse.ErrorMessage);
            }

            return View();
        }

        // POST: UserRolesController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int userId, int projectId, IFormCollection collection)
        {
            try
            {
                await _userRolesService.DeleteAsync(userId, projectId);

                return RedirectToAction("Details", "Projects", new { id = projectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
