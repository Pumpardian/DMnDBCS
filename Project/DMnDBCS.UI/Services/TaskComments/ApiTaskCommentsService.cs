using DMnDBCS.UI.Services.Auth;
using System.Text.Json;

namespace DMnDBCS.UI.Services.TaskComments
{
    public class ApiTaskCommentsService(HttpClient httpClient, ILogger<ApiTaskCommentsService> logger, ITokenAccessor tokenAccessor) : ITaskCommentsService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiTaskCommentsService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task CreateAsync(TaskComment taskComment)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PostAsJsonAsync(urlString, taskComment);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating taskcomment data. Error: {response.StatusCode}";
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

            var msg = $"Error while deleting taskcomment data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<TaskComment>> GetByIdAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<TaskComment>.Success((await response.Content.ReadFromJsonAsync<TaskComment>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving taskcomment data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<TaskComment>.Error(msg);
        }

        public async Task<ResponseData<List<TaskComment>>> GetTaskCommentsByTaskIdAsync(int taskId)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/task/{taskId}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<List<TaskComment>>.Success((await response.Content.ReadFromJsonAsync<List<TaskComment>>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving taskcomment data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<List<TaskComment>>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(TaskComment taskComment)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{taskComment.Id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.PutAsJsonAsync(urlString, taskComment);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while updating taskcomment data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }
    }
}
