using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.UserProfiles
{
    internal interface IUserProfileRepository
    {
        Task<UserProfile> GetByIdAsync(int id);
        Task<UserProfile> GetByPhoneAsync(string phone);
        Task<bool> CreateAsync(UserProfile userProfile);
        Task<bool> UpdateAsync(UserProfile userProfile);
    }
}
