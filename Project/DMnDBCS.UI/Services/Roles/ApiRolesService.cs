using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.Roles
{
    public class ApiRolesService(HttpClient httpClient, ILogger<ApiRolesService> logger, ITokenAccessor tokenAccessor) : IRolesService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiRolesService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task<ResponseData<List<Role>>> GetAllAsync()
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Role>>.Success((await response.Content.ReadFromJsonAsync<List<Role>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving role data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Role>>.Error(msg);
        }
    }
}
