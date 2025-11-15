using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.ProjectResources
{
    internal class ProjectResourceRepository(NpgsqlConnection connection) : IProjectResourceRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(ProjectResource projectResource)
        {
            const string procedureName = "create_projectresource";
            return await _connection.CreateDBEntity(procedureName, projectResource.Description, projectResource.Type, projectResource.ProjectId, projectResource.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_projectresource";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<ProjectResource>> GetAllForProjectAsync(int id)
        {
            const string procedureName = "get_projectresources";

            return await _connection.QueryDBEntities(procedureName, reader => new ProjectResource
            {
                Id = reader.GetInt32(0),
                Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                Type = reader.GetString(2),
                ProjectId = id
            }, id);
        }

        public async Task<ProjectResource> GetByIdAsync(int id)
        {
            const string procedureName = "get_projectresource";

            return await _connection.QueryDBEntity(procedureName, reader => new ProjectResource
            {
                Id = reader.GetInt32(0),
                Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                Type = reader.GetString(2),
                ProjectId = reader.GetInt32(3)
            }, id);
        }

        public async Task<bool> UpdateAsync(ProjectResource projectResource)
        {
            const string procedureName = "update_projectresource";
            return await _connection.UpdateDBEntity(procedureName, projectResource.Id, projectResource.Description, projectResource.Type);
        }
    }
}
