namespace DMnDBCS.API.Repositories.Tasks
{
    internal interface ITaskRepository
    {
        Task<Domain.Entities.Task> GetByIdAsync(int id);
        Task<IEnumerable<Domain.Entities.Task>> GetAllForProjectAsync(int id);
        Task<IEnumerable<Domain.Entities.Task>> GetAllForExecutorAsync(int id);
        Task<bool> CreateAsync(Domain.Entities.Task task);
        Task<bool> UpdateAsync(Domain.Entities.Task task);
        Task<bool> DeleteAsync(int id);
    }
}
