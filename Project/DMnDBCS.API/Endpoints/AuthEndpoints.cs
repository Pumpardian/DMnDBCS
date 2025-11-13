using DMnDBCS.API.Repositories.Users;
using DMnDBCS.API.Services;
using DMnDBCS.Domain.Entities;
using DMnDBCS.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
namespace DMnDBCS.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth").WithTags(nameof(Project));

        group.MapPost("/register", async (LoginRequest request, [FromServices] IUserRepository repository) =>
        {
            try
            {
                var isCreated = await repository.CreateAsync(new User() { Email = request.Email, Name = request.Name }, request.Password);

                if (isCreated)
                {
                    return Results.Ok(new { message = "User created successfully" });
                }

                return Results.BadRequest(new { message = "Failed to create user" });
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return Results.BadRequest(new { message = "Email already exists" });
            }
        });

        group.MapPost("/login", async (LoginRequest request, [FromServices] IUserRepository repository, [FromServices] ITokenService tokenService) =>
        {
            var user = await repository.AuthenticateAsync(request.Email, request.Password);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            var token = tokenService.GenerateJwtToken(user);

            return Results.Ok(new AuthResponse(
                Token: token,
                UserId: user.Id,
                UserName: user.Name,
                Email: user.Email
            ));
        });
    }
}
