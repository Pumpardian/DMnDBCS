using DMnDBCS.Domain.Entities;
using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.Roles;
using DMnDBCS.UI.Services.UserRoles;
using DMnDBCS.UI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace DMnDBCS.UI.Controllers
{
    public class UserRolesController(IUserRolesService userRolesService, IRolesService rolesService, IUsersService usersService, IJwtService jwtService) : Controller
    {
        private readonly IUserRolesService _userRolesService = userRolesService;
        private readonly IRolesService _rolesService = rolesService;
        private readonly IUsersService _usersService = usersService;
        private readonly IJwtService _jwtService = jwtService;

        // GET: UserRolesController/Create
        public async Task<ActionResult> Create(int projectId)
        {
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, projectId);
            if (!userroleResponse.IsSuccessful || userroleResponse.Data == null || userroleResponse.Data.RoleName != "Admin")
            {
                return Unauthorized();
            }

            var roleResponse = await _rolesService.GetAllAsync();
            if (!roleResponse.IsSuccessful)
            {
                return NotFound(roleResponse.ErrorMessage);
            }

            var userResponse = await _usersService.GetAllNotInProjectAsync(projectId);
            if (!userResponse.IsSuccessful)
            {
                return NotFound(userResponse.ErrorMessage);
            }

            ViewBag.Roles = roleResponse.Data;
            ViewBag.Users = userResponse.Data;

            return View(new UserRole() { ProjectId = projectId });
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
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var loggedUserRoleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, projectId);
            if (!loggedUserRoleResponse.IsSuccessful || loggedUserRoleResponse.Data == null
                || (loggedUserRoleResponse.Data.RoleName != "Admin" && loggedUserRoleResponse.Data.RoleName != "Project Manager"))
            {
                return Unauthorized();
            }

            var roleResponse = await _rolesService.GetAllAsync();
            if (!roleResponse.IsSuccessful)
            {
                return NotFound(roleResponse.ErrorMessage);
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(userId, projectId);
            if (!userroleResponse.IsSuccessful)
            {
                return NotFound(userroleResponse.ErrorMessage);
            }

            ViewBag.Roles = roleResponse.Data;

            return View(userroleResponse.Data);
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
            if (!int.TryParse(_jwtService.GetUserId(), out int loggedUserId))
            {
                return Unauthorized();
            }

            var loggedUserRoleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(loggedUserId, projectId);
            if (!loggedUserRoleResponse.IsSuccessful || loggedUserRoleResponse.Data == null
                || (loggedUserRoleResponse.Data.RoleName != "Admin" && userId != loggedUserId))
            {
                return Unauthorized();
            }

            var userroleResponse = await _userRolesService.GetByIdUserAndProjectIdsAsync(userId, projectId);
            if (!userroleResponse.IsSuccessful)
            {
                return NotFound(userroleResponse.ErrorMessage);
            }

            return View(userroleResponse.Data);
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
