using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.Users
{
    internal class UserRepository(NpgsqlConnection connection) : IUserRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            const string procedureName = "authenticate_user";
            return await _connection.QueryDBEntity(procedureName, reader => new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = email
            }, email, password);
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            const string procedureName = "create_user";
            return await _connection.CreateDBEntity(procedureName, user.Name, user.Email, password, user.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_user";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            const string procedureName = "get_users";

            return await _connection.QueryDBEntities(procedureName, reader => new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }

        public async Task<User> GetByIdAsync(int id)
        {
            const string procedureName = "get_user";
            return await _connection.QueryDBEntity(procedureName, reader => new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2)
            }, id);
        }

        public async Task<bool> UpdateAsync(User user, string? newPassword)
        {
            const string procedureName = "update_user";
            return await _connection.UpdateDBEntity(procedureName, user.Id, user.Name, user.Email, newPassword);
        }
    }
}
