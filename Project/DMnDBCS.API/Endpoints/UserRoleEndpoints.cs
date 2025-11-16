using DMnDBCS.API.Repositories.UserRoles;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Reflection;
namespace DMnDBCS.API.Endpoints;

public static class UserRoleEndpoints
{
    public static void MapUserRoleEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/userroles").WithTags(nameof(UserRole));

        group.MapGet("/project/{id}", async (int id, [FromServices] IUserRoleRepository repository) =>
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
        .WithName("GetAllUserRolesForProjectById")
        .WithOpenApi();

        group.MapGet("/{projectId}/{userId}", async (int userId, int projectId, [FromServices] IUserRoleRepository repository) =>
        {
            try
            {
                var data = await repository.GetByUserAndProjectIdsAsync(userId, projectId);
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
        .WithName("GetUserRoleById")
        .WithOpenApi();

        group.MapPut("/{projectId}/{userId}", async (int userId, int projectId, UserRole input, [FromServices] IUserRoleRepository repository) =>
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
        .WithName("UpdateUserRole")
        .WithOpenApi();

        group.MapPost("/", async (UserRole model, [FromServices] IUserRoleRepository repository) =>
        {
            try
            {
                var data = await repository.CreateAsync(model);
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
        .WithName("CreateUserRole")
        .WithOpenApi();

        group.MapDelete("/{projectId}/{userId}", async (int userId, int projectId, [FromServices] IUserRoleRepository repository) =>
        {
            try
            {
                var data = await repository.DeleteAsync(userId, projectId);
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
        .WithName("DeleteUserRole")
        .WithOpenApi();
    }
}
