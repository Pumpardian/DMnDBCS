namespace DMnDBCS.UI.Services.TaskStatuses
{
    public interface ITaskStatusesService
    {
        Task<ResponseData<List<Domain.Entities.TaskStatus>>> GetAllAsync();
    }
}
