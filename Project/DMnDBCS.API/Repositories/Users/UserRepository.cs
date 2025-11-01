using DMnDBCS.Domain.Entities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Npgsql;

namespace DMnDBCS.API.Repositories.Users
{
    internal class UserRepository(NpgsqlConnection connection) : IUserRepository
    {
        private readonly NpgsqlConnection _connection = connection;
        public async Task<int> CreateAsync(User user, string password)
        {
            const string procedureName = "create_user";
            await _connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({user.Name}, {user.Email}, {password}, {user.Id});";
                using var command = new NpgsqlCommand(sql, _connection);
                await command.ExecuteNonQueryAsync();
                return user.Id;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_user";
            await _connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({id});";

                using var command = new NpgsqlCommand(sql, _connection);
                return await command.ExecuteNonQueryAsync() != 0;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            const string procedureName = "get_users";
            var users = new List<User>();
            await _connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}('cursor'); FETCH ALL FROM cursor;";
                using var command = new NpgsqlCommand(sql, _connection);
                using var reader = await command.ExecuteReaderAsync();

                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2)
                    });
                }

                return users;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally 
            {
                _connection.Close();
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            const string procedureName = "get_user";
            await _connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({id}, 'cursor'); FETCH ALL FROM cursor;";
                using var command = new NpgsqlCommand(sql, _connection);
                using var reader = await command.ExecuteReaderAsync();

                await reader.NextResultAsync();
                if (await reader.ReadAsync())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2)
                    };
                    return user;
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> UpdateAsync(User user, string? newPassword)
        {
            const string procedureName = "update_user";
            await _connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({user.Id}, {user.Name}, {user.Email}, {newPassword};";
                using var command = new NpgsqlCommand(sql, _connection);
                return await command.ExecuteNonQueryAsync() != 0;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
