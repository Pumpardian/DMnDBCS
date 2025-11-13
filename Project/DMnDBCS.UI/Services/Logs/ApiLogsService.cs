using System.Text.Json;

namespace DMnDBCS.UI.Services.Logs
{
    public class ApiLogsService(HttpClient httpClient, ILogger<ApiLogsService> logger) : ILogsService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiLogsService> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<ResponseData<List<Log>>> GetLogListAsync()
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Log>>.Success((await response.Content.ReadFromJsonAsync<List<Log>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving log data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Log>>.Error(msg);
        }
    }
}
