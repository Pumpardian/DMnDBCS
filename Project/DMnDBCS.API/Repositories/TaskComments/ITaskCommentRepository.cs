using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.TaskComments
{
    internal interface ITaskCommentRepository
    {
        Task<IEnumerable<TaskComment>> GetAllForProjectAsync(int id);
        Task<TaskComment> GetByIdAsync(int id);
        Task<bool> CreateAsync(TaskComment taskComment);
        Task<bool> UpdateAsync(TaskComment taskComment);
        Task<bool> DeleteAsync(int id);
    }
}
