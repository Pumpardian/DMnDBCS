using DMnDBCS.API.Extensions;
using DMnDBCS.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.ConnectDatabase();
builder.RegisterServices();
builder.SetupAuth();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapUserEndpoints();
app.MapLogEndpoints();
app.MapProjectEndpoints();
app.MapProjectResourceEndpoints();
app.MapTaskEndpoints();
app.MapTaskCommentEndpoints();
app.MapUserRoleEndpoints();
app.MapUserProfileEndpoints();
app.MapTaskStatusEndpoints();
app.MapRoleEndpoints();
app.MapNotificationEndpoints();

app.MapAuthEndpoints();

app.UseCors("AllowWebUI");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
