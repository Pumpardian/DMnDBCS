using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Users
{
    internal interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllNotInProject(int projectId);
        Task<bool> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User> AuthenticateAsync(string email, string password);
    }
}
