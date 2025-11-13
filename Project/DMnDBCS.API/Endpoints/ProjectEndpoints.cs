using DMnDBCS.API.Repositories.Projects;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Reflection;
namespace DMnDBCS.API.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/projects").WithTags(nameof(Project));

        group.MapGet("/", async ([FromServices] IProjectRepository repository) =>
        {
            var data = await repository.GetAllAsync();
            return TypedResults.Ok(data);
        })
        .WithName("GetAllProjects")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] IProjectRepository repository) =>
        {
            var data = await repository.GetByIdAsync(id);
            return TypedResults.Ok(data);
        })
        .WithName("GetProjectById")
        .WithOpenApi();

        group.MapPut("/{id}", UpdateProject)
        .WithName("UpdateProject")
        .WithOpenApi();

        async Task<IResult> UpdateProject(int id, Project input, [FromServices] IProjectRepository repository)
        {
            var data = await repository.UpdateAsync(input);
            return data ? TypedResults.Ok() : TypedResults.BadRequest();
        }

        group.MapPost("/", CreateProject)
        .WithName("CreateProject")
        .WithOpenApi();

        async Task<IResult> CreateProject(Project model, [FromServices] IProjectRepository repository)
        {
            var data = await repository.CreateAsync(model);
            return data ? TypedResults.Created($"/api/projects/{model.Id}", model) : TypedResults.BadRequest();
        }

        group.MapDelete("/{id}", DeleteProject)
        .WithName("DeleteProject")
        .WithOpenApi();

        async Task<IResult> DeleteProject(int id, [FromServices] IProjectRepository repository)
        {
            var data = await repository.DeleteAsync(id);
            return data ? TypedResults.Ok() : TypedResults.BadRequest();
        }
    }
}
