using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.Notifications
{
    public class ApiNotificationsService(HttpClient httpClient, ILogger<ApiNotificationsService> logger, ITokenAccessor tokenAccessor) : INotificationsService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiNotificationsService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public System.Threading.Tasks.Task CreateAsync(Notification notification)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<Notification>>> GetAllForUserAsync(int userId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/user/{userId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Notification>>.Success((await response.Content.ReadFromJsonAsync<List<Notification>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving notification data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Notification>>.Error(msg);
        }

        public Task<ResponseData<Notification>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateAsync(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
