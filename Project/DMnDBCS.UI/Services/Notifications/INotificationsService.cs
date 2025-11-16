namespace DMnDBCS.UI.Services.Notifications
{
    public interface INotificationsService
    {
        Task<ResponseData<List<Notification>>> GetAllForUserAsync(int userId);
        Task<ResponseData<Notification>> GetByIdAsync(int id);
        System.Threading.Tasks.Task CreateAsync(Notification notification);
        System.Threading.Tasks.Task DeleteAsync(int id);
        System.Threading.Tasks.Task UpdateAsync(Notification notification);
    }
}
