using AutoMapper;
using Carter;
using Genovel.Shared.Patterns;
using MediatR;

namespace Genovel.Features.Stories.UpdateStory;

public class UpdateStoryRequest
{
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public List<string>? Genres { get; set; } = null;
    public string? Language { get; set; } = null;
    public string? Image { get; set; } = null;
    public bool? IsOriginalCompleted { get; set; }
    public string? Source { get; set; } = null;
}

public class UpdateStoryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsOriginalCompleted { get; set; }
    public string Source { get; set; } = string.Empty;
}

public class UpdateStortEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "/api/stories/{id:guid}",
                async (Guid id, UpdateStoryRequest req, IMapper mapper, ISender sender, ILogger<UpdateStortEndpoint> logger) =>
        {
            // Create command from request
            var command = mapper.Map<UpdateStoryCommand>(req);
            command.Id = id;

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

            // Return response
            var resp = mapper.Map<UpdateStoryResponse>(result.Value);
            return Results.Ok(resp);
        })
            .WithName("UpdateStory")
            .WithDescription("Update a story metadata partially")
            .WithTags("Story")
            .Produces<UpdateStoryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
