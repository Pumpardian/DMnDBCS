using DMnDBCS.API.Repositories.Tasks;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using NuGet.Protocol.Core.Types;
namespace DMnDBCS.API.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tasks").WithTags(nameof(Domain.Entities.Task));

        group.MapGet("/executor/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            var data = await repository.GetAllForExecutorAsync(id);
            return TypedResults.Ok(data);
        })
        .WithName("GetAllTasksByExecutor")
        .WithOpenApi();

        group.MapGet("/project/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            var data = await repository.GetAllForProjectAsync(id);
            return TypedResults.Ok(data);
        })
        .WithName("GetAllTasksByProject")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            var data = await repository.GetByIdAsync(id);
            return TypedResults.Ok(data);
        })
        .WithName("GetTaskById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Domain.Entities.Task input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateTask")
        .WithOpenApi();

        group.MapPost("/", (Domain.Entities.Task model) =>
        {
            //return TypedResults.Created($"/api/Tasks/{model.ID}", model);
        })
        .WithName("CreateTask")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Task { ID = id });
        })
        .WithName("DeleteTask")
        .WithOpenApi();
    }
}
