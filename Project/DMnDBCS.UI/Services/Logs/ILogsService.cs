namespace DMnDBCS.UI.Services.Logs
{
    public interface ILogsService
    {
        Task<ResponseData<List<Log>>> GetLogListAsync();
    }
}
