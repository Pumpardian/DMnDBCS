using DMnDBCS.API.Repositories.TaskComments;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
namespace DMnDBCS.API.Endpoints;

public static class TaskCommentEndpoints
{
    public static void MapTaskCommentEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/taskcomments").WithTags(nameof(TaskComment)).RequireAuthorization();

        group.MapGet("/task/{id}", async (int id, [FromServices] ITaskCommentRepository repository) =>
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
        .WithName("GetTaskCommentsForTaskById")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] ITaskCommentRepository repository) =>
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
        .WithName("GetTaskCommentById")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, TaskComment input, [FromServices] ITaskCommentRepository repository) =>
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
        .WithName("UpdateTaskComment")
        .WithOpenApi();

        group.MapPost("/", async (TaskComment model, [FromServices] ITaskCommentRepository repository) =>
        {
            try
            {
                var data = await repository.CreateAsync(model);
                return data ? TypedResults.Created($"/api/taskcomments/{model.Id}", model) : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("CreateTaskComment")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] ITaskCommentRepository repository) =>
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
        .WithName("DeleteTaskComment")
        .WithOpenApi();
    }
}
