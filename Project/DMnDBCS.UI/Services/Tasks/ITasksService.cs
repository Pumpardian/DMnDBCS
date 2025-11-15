namespace DMnDBCS.UI.Services.Tasks
{
    public interface ITasksService
    {
        Task<ResponseData<List<Domain.Entities.Task>>> GetTasksForProjectAsync(int projectId);
        Task<ResponseData<List<Domain.Entities.Task>>> GetTasksForExecutorAsync(int executorId);
        Task<ResponseData<Domain.Entities.Task>> GetByIdAsync(int taskId);
        System.Threading.Tasks.Task CreateAsync(Domain.Entities.Task task);
        System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task task);
        System.Threading.Tasks.Task DeleteAsync(int taskId);
    }
}
