using DMnDBCS.API.Repositories.Logs;
using DMnDBCS.API.Repositories.ProjectResources;
using DMnDBCS.API.Repositories.Projects;
using DMnDBCS.API.Repositories.Roles;
using DMnDBCS.API.Repositories.TaskComments;
using DMnDBCS.API.Repositories.Tasks;
using DMnDBCS.API.Repositories.TaskStatuses;
using DMnDBCS.API.Repositories.UserProfiles;
using DMnDBCS.API.Repositories.UserRoles;
using DMnDBCS.API.Repositories.Users;
using DMnDBCS.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

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
            builder.Services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ILogRepository, LogRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectResourceRepository, ProjectResourceRepository>()
                .AddScoped<ITaskRepository, TaskRepository>()
                .AddScoped<ITaskCommentRepository, TaskCommentRepository>()
                .AddScoped<ITaskStatusRepository, TaskStatusRepository>()
                .AddScoped<IUserRoleRepository, UserRoleRepository>()
                .AddScoped<IUserProfileRepository, UserProfileRepository>()
                .AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddSingleton<IImageService, ImageService>();
            builder.Services.AddHttpContextAccessor();
        }

        internal static void SetupAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ITokenService, TokenService>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebUI", policy =>
                {
                    policy.WithOrigins("https://localhost:7008")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }
}
