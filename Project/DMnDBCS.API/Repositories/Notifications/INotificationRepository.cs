using DMnDBCS.Domain.Entities;

namespace DMnDBCS.API.Repositories.Notifications
{
    internal interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllForUserAsync(int id);
        Task<IEnumerable<Notification>> GetAllForProjectAsync(int id);
        Task<bool> CreateAsync(Notification notification);
        Task<bool> UpdateAsync(Notification notification);
        Task<bool> DeleteAsync(int id);
    }
}
