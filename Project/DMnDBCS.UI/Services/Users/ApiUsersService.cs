using DMnDBCS.UI.Services.Auth;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.Json;

namespace DMnDBCS.UI.Services.Users
{
    public class ApiUsersService(HttpClient httpClient, ILogger<ApiUsersService> logger, ITokenAccessor tokenAccessor) : IUsersService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiUsersService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.DeleteAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while deleting user data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<List<User>>> GetAllInProjectAsync(int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/in-project/{projectId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<User>>.Success((await response.Content.ReadFromJsonAsync<List<User>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving user data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<User>>.Error(msg);
        }

        public async Task<ResponseData<List<User>>> GetAllNotInProjectAsync(int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/not-in-project/{projectId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<User>>.Success((await response.Content.ReadFromJsonAsync<List<User>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving user data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<User>>.Error(msg);
        }

        public async Task<ResponseData<User>> GetByEmailAsync(string email)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{email}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<User>.Success((await response.Content.ReadFromJsonAsync<User>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving user data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<User>.Error(msg);
        }

        public async Task<ResponseData<User>> GetByIdAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<User>.Success((await response.Content.ReadFromJsonAsync<User>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving user data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<User>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(User user)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{user.Id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PutAsJsonAsync(urlString, user);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while updating task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }
    }
}
