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
            var data = await repository.GetAllAsync();
            return TypedResults.Ok(data);
        })
        .WithName("GetAllLogs")
        .WithOpenApi();

        /*group.MapGet("/{id}", (int id) =>
        {
            //return new Log { ID = id };
        })
        .WithName("GetLogById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Log input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateLog")
        .WithOpenApi();

        group.MapPost("/", (Log model) =>
        {
            //return TypedResults.Created($"/api/Logs/{model.ID}", model);
        })
        .WithName("CreateLog")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Log { ID = id });
        })
        .WithName("DeleteLog")
        .WithOpenApi();*/
    }
}
