using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.Notifications
{
    internal class NotificationRepository(NpgsqlConnection connection) : INotificationRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(Notification notification)
        {
            const string procedureName = "create_notification";
            return await _connection.CreateDBEntity(procedureName, notification.Message, notification.Time, notification.UserId, notification.ProjectId, notification.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_notification";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<Notification>> GetAllForProjectAsync(int id)
        {
            const string procedureName = "get_notifications_for_project";

            return await _connection.QueryDBEntities(procedureName, reader => new Notification
            {
                Id = reader.GetInt32(0),
                Message = reader.GetString(1),
                Time = reader.GetDateTime(2),
                ProjectId = id,
                UserId = null
            }, id);
        }

        public async Task<IEnumerable<Notification>> GetAllForUserAsync(int id)
        {
            const string procedureName = "get_notifications_for_user";

            return await _connection.QueryDBEntities(procedureName, reader => new Notification
            {
                Id = reader.GetInt32(0),
                Message = reader.GetString(1),
                Time = reader.GetDateTime(2),
                ProjectId = null,
                UserId = id
            }, id);
        }

        public async Task<bool> UpdateAsync(Notification notification)
        {
            const string procedureName = "update_notification";
            return await _connection.UpdateDBEntity(procedureName, notification.Id, notification.Message, notification.Time);
        }
    }
}
