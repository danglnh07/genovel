using AutoMapper;
using Carter;
using Genovel.Shared.Patterns;
using MediatR;

namespace Genovel.Features.Stories.GetStories;

public class GetStoriesParams(int? page, int? size, string[]? genres)
{
    public int? Page { get; set; } = page;
    public int? Size { get; set; } = size;
    public string[]? Genres { get; set; } = genres;
}

public class GetStoriesResponse(IEnumerable<GetStoriesItem> stories, Pagination metadata)
{
    public IEnumerable<GetStoriesItem> Stories { get; set; } = stories;
    public Pagination Metadata { get; set; } = metadata;
}

public class GetStoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/stories",
                async ([AsParameters] GetStoriesParams param, ISender sender, IMapper mapper, ILogger<GetStoriesEndpoint> logger) =>
        {
            // Create command from request
            var query = mapper.Map<GetStoriesQuery>(param);

            // Send command to mediator
            var result = await sender.Send(query);
            if (result.IsFailure)
            {
                if (Error.IsInvalidRequest(result.Error))
                {
                    return Results.BadRequest(result.Error);
                }

                logger.LogError("{ErrorCode}: {ErrorMessage}", result.Error.Code, result.Error.Message);
                return Results.StatusCode(500);
            }

            // Return response
            var resp = mapper.Map<GetStoriesResponse>(result.Value);
            return Results.Ok(resp);
        })
            .RequireCors("AllowSpecificOrigin")
            .WithName("GetStories")
            .WithDescription("Get paged list of stories with minimal information, ordered by their updated time, descending")
            .WithTags("Story")
            .Produces<GetStoriesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
