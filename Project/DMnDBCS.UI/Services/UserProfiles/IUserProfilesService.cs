namespace DMnDBCS.UI.Services.UserProfiles
{
    public interface IUserProfilesService
    {
        Task<ResponseData<UserProfile>> GetByIdAsync(int id);
        Task<ResponseData<UserProfile>> GetByPhoneAsync(string phone);
        System.Threading.Tasks.Task CreateAsync(UserProfile profile, IFormFile? formFile);
        System.Threading.Tasks.Task UpdateAsync(UserProfile profile, IFormFile? formFile);
    }
}
