using AutoMapper;
using Carter;
using MediatR;

namespace Genovel.Features.Stories.CreateStory;

public class CreateStoryRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsOriginalCompleted { get; set; }
    public string Source { get; set; } = string.Empty;
};

public class CreateStoryResponse
{
    public Guid Id { get; set; }
};

public class CreateStortEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/stories",
                async (CreateStoryRequest req, IMapper mapper, ISender sender, ILogger<CreateStortEndpoint> logger) =>
        {
            // Create command from request
            var command = mapper.Map<CreateStoryCommand>(req);

            // Send command to mediator
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                logger.LogError("{ErrorCode}: {ErrorMessage}", result.Error.Code, result.Error.Message);
                return Results.StatusCode(500);
            }

            // Return response
            var resp = mapper.Map<CreateStoryResponse>(result.Value);
            return Results.Created($"/api/stories/{resp.Id}", resp);
        })
            .WithName("CreateStory")
            .WithDescription("Create a story metadata")
            .WithTags("Story")
            .Produces<CreateStoryResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

    }
}
