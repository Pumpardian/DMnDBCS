using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.ProjectResources
{
    internal interface IProjectResourceRepository
    {
        Task<IEnumerable<ProjectResource>> GetAllForProjectAsync(int id);
        Task<bool> CreateAsync(ProjectResource projectResource);
        Task<bool> UpdateAsync(ProjectResource projectResource);
        Task<bool> DeleteAsync(int id);
    }
}
