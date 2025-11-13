using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.UserProfiles
{
    internal class UserProfileRepository(NpgsqlConnection connection) : IUserProfileRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(UserProfile userProfile)
        {
            const string procedureName = "create_userprofile";
            return await _connection.CreateDBEntity(procedureName, userProfile.Phone, userProfile.Address, userProfile.DateOfBirth, userProfile.ProfilePicture, userProfile.UserId);
        }

        public async Task<UserProfile> GetByIdAsync(int id)
        {
            const string procedureName = "get_userprofile";
            return await _connection.QueryDBEntity(procedureName, reader => new UserProfile
            {
                UserId = reader.GetInt32(0),
                Phone = reader.GetString(3),
                Address = reader.GetString(4),
                DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(5)),
                ProfilePicture = reader.GetString(6)
            }, id);
        }

        public async Task<bool> UpdateAsync(UserProfile userProfile)
        {
            const string procedureName = "update_profile";
            return await _connection.CreateDBEntity(procedureName, userProfile.UserId, userProfile.Phone, userProfile.Address, userProfile.DateOfBirth, userProfile.ProfilePicture);
        }
    }
}
