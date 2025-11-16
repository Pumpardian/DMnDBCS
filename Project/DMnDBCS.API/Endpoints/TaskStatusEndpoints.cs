using DMnDBCS.API.Repositories.TaskStatuses;

namespace DMnDBCS.API.Endpoints;

public static class TaskStatusEndpoints
{
    public static void MapTaskStatusEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/taskstatuses").WithTags(nameof(Domain.Entities.TaskStatus)).RequireAuthorization();

        group.MapGet("/", async (ITaskStatusRepository repository) =>
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
        .WithName("GetAllTaskStatuses")
        .WithOpenApi();
    }
}
