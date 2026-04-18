using AutoMapper;
using Genovel.Models;
using Genovel.Shared.CQRS;
using Genovel.Shared.Patterns;
using Marten;

namespace Genovel.Features.Stories.GetStoryById;

public class GetStoryByIdQuery(Guid id) : IQuery<Result<GetStoryByIdResult>>
{
    public Guid Id { get; set; } = id;
}

public class ChapterItem
{
    public Guid Id { get; set; }
    public string ChapterNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}

public class GetStoryByIdResult
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

public class GetStoryByIdHandler(IDocumentSession session, IMapper mapper)
    : IQueryHandler<GetStoryByIdQuery, Result<GetStoryByIdResult>>
{
    public async Task<Result<GetStoryByIdResult>> Handle(GetStoryByIdQuery query, CancellationToken cancellationToken)
    {
        // Get the story by ID
        var story = await session.LoadAsync<Story>(query.Id, cancellationToken);
        if (story is null)
        {
            return Result<GetStoryByIdResult>.Failure(Error.NotFound(nameof(Story)));
        }

        // Get all chapters belong to this story
        var chapters = await session.Query<Chapter>()
            .Where(chapter => chapter.StoryId == story.Id && !chapter.IsDeleted)
            .Select(chapter => new ChapterItem
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                Title = chapter.Title
            })
            .ToListAsync(cancellationToken);

        // Create response
        var resp = mapper.Map<GetStoryByIdResult>(story);
        resp.Chapters = chapters;

        return Result<GetStoryByIdResult>.Success(resp);
    }
}
