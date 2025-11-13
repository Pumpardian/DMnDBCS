using DMnDBCS.API.Extensions;
using DMnDBCS.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.ConnectDatabase();
builder.RegisterServices();
builder.SetupAuth();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapUserEndpoints();
app.MapLogEndpoints();
app.MapProjectEndpoints();
app.MapTaskEndpoints();
app.MapAuthEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseCors("AllowWebUI");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();
app.Run();
