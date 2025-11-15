using DMnDBCS.Domain.Entities;
using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.UserRoles
{
    public class ApiUserRolesService(HttpClient httpClient, ILogger<ApiUserRolesService> logger, ITokenAccessor tokenAccessor) : IUserRolesService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiUserRolesService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task CreateAsync(UserRole userRole)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PostAsJsonAsync(urlString, userRole);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating userrole data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int userId, int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{projectId}/{userId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.DeleteAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while deleting userrole data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<UserRole>> GetByIdUserAndProjectIdsAsync(int userId, int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{projectId}/{userId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<UserRole>.Success((await response.Content.ReadFromJsonAsync<UserRole>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving userrole data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<UserRole>.Error(msg);
        }

        public async Task<ResponseData<List<UserRole>>> GetRolesForProjectByIdAsync(int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/project/{projectId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<UserRole>>.Success((await response.Content.ReadFromJsonAsync<List<UserRole>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving userrole data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<UserRole>>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserRole userRole)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PutAsJsonAsync(urlString, userRole);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while updating userrole data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }
    }
}
