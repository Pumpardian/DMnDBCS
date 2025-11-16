using DMnDBCS.API.Repositories.Notifications;
using DMnDBCS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace DMnDBCS.API.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/notifications").WithTags(nameof(Notification)).RequireAuthorization();

        group.MapGet("/user/{userId}", async (int userId, [FromServices] INotificationRepository repository) =>
        {
            try
            {
                var data = await repository.GetAllForUserAsync(userId);
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
        .WithName("GetAllNotificationsForUser")
        .WithOpenApi();

        group.MapGet("/{id}", async (int id, [FromServices] INotificationRepository repository) =>
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
        .WithName("GetNotificationById")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, Notification input, [FromServices] INotificationRepository repository) =>
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
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("UpdateNotification")
        .WithOpenApi();

        group.MapPost("/", async (Notification model, [FromServices] INotificationRepository repository) =>
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
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("CreateNotification")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, [FromServices] INotificationRepository repository) =>
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
                    title: "Error occurred",
                    statusCode: 500);
            }
        })
        .WithName("DeleteNotification")
        .WithOpenApi();
    }
}
