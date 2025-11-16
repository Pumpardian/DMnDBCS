using DMnDBCS.API.Repositories.UserProfiles;
using DMnDBCS.API.Services;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace DMnDBCS.API.Endpoints;

public static class UserProfileEndpoints
{
    public static void MapUserProfileEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/userprofiles").WithTags(nameof(UserProfile)).DisableAntiforgery();

        group.MapGet("/{id}", async (int id, [FromServices] IUserProfileRepository repository) =>
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
        .WithName("GetUserProfileById")
        .WithOpenApi();

        group.MapPut("/{id}", async ([FromRoute] int id, [FromForm] string profile, [FromForm] IFormFile? file,
            [FromServices] IUserProfileRepository repository, [FromServices] IImageService imageService) =>
        {
            try
            {
                var updatedProfile = JsonSerializer.Deserialize<UserProfile>(profile, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                if (file != null)
                {
                    if (updatedProfile!.ProfilePicture != null)
                    {
                        await imageService.DeleteImage(updatedProfile!.ProfilePicture);
                    }
                    updatedProfile.ProfilePicture = await imageService.SaveImage(file);
                }
                else
                {
                    var oldData = await repository.GetByIdAsync(id);
                    updatedProfile!.ProfilePicture = oldData.ProfilePicture;
                }

                    var data = await repository.UpdateAsync(updatedProfile!);
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
        .WithName("UpdateUserProfile")
        .WithOpenApi();

        group.MapPost("/", async ([FromForm] string profile, [FromForm] IFormFile? file,
            [FromServices] IUserProfileRepository repository, [FromServices] IImageService imageService) =>
        {
            try
            {
                var updatedProfile = JsonSerializer.Deserialize<UserProfile>(profile, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                if (file != null)
                {
                    if (updatedProfile!.ProfilePicture != null)
                    {
                        await imageService.DeleteImage(updatedProfile!.ProfilePicture);
                    }
                    updatedProfile.ProfilePicture = await imageService.SaveImage(file);
                }

                var data = await repository.CreateAsync(updatedProfile!);
                return data ? TypedResults.Created($"/api/userprofiles/{updatedProfile!.UserId}", updatedProfile!) : TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.ToString(),
                    title: "Error occured",
                    statusCode: 500);
            }
        })
        .WithName("CreateUserProfile")
        .WithOpenApi();
    }
}
