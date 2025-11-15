namespace DMnDBCS.UI.Services.TaskComments
{
    public interface ITaskCommentsService
    {
        Task<ResponseData<List<TaskComment>>> GetTaskCommentsByTaskIdAsync(int taskId);
        Task<ResponseData<TaskComment>> GetByIdAsync(int id);
        System.Threading.Tasks.Task CreateAsync(TaskComment taskComment);
        System.Threading.Tasks.Task DeleteAsync(int id);
        System.Threading.Tasks.Task UpdateAsync(TaskComment taskComment);
    }
}
