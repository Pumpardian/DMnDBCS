namespace DMnDBCS.UI.Services.ProjectResources
{
    public interface IProjectResourcesService
    {
        Task<ResponseData<List<ProjectResource>>> GetProjectResourcesByProjectIdAsync(int projectId);
        Task<ResponseData<ProjectResource>> GetByIdAsync(int id);
        System.Threading.Tasks.Task CreateAsync(ProjectResource resource);
        System.Threading.Tasks.Task DeleteAsync(int id);
        System.Threading.Tasks.Task UpdateAsync(ProjectResource resource);
    }
}
