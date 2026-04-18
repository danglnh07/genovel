using Carter;
using Genovel.Shared.Patterns;
using MediatR;

namespace Genovel.Features.Stories.DeleteStory;

public class DeleteStortEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/stories/{id}",
                async (Guid id, ISender sender, ILogger<DeleteStortEndpoint> logger) =>
        {
            // Create command from request
            var command = new DeleteStoryCommand(id);

            // Send command to mediator
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                if (Error.IsNotFound(result.Error))
                {
                    return Results.BadRequest(result.Error);
                }

                logger.LogError("{ErrorCode}: {ErrorMessage}", result.Error.Code, result.Error.Message);
                return Results.StatusCode(500);
            }

            return Results.NoContent();
        })
            .WithName("DeleteStory")
            .WithDescription("Soft delete a story")
            .WithTags("Story")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
