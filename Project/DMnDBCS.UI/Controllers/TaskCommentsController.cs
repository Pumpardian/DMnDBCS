using DMnDBCS.Domain.Entities;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.TaskComments;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DMnDBCS.UI.Controllers
{
    public class TaskCommentsController(ITaskCommentsService taskCommentsService) : Controller
    {
        private readonly ITaskCommentsService _taskCommentsService = taskCommentsService;

        // GET: TaskCommentsController/Create
        public ActionResult Create(int taskId)
        {
            return View(new TaskComment() { TaskId = taskId });
        }

        // POST: TaskCommentsController/Create
        [HttpPost]
        public async Task<ActionResult> Create(TaskComment taskComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(taskComment);
                }

                if (string.IsNullOrEmpty(taskComment.Content))
                {
                    ModelState.AddModelError(nameof(taskComment.Content), "Content cannot be blank");
                    return View(taskComment);
                }

                if (string.IsNullOrEmpty(taskComment.CreationDate.ToString()))
                {
                    ModelState.AddModelError(nameof(taskComment.CreationDate), "Creation date cannot be blank");
                    return View(taskComment);
                }

                taskComment.CreationDate = DateOnly.FromDateTime(DateTime.Today);

                await _taskCommentsService.CreateAsync(taskComment);

                return RedirectToAction("Details", "Tasks", new { id = taskComment.TaskId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskCommentsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _taskCommentsService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: TaskCommentsController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(TaskComment taskComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(taskComment);
                }

                if (string.IsNullOrEmpty(taskComment.Content))
                {
                    ModelState.AddModelError(nameof(taskComment.Content), "Content cannot be blank");
                    return View(taskComment);
                }

                if (string.IsNullOrEmpty(taskComment.CreationDate.ToString()))
                {
                    ModelState.AddModelError(nameof(taskComment.CreationDate), "Creation date cannot be blank");
                    return View(taskComment);
                }

                await _taskCommentsService.UpdateAsync(taskComment);

                return RedirectToAction("Details", "Tasks", new { id = taskComment.TaskId });
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskCommentsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _taskCommentsService.GetByIdAsync(id);
            if (!response.IsSuccessful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            return View(response.Data);
        }

        // POST: TaskCommentsController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, int taskId)
        {
            try
            {
                await _taskCommentsService.DeleteAsync(id);

                return RedirectToAction("Details", "Tasks", new { id = taskId });
            }
            catch
            {
                return View();
            }
        }
    }
}
