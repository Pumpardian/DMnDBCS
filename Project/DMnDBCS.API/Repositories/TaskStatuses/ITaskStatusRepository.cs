namespace DMnDBCS.API.Repositories.TaskStatuses
{
    internal interface ITaskStatusRepository
    {
        Task<IEnumerable<Domain.Entities.TaskStatus>> GetAllAsync();
    }
}
