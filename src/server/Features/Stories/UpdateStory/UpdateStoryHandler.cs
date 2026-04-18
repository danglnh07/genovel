using AutoMapper;
using Genovel.Models;
using Genovel.Shared.CQRS;
using Genovel.Shared.Patterns;
using Marten;

namespace Genovel.Features.Stories.UpdateStory;

public class UpdateStoryCommand : ICommand<Result<UpdateStoryResult>>
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public List<string>? Genres { get; set; } = null;
    public string? Language { get; set; } = null;
    public string? Image { get; set; } = null;
    public bool? IsOriginalCompleted { get; set; }
    public string? Source { get; set; } = null;
}

public class UpdateStoryResult
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

public class UpdateStoryHandler(IDocumentSession session, IMapper mapper)
    : ICommandHandler<UpdateStoryCommand, Result<UpdateStoryResult>>
{
    public async Task<Result<UpdateStoryResult>> Handle(UpdateStoryCommand command, CancellationToken cancellationToken)
    {
        // Get entity from database by ID
        var story = await session.Query<Story>()
            .Where(x => x.Id == command.Id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (story is null)
        {
            return Result<UpdateStoryResult>.Failure(Error.NotFound(nameof(Story)));
        }

        // Update new value to story
        story = mapper.Map(command, story);
        story.UpdatedAt = DateTimeOffset.UtcNow;

        // Update into database
        session.Update(story);
        await session.SaveChangesAsync(cancellationToken);

        return Result<UpdateStoryResult>.Success(mapper.Map<UpdateStoryResult>(story));
    }
}
