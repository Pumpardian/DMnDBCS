using DMnDBCS.UI.Services.Logs;
using DMnDBCS.UI.Services.Projects;

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

            webapplicationBuilder.Services.AddHttpClient();

            webapplicationBuilder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            webapplicationBuilder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
