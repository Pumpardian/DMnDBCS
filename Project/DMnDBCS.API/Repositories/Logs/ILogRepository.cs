using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Logs
{
    internal interface ILogRepository
    {
        Task<IEnumerable<Log>> GetAllAsync();
    }
}
