namespace DMnDBCS.UI.Services.Auth
{
    public interface ITokenAccessor
    {
        void SetAuthHeaderAsync(HttpClient httpClient);
    }
}
