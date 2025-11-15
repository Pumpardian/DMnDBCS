namespace DMnDBCS.UI.Services.Jwt
{
    public interface IJwtService
    {
        string? GetUserId();
        string? GetUsername();
        string? GetUserEmail();
    }
}
