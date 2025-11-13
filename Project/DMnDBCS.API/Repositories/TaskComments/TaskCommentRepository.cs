using DMnDBCS.API.Extensions;
using DMnDBCS.Domain.Entities;
using Npgsql;

namespace DMnDBCS.API.Repositories.TaskComments
{
    internal class TaskCommentRepository(NpgsqlConnection connection) : ITaskCommentRepository
    {
        private readonly NpgsqlConnection _connection = connection;

        public async Task<bool> CreateAsync(TaskComment taskComment)
        {
            const string procedureName = "create_taskcomment";
            return await _connection.CreateDBEntity(procedureName, taskComment.Content, taskComment.TaskId, taskComment.AuthorId, taskComment.CreationDate,taskComment.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string procedureName = "delete_taskcomment";
            return await _connection.DeleteDBEntity(procedureName, id);
        }

        public async Task<IEnumerable<TaskComment>> GetAllForProjectAsync(int id)
        {
            const string procedureName = "get_taskcomments";

            return await _connection.QueryDBEntities(procedureName, reader => new TaskComment
            {
                Id = reader.GetInt32(0),
                Content = reader.GetString(1),
                CreationDate = DateOnly.FromDateTime(reader.GetDateTime(2)),
                AuthorId = reader.GetInt32(3),
                TaskId = id
            }, id);
        }

        public async Task<bool> UpdateAsync(TaskComment taskComment)
        {
            const string procedureName = "update_taskcomment";
            return await _connection.UpdateDBEntity(procedureName, taskComment.Id, taskComment.Content, taskComment.CreationDate);
        }
    }
}
