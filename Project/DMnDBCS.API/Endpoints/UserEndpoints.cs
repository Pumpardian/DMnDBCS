using DMnDBCS.API.Repositories.Users;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DMnDBCS.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags(nameof(User)).RequireAuthorization();

        group.MapGet("/", async ([FromServices] IUserRepository repository) =>
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
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/not-in-project/{projectId}", async (int projectId, [FromServices] IUserRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllNotInProjectAsync(projectId);
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
        .WithName("GetAllUsersNotInProject")
        .WithOpenApi();

        group.MapGet("/in-project/{projectId}", async (int projectId, [FromServices] IUserRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllInProjectAsync(projectId);
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
        .WithName("GetAllUsersInProject")
        .WithOpenApi();

        group.MapGet("/{identifier}", async (string identifier, [FromServices] IUserRepository repository) =>
        {
            try
            {
                if (int.TryParse(identifier, out int id))
                {
                    var data = await repository.GetByIdAsync(id);
                    return TypedResults.Ok(data);
                }
                else
                {
                    var data = await repository.GetByEmailAsync(identifier);
                    return TypedResults.Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("GetUserByIdentifier")
        .WithOpenApi()
        .AllowAnonymous();

        group.MapPut("/{id}", async (int id, User input, [FromServices] IUserRepository repository) =>
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
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] IUserRepository repository) =>
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
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}
