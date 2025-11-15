using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.TaskStatuses
{
    public class ApiTaskStatusesService(HttpClient httpClient, ILogger<ApiTaskStatusesService> logger, ITokenAccessor tokenAccessor) : ITaskStatusesService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiTaskStatusesService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<ResponseData<List<Domain.Entities.TaskStatus>>> GetAllAsync()
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Domain.Entities.TaskStatus>>.Success((await response.Content.ReadFromJsonAsync<List<Domain.Entities.TaskStatus>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving taskstatus data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Domain.Entities.TaskStatus>>.Error(msg);
        }
    }
}
