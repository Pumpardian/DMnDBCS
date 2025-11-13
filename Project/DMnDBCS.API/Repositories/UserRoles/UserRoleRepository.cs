using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.UserRoles
{
    internal class UserRoleRepository(NpgsqlConnection connection) : IUserRoleRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(UserRole userRole)
        {
            const string procedureName = "create_userrole";
            return await _connection.CreateDBEntity(procedureName, userRole.UserId, userRole.ProjectId, userRole.RoleId);
        }

        public async Task<bool> DeleteAsync(int userId, int projectId)
        {
            const string procedureName = "delete_userrole";
            return await _connection.DeleteDBEntity(procedureName, userId, projectId);
        }

        //Leave that for later stages, as its precisely unknown how that will be used
        public Task<IEnumerable<UserRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(UserRole userRole)
        {
            const string procedureName = "update_userrole";
            return await _connection.CreateDBEntity(procedureName, userRole.UserId, userRole.ProjectId, userRole.RoleId);
        }
    }
}
