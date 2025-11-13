using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.Logs
{
    internal class LogRepository(NpgsqlConnection connection) : ILogRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            const string procedureName = "get_all_logs";

            return await _connection.QueryDBEntities(procedureName, reader => new Log
            {
                Id = reader.GetInt32(0),
                Action = reader.GetString(1),
                Date = reader.GetDateTime(2),
                UserName = reader.GetString(3)
            });
        }
    }
}
