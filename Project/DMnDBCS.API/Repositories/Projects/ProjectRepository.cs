using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.Projects
{
    internal class ProjectRepository(NpgsqlConnection connection) : IProjectRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(Project project)
        {
            const string procedureName = "create_project";
            return await _connection.CreateDBEntity(procedureName, project.Title, project.Description, project.StartDate, project.EndDate, project.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_project";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            const string procedureName = "get_all_projects";
            return await _connection.QueryDBEntities(procedureName, reader => new Project
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                EndDate = reader.IsDBNull(4) ? null : DateOnly.FromDateTime(reader.GetDateTime(4))
            });
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            const string procedureName = "get_project";
            return await _connection.QueryDBEntity(procedureName, reader => new Project
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                EndDate = reader.IsDBNull(4) ? null : DateOnly.FromDateTime(reader.GetDateTime(4))
            }, id);
        }

        public async Task<Project> GetLatestAsync()
        {
            const string procedureName = "get_latest_project";
            return await _connection.QueryDBEntity(procedureName, reader => new Project
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                EndDate = reader.IsDBNull(4) ? null : DateOnly.FromDateTime(reader.GetDateTime(4))
            });
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            const string procedureName = "update_project";
            return await _connection.UpdateDBEntity(procedureName, project.Id, project.Title, project.Description, project.StartDate, project.EndDate);
        }
    }
}
