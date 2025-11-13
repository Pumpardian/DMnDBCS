using DMnDBCS.API.Repositories.Users;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags(nameof(User));

        group.MapGet("/", async ([FromServices] IUserRepository repository) =>
        {
            var data = await repository.GetAllAsync();
            return TypedResults.Ok(data);
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] IUserRepository repository) =>
        {
            var data = await repository.GetByIdAsync(id);
            return TypedResults.Ok(data);
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, User input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            return TypedResults.Ok(id);
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}
