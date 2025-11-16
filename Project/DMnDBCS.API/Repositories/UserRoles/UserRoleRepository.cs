using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Microsoft.CodeAnalysis;
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

        
        public async Task<IEnumerable<UserRole>> GetAllForProjectAsync(int projectId)
        {
            const string procedureName = "get_project_members";

            return await _connection.QueryDBEntities(procedureName, reader => new UserRole
            {
                UserId = reader.GetInt32(0),
                RoleId = reader.GetInt32(1),
                UserName = reader.GetString(2),
                RoleName = reader.GetString(3),
                ProjectId = projectId
            }, projectId);
        }

        public async Task<UserRole> GetByUserAndProjectIdsAsync(int userId, int projectId)
        {
            const string procedureName = "get_user_project_role";

            return await _connection.QueryDBEntity(procedureName, reader => new UserRole
            {
                UserId = userId,
                RoleId = reader.GetInt32(0),
                RoleName = reader.GetString(1),
                ProjectId = projectId
            }, userId, projectId);
        }

        public async Task<bool> UpdateAsync(UserRole userRole)
        {
            const string procedureName = "update_userrole";
            return await _connection.CreateDBEntity(procedureName, userRole.UserId, userRole.ProjectId, userRole.RoleId);
        }
    }
}
