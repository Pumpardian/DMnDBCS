using DMnDBCS.UI.Services.Auth;
using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.Logs;
using DMnDBCS.UI.Services.ProjectResources;
using DMnDBCS.UI.Services.Projects;
using DMnDBCS.UI.Services.TaskComments;
using DMnDBCS.UI.Services.Tasks;
using DMnDBCS.UI.Services.TaskStatuses;
using DMnDBCS.UI.Services.UserRoles;

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
