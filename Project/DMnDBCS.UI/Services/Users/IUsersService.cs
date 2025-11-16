namespace DMnDBCS.UI.Services.Users
{
    public interface IUsersService
    {
        Task<ResponseData<User>> GetByIdAsync(int id);
        Task<ResponseData<User>> GetByEmailAsync(string email);
        Task<ResponseData<List<User>>> GetAllNotInProjectAsync(int projectId);
        Task<ResponseData<List<User>>> GetAllInProjectAsync(int projectId);
        System.Threading.Tasks.Task UpdateAsync(User user);
        System.Threading.Tasks.Task DeleteAsync(int id);
    }
}
