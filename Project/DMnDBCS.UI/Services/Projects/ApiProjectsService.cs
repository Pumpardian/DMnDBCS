using DMnDBCS.UI.Services.Logs;
using System.Text.Json;

namespace DMnDBCS.UI.Services.Projects
{
    public class ApiProjectsService(HttpClient httpClient, ILogger<ApiProjectsService> logger) : IProjectsService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiProjectsService> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new () { PropertyNamingPolicy  = JsonNamingPolicy.CamelCase };

        public async Task<ResponseData<Project>> GetByIdAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<Project>.Success((await response.Content.ReadFromJsonAsync<Project>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving project data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<Project>.Error(msg);
        }

        public async Task<ResponseData<List<Project>>> GetProjectListAsync()
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Project>>.Success((await response.Content.ReadFromJsonAsync<List<Project>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving project data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Project>>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Project project)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{project.Id}";

            var response = await _client.PutAsJsonAsync(urlString, project);
            if (response.IsSuccessStatusCode)
            {
               return;
            }

            var msg = $"Error while updating project data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async System.Threading.Tasks.Task CreateAsync(Project project)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            var response = await _client.PostAsJsonAsync(urlString, project);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating project data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            var response = await _client.DeleteAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while deleting project data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }
    }
}
