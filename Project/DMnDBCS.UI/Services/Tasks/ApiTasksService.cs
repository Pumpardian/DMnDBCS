using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.Tasks
{
    public class ApiTasksService(HttpClient httpClient, ILogger<ApiTasksService> logger, ITokenAccessor tokenAccessor) : ITasksService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiTasksService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task CreateAsync(Domain.Entities.Task task)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PostAsJsonAsync(urlString, task);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int taskId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{taskId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.DeleteAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while deleting task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<Domain.Entities.Task>> GetByIdAsync(int taskId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{taskId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<Domain.Entities.Task>.Success((await response.Content.ReadFromJsonAsync<Domain.Entities.Task>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<Domain.Entities.Task>.Error(msg);
        }

        public async Task<ResponseData<List<Domain.Entities.Task>>> GetTasksForExecutorAsync(int executorId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/executor/{executorId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Domain.Entities.Task>>.Success((await response.Content.ReadFromJsonAsync<List<Domain.Entities.Task>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Domain.Entities.Task>>.Error(msg);
        }

        public async Task<ResponseData<List<Domain.Entities.Task>>> GetTasksForProjectAsync(int projectId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/project/{projectId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<Domain.Entities.Task>>.Success((await response.Content.ReadFromJsonAsync<List<Domain.Entities.Task>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving task data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<Domain.Entities.Task>>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task task)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{task.Id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PutAsJsonAsync(urlString, task);
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
