using DMnDBCS.UI.Services.ProjectResources;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Controllers
{
    public class ProjectResourcesController(IProjectResourcesService projectResourcesService) : Controller
    {
        private readonly IProjectResourcesService _projectResourcesService = projectResourcesService;

        // GET: ProjectResourcesController/Create
        public ActionResult Create(int projectId)
        {
            ViewBag.ProjectId = projectId;

            return View();
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
            var response = await _projectResourcesService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
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
            var response = await _projectResourcesService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: ProjectResourcesController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _projectResourcesService.DeleteAsync(id);

                return RedirectToAction("Details", "Projects", new { id = ViewBag.ProjectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
