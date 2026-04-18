using AutoMapper;
using Carter;
using Genovel.Shared.Patterns;
using MediatR;

namespace Genovel.Features.Stories.GetStoryById;

public class GetStoryByIdResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsOriginalCompleted { get; set; }
    public string Source { get; set; } = string.Empty;
    public IEnumerable<ChapterItem> Chapters { get; set; } = [];
}

public class GetStoryByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/stories/{id}",
                async (Guid id, ISender sender, IMapper mapper, ILogger<GetStoryByIdEndpoint> logger) =>
        {
            // Create query from request
            var query = new GetStoryByIdQuery(id);

            // Send command to mediator
            var result = await sender.Send(query);
            if (result.IsFailure)
            {
                if (Error.IsNotFound(result.Error))
                {
                    return Results.NotFound();
                }

                logger.LogError("{ErrorCode}: {ErrorMessage}", result.Error.Code, result.Error.Message);
                return Results.StatusCode(500);
            }

            // Return response
            return Results.Ok(mapper.Map<GetStoryByIdResponse>(result.Value));
        })
            .WithName("GetStoryById")
            .WithDescription("Get a story by ID, including all story information and a minimal list of all chapters")
            .WithTags("Story")
            .Produces<GetStoryByIdResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
