using DMnDBCS.API.Repositories.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Reflection;

namespace DMnDBCS.API.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tasks").WithTags(nameof(Domain.Entities.Task)).RequireAuthorization();

        group.MapGet("/executor/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllForExecutorAsync(id);
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("GetAllTasksByExecutor")
        .WithOpenApi();

        group.MapGet("/project/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllForProjectAsync(id);
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("GetAllTasksByProject")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            try
            { 
                var data = await repository.GetByIdAsync(id);
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("GetTaskById")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, Domain.Entities.Task input, [FromServices] ITaskRepository repository) =>
        {
            try
            {
                var data = await repository.UpdateAsync(input);
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("UpdateTask")
        .WithOpenApi();

        group.MapPost("/", async (Domain.Entities.Task model, [FromServices] ITaskRepository repository) =>
        {
            try
            {
                var data = await repository.CreateAsync(model);
                return data ? TypedResults.Created($"/api/tasks/{model.Id}", model) : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("CreateTask")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] ITaskRepository repository) =>
        {
            try
            {
                var data = await repository.DeleteAsync(id);
                return TypedResults.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("DeleteTask")
        .WithOpenApi();
    }
}
