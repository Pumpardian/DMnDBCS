using System.Net.Http.Headers;

namespace DMnDBCS.UI.Services.Auth
{
    public class TokenAccessor(IHttpContextAccessor contextAccessor) : ITokenAccessor
    {
        private readonly HttpContext _context = contextAccessor.HttpContext!;

        public void SetAuthHeaderAsync(HttpClient httpClient)
        {
            var token = _context.Session.GetString("JWTToken");

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return;
            }

            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
