namespace DMnDBCS.UI.Services.Jwt
{
    public interface IJwtService
    {
        string? GetUserId();
        string? GetUsername();
        string? GetUserEmail();
        bool GetFromProject();
        void SetFromProject(bool data);
    }
}
