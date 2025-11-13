using DMnDBCS.API.Extensions;
using Npgsql;

namespace DMnDBCS.API.Repositories.TaskStatuses
{
    internal class TaskStatusRepository(NpgsqlConnection connection) : ITaskStatusRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<IEnumerable<Domain.Entities.TaskStatus>> GetAllAsync()
        {
            const string procedureName = "get_all_statuses";

            return await _connection.QueryDBEntities(procedureName, reader => new Domain.Entities.TaskStatus
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
    }
}
