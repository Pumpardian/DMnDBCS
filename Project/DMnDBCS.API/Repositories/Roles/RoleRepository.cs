using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.Roles
{
    internal class RoleRepository(NpgsqlConnection connection) : IRoleRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(Role role)
        {
            const string procedureName = "create_role";
            return await _connection.CreateDBEntity(procedureName, role.Name, role.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_role";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            const string procedureName = "get_all_roles";

            return await _connection.QueryDBEntities(procedureName, reader => new Role
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        public async Task<bool> UpdateAsync(Role role)
        {
            const string procedureName = "update_role";
            return await _connection.UpdateDBEntity(procedureName, role.Id, role.Name);
        }
    }
}
