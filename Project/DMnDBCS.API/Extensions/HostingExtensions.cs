using DMnDBCS.API.Repositories.Users;
using Npgsql;

namespace DMnDBCS.API.Extensions
{
    internal static class HostingExtensions
    {
        internal static void ConnectDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
                return new NpgsqlConnection(connectionString);
            });
        }

        internal static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
