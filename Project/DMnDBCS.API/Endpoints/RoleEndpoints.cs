using DMnDBCS.API.Repositories.Roles;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/roles").WithTags(nameof(Role)).RequireAuthorization();

        group.MapGet("/", async ([FromServices] IRoleRepository repository) =>
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
        .WithName("GetAllRoles")
        .WithOpenApi();

/*        group.MapGet("/{id}", (int id) =>
        {
            //return new Role { ID = id };
        })
        .WithName("GetRoleById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Role input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateRole")
        .WithOpenApi();

        group.MapPost("/", (Role model) =>
        {
            //return TypedResults.Created($"/api/Roles/{model.ID}", model);
        })
        .WithName("CreateRole")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Role { ID = id });
        })
        .WithName("DeleteRole")
        .WithOpenApi();
*/    }
}
