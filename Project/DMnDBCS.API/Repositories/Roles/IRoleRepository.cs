using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Roles
{
    internal interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<bool> CreateAsync(Role role);
        Task<bool> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int id);
    }
}
