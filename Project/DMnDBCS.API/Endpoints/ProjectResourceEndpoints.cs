using DMnDBCS.API.Repositories.ProjectResources;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class ProjectResourceEndpoints
{
    public static void MapProjectResourceEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/projectresources").WithTags(nameof(ProjectResource)).RequireAuthorization();

        group.MapGet("/{id}", async (int id, [FromServices] IProjectResourceRepository repository) =>
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
        .WithName("GetProjectResourceById")
        .WithOpenApi();

        group.MapGet("/project/{id}", async (int id, [FromServices] IProjectResourceRepository repository) =>
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
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("GetProjectResourceByProjectId")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, ProjectResource input, [FromServices] IProjectResourceRepository repository) =>
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
        .WithName("UpdateProjectResource")
        .WithOpenApi();

        group.MapPost("/", async (ProjectResource model, [FromServices] IProjectResourceRepository repository) =>
        {
            try
            {
                var data = await repository.CreateAsync(model);
                return data ? TypedResults.Created($"/api/projectresources/{model.Id}", model) : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("CreateProjectResource")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] IProjectResourceRepository repository) =>
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
        .WithName("DeleteProjectResource")
        .WithOpenApi();
    }
}
