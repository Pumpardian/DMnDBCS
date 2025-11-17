using DMnDBCS.API.Repositories.Projects;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/projects").WithTags(nameof(Project)).RequireAuthorization();

        group.MapGet("/", async ([FromServices] IProjectRepository repository) =>
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
        .WithName("GetAllProjects")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] IProjectRepository repository) =>
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
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("GetProjectById")
        .WithOpenApi();

        group.MapGet("/latest", async ([FromServices] IProjectRepository repository) =>
        {
            try
            {
                var data = await repository.GetLatestAsync();
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
        .WithName("GetLatest")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, Project input, [FromServices] IProjectRepository repository) =>
        {
            try
            {
                var data = await repository.UpdateAsync(input);
                return data ? TypedResults.Ok() : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("UpdateProject")
        .WithOpenApi();

        group.MapPost("/", async (Project model, [FromServices] IProjectRepository repository) =>
        {
            try
            {
                var data = await repository.CreateAsync(model);
                return data ? TypedResults.Created($"/api/projects/{model.Id}", model) : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("CreateProject")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] IProjectRepository repository) =>
        {
            try
            {
                var data = await repository.DeleteAsync(id);
                return data ? TypedResults.Ok() : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("DeleteProject")
        .WithOpenApi();
    }
}
