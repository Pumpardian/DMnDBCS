using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Users
{
    internal interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> CreateAsync(User user, string password);
        Task<bool> UpdateAsync(User user, string? newPassword = null);
        Task<bool> DeleteAsync(int id);
    }
}
