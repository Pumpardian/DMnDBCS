using DMnDBCS.UI.Services.Auth;
using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.Logs;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.Projects;
using DMnDBCS.UI.Services.Roles;
using DMnDBCS.UI.Services.TaskComments;
using DMnDBCS.UI.Services.Tasks;
using DMnDBCS.UI.Services.TaskStatuses;
using DMnDBCS.UI.Services.UserProfiles;
using DMnDBCS.UI.Services.UserRoles;
using DMnDBCS.UI.Services.Users;

namespace DMnDBCS.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder webapplicationBuilder)
        {
            var api = webapplicationBuilder.Configuration.GetValue<string>("UriData:ApiUri");

            webapplicationBuilder.Services
                .AddHttpClient<ILogsService, ApiLogsService>(opt => opt.BaseAddress = new Uri(api + "logs"));
            webapplicationBuilder.Services
                .AddHttpClient<IProjectsService, ApiProjectsService>(opt => opt.BaseAddress = new Uri(api + "projects"));
            webapplicationBuilder.Services
                .AddHttpClient<IProjectResourcesService, ApiProjectResourcesService>(opt => opt.BaseAddress = new Uri(api + "projectresources"));
            webapplicationBuilder.Services
                .AddHttpClient<ITasksService, ApiTasksService>(opt => opt.BaseAddress = new Uri(api + "tasks"));
            webapplicationBuilder.Services
                .AddHttpClient<ITaskCommentsService, ApiTaskCommentsService>(opt => opt.BaseAddress = new Uri(api + "taskcomments"));
            webapplicationBuilder.Services
                .AddHttpClient<IUserRolesService, ApiUserRolesService>(opt => opt.BaseAddress = new Uri(api + "userroles"));
            webapplicationBuilder.Services
                .AddHttpClient<ITaskStatusesService, ApiTaskStatusesService>(opt => opt.BaseAddress = new Uri(api + "taskstatuses"));
            webapplicationBuilder.Services
                .AddHttpClient<IUserProfilesService, ApiUserProfilesService>(opt => opt.BaseAddress = new Uri(api + "userprofiles"));
            webapplicationBuilder.Services
                .AddHttpClient<IUsersService, ApiUsersService>(opt => opt.BaseAddress = new Uri(api + "users"));
            webapplicationBuilder.Services
                .AddHttpClient<IRolesService, ApiRolesService>(opt => opt.BaseAddress = new Uri(api + "roles"));

            webapplicationBuilder.Services.AddHttpClient();
        }

        public static void SetupAuth(this WebApplicationBuilder webapplicationBuilder)
        {
            webapplicationBuilder.Services.AddHttpContextAccessor();
            webapplicationBuilder.Services.AddScoped<ITokenAccessor, TokenAccessor>();
            webapplicationBuilder.Services.AddScoped<IJwtService, JwtService>();

            webapplicationBuilder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
