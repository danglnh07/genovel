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
            // Validate param
            if (param.Page < 1)
            {
                logger.LogWarning("invalid value for page, proceed to default value (1): {page}", param.Page);
                param.Page = 1;
            }

            if (param.Size < 1)
            {
                logger.LogWarning("invalid value for size, proceed to default value (10): {size}", param.Size);
                param.Size = 10;
            }
            else if (param.Size > 100)
            {
                logger.LogWarning("invalid value for size, proceed to default value (10): {size}", param.Size);
                param.Size = 10;
            }

            if (param.Genres is not null && param.Genres.Length == 0)
            {
                param.Genres = null;
            }

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
            .WithName("GetStories")
            .WithDescription("Get paged list of stories with minimal information, ordered by their updated time, descending")
            .WithTags("Story")
            .Produces<GetStoriesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
