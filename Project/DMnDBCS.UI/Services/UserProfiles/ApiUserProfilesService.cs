using DMnDBCS.UI.Services.Auth;
using System.Text;
using System.Text.Json;

namespace DMnDBCS.UI.Services.UserProfiles
{
    public class ApiUserProfilesService(HttpClient httpClient, ILogger<ApiUserProfilesService> logger, ITokenAccessor tokenAccessor) : IUserProfilesService
    {
        private readonly HttpClient _client = httpClient;
        private readonly ILogger<ApiUserProfilesService> _logger = logger;
        private readonly ITokenAccessor _tokenAccessor = tokenAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async System.Threading.Tasks.Task CreateAsync(UserProfile profile, IFormFile? formFile)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri;

            using var request = new HttpRequestMessage(HttpMethod.Post, urlString);
            using var content = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(profile, _jsonSerializerOptions);
            var profileContent = new StringContent(json, Encoding.UTF8, "application/json");
            content.Add(profileContent, "profile");

            if (formFile != null && formFile.Length > 0)
            {
                var streamContent = new StreamContent(formFile.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType);
                content.Add(streamContent, "file", formFile.FileName);
            }

            request.Content = content;

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.SendAsync(request, CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var msg = $"Error while creating profile data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            throw new HttpRequestException(msg);
        }

        public async Task<ResponseData<UserProfile>> GetByIdAsync(int id)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{id}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<UserProfile>.Success((await response.Content.ReadFromJsonAsync<UserProfile>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving profile data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<UserProfile>.Error(msg);
        }

        public async Task<ResponseData<UserProfile>> GetByPhoneAsync(string phone)
        {
            var urlString = _client.BaseAddress!.AbsoluteUri + $"/{phone}";

            _tokenAccessor.SetAuthHeaderAsync(_client);

            var response = await _client.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return ResponseData<UserProfile>.Success((await response.Content.ReadFromJsonAsync<UserProfile>(_jsonSerializerOptions))!);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            var msg = $"Error while receiving profile data. Error: {response.StatusCode}";
            _logger.LogError(msg);
            return ResponseData<UserProfile>.Error(msg);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserProfile profile, IFormFile? formFile)
        {
            try
            {
                var urlString = _client.BaseAddress!.AbsoluteUri + $"/{profile.UserId}";

                using var request = new HttpRequestMessage(HttpMethod.Put, urlString);
                using var content = new MultipartFormDataContent();

                var json = JsonSerializer.Serialize(profile, _jsonSerializerOptions);
                var profileContent = new StringContent(json, Encoding.UTF8, "application/json");
                content.Add(profileContent, "profile");

                if (formFile != null && formFile.Length > 0)
                {
                    var streamContent = new StreamContent(formFile.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType);
                    content.Add(streamContent, "file", formFile.FileName);
                }

                request.Content = content;

                _tokenAccessor.SetAuthHeaderAsync(_client);

                var response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                var msg = $"Error while updating profile data. Status: {response.StatusCode}";
                _logger.LogError(msg);
                throw new HttpRequestException(msg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
