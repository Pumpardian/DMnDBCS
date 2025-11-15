using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.UserRoles
{
    internal interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllForProjectAsync(int projectId);
        Task<UserRole> GetByIdUserAndProjectIdsAsync(int userId, int projectId);
        Task<bool> CreateAsync(UserRole userRole);
        Task<bool> UpdateAsync(UserRole userRole);
        Task<bool> DeleteAsync(int userId, int projectId);
    }
}
