using AutoMapper;
using Genovel.Shared.CQRS;
using Genovel.Shared.Patterns;
using Marten;

namespace Genovel.Features.Stories.CreateStory;

public class CreateStoryCommand : ICommand<Result<CreateStoryResult>>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsOriginalCompleted { get; set; }
    public string Source { get; set; } = string.Empty;
};

public record CreateStoryResult(Guid Id);

public class CreateStoryHandler(IDocumentSession session, IMapper mapper) : ICommandHandler<CreateStoryCommand, Result<CreateStoryResult>>
{
    public async Task<Result<CreateStoryResult>> Handle(CreateStoryCommand command, CancellationToken cancellationToken)
    {
        // Map command to entity
        var story = mapper.Map<Models.Story>(command);

        // Store into database
        session.Store(story);
        await session.SaveChangesAsync(cancellationToken);

        // Return result
        return Result<CreateStoryResult>.Success(new CreateStoryResult(story.Id));
    }
}
