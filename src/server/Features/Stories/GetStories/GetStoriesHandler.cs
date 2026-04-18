using Genovel.Models;
using Genovel.Shared.CQRS;
using Genovel.Shared.Patterns;
using Genovel.Shared.Utils;
using Marten;
using Marten.Pagination;

namespace Genovel.Features.Stories.GetStories;

public class GetStoriesQuery(int page = 1, int size = 10, string[]? genres = null) : IQuery<Result<GetStoriesResult>>
{
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;
    public string[]? Genres = genres;
}

public class GetStoriesItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

public class GetStoriesResult(IPagedList<GetStoriesItem> stories)
{
    public IPagedList<GetStoriesItem> Stories { get; set; } = stories;
}

public class GetStoriesHandler(IDocumentSession session) : IQueryHandler<GetStoriesQuery, Result<GetStoriesResult>>
{
    public async Task<Result<GetStoriesResult>> Handle(GetStoriesQuery query, CancellationToken cancellationToken)
    {
        // Get stories
        var queryable = session.Query<Story>().Where(story => !story.IsDeleted);
        if (query.Genres is not null && query.Genres.Length > 0)
        {
            var genres = Normalizer.NormalizeGenres([.. query.Genres]);
            queryable = queryable.Where(story => story.Genres.Any(genre => genres.Contains(genre)));
        }

        var stories = await queryable
            .OrderByDescending(story => story.UpdatedAt)
            .Select(story => new GetStoriesItem
            {
                Id = story.Id,
                Title = story.Title,
                Language = story.Language,
                Image = story.Image,
            })
            .ToPagedListAsync(query.Page, query.Size, cancellationToken);

        return Result<GetStoriesResult>.Success(new GetStoriesResult(stories));
    }
}
