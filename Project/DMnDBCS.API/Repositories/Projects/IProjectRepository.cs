using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Projects
{
    internal interface IProjectRepository
    {
        Task<Project> GetByIdAsync(int id);
        Task<Project> GetLatestAsync();
        Task<IEnumerable<Project>> GetAllAsync();
        Task<bool> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
    }
}
