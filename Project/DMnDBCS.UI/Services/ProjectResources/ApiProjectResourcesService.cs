using DMnDBCS.UI.Services.Auth;
using Elfie.Serialization;
using Microsoft.CodeAnalysis;
using System.Text.Json;

namespace DMnDBCS.UI.Services.ProjectResources
{
    public class ApiProjectResourcesService(HttpClient httpClient, ILogger<ApiProjectResourcesService> logger, ITokenAccessor tokenAccessor) : IProjectResourcesService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiProjectResourcesService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task CreateAsync(ProjectResource resource)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PostAsJsonAsync(urlString, resource);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating resource data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.DeleteAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while deleting resource data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<ProjectResource>> GetByIdAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<ProjectResource>.Success((await response.Content.ReadFromJsonAsync<ProjectResource>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving resource data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<ProjectResource>.Error(msg);
        }

        public async Task<ResponseData<List<ProjectResource>>> GetProjectResourcesByProjectIdAsync(int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/project/{projectId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<ProjectResource>>.Success((await response.Content.ReadFromJsonAsync<List<ProjectResource>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving resource data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<ProjectResource>>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ProjectResource resource)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{resource.Id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PutAsJsonAsync(urlString, resource);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while updating resource data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }
    }
}
