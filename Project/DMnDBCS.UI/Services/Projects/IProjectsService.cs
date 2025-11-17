namespace DMnDBCS.UI.Services.Projects
{
    public interface IProjectsService
    {
        Task<ResponseData<List<Project>>> GetProjectListAsync();
        Task<ResponseData<Project>> GetByIdAsync(int id);
        Task<ResponseData<Project>> GetLatestAsync();
        System.Threading.Tasks.Task UpdateAsync(Project project);
        System.Threading.Tasks.Task CreateAsync(Project project);
        System.Threading.Tasks.Task DeleteAsync(int id);
    }
}
