using DMnDBCS.API.Extensions;
using Npgsql;

namespace DMnDBCS.API.Repositories.Tasks
{
    internal class TaskRepository(NpgsqlConnection connection) : ITaskRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(Domain.Entities.Task task)
        {
            const string procedureName = "create_task";
            return await _connection.CreateDBEntity(procedureName, task.Title, task.Description, task.ProjectId, task.ExecutorId, task.Status, task.CreationDate, task.CompletionDate, task.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_task";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetAllForExecutorAsync(int id)
        {
            const string procedureName = "get_tasks_by_executor";
            return await _connection.QueryDBEntities(procedureName, reader => new Domain.Entities.Task
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                ProjectId = reader.GetInt32(3),
                ExecutorId = id,
                Status = reader.GetInt32(4),
                StatusName = reader.GetString(5),
                CreationDate = DateOnly.FromDateTime(reader.GetDateTime(6)),
                CompletionDate = reader.IsDBNull(7) ? null : DateOnly.FromDateTime(reader.GetDateTime(7))
            }, id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetAllForProjectAsync(int id)
        {
            const string procedureName = "get_tasks_by_project";
            return await _connection.QueryDBEntities(procedureName, reader => new Domain.Entities.Task
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                ProjectId = id,
                ExecutorId = reader.GetInt32(3),
                Status = reader.GetInt32(4),
                StatusName = reader.GetString(5),
                CreationDate = DateOnly.FromDateTime(reader.GetDateTime(6)),
                CompletionDate = reader.IsDBNull(7) ? null : DateOnly.FromDateTime(reader.GetDateTime(7))
            }, id);
        }

        public async Task<Domain.Entities.Task> GetByIdAsync(int id)
        {
            const string procedureName = "get_task";
            return await _connection.QueryDBEntity(procedureName, reader => new Domain.Entities.Task
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                ProjectId = reader.GetInt32(3),
                ExecutorId = reader.GetInt32(4),
                Status = reader.GetInt32(5),
                StatusName = reader.GetString(6),
                CreationDate = DateOnly.FromDateTime(reader.GetDateTime(7)),
                CompletionDate = reader.IsDBNull(8) ? null : DateOnly.FromDateTime(reader.GetDateTime(8))
            }, id);
        }

        public async Task<bool> UpdateAsync(Domain.Entities.Task task)
        {
            const string procedureName = "update_task";
            return await _connection.UpdateDBEntity(procedureName, task.Id, task.Title, task.Description, task.ProjectId, task.ExecutorId, task.Status, task.CreationDate, task.CompletionDate);
        }
    }
}
