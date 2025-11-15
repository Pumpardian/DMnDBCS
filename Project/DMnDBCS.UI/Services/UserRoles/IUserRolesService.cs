namespace DMnDBCS.UI.Services.UserRoles
{
    public interface IUserRolesService
    {
        Task<ResponseData<List<UserRole>>> GetRolesForProjectByIdAsync(int projectId);
        Task<ResponseData<UserRole>> GetByIdUserAndProjectIdsAsync(int userId, int projectId);
        System.Threading.Tasks.Task CreateAsync(UserRole userRole);
        System.Threading.Tasks.Task UpdateAsync(UserRole userRole);
        System.Threading.Tasks.Task DeleteAsync(int userId, int projectId);
    }
}
