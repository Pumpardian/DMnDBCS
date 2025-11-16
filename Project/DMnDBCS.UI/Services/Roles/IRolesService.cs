namespace DMnDBCS.UI.Services.Roles
{
    public interface IRolesService
    {
        Task<ResponseData<List<Role>>> GetAllAsync();
    }
}
