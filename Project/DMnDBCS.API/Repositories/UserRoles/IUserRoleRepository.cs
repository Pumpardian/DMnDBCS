using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.UserRoles
{
    internal interface IUserRoleRepository
    {
        Task<UserRole> GetByIdAsync(int id);
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<bool> CreateAsync(UserRole userRole);
        Task<bool> UpdateAsync(UserRole userRole);
        Task<bool> DeleteAsync(int userId, int projectId);
    }
}
