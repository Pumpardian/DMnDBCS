using DMnDBCS.API.Repositories.Logs;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class LogEndpoints
{
    public static void MapLogEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/logs").WithTags(nameof(Log));

        group.MapGet("/", async ([FromServices] ILogRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllAsync();
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("GetAllLogs")
        .WithOpenApi().RequireAuthorization();
    }
}
