namespace DMnDBCS.UI.Services.Jwt
{
    public class JwtService(IHttpContextAccessor httpContextAccessor) : IJwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? GetUserId()
        {
            return GetClaimValue("nameid");
        }

        public string? GetUsername()
        {
            return GetClaimValue("unique_name");
        }

        public string? GetUserEmail()
        {
            return GetClaimValue("email");
        }

        private string? GetClaimValue(string claimType)
        {
            var token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }
            return null;
        }
    }
}
