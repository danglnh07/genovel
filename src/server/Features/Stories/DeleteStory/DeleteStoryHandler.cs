using Genovel.Models;
using Genovel.Shared.CQRS;
using Genovel.Shared.Patterns;
using Marten;

namespace Genovel.Features.Stories.DeleteStory;

public class DeleteStoryCommand(Guid id) : ICommand<Result>
{
    public Guid Id { get; set; } = id;
}

public class DeleteStoryHandler(IDocumentSession session) : ICommandHandler<DeleteStoryCommand, Result>
{
    public async Task<Result> Handle(DeleteStoryCommand command, CancellationToken cancellationToken)
    {
        // Get entity from database by ID
        var story = await session.Query<Story>()
            .Where(x => x.Id == command.Id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (story is null)
        {
            return Result.Failure(Error.NotFound(nameof(Story)));
        }

        // Cascade delete all associating chapters
        var chapters = await session.Query<Chapter>()
            .Where(chapter => chapter.StoryId == story.Id && !chapter.IsDeleted)
            .ToListAsync(cancellationToken);
        foreach (var chapter in chapters)
        {
            chapter.IsDeleted = true;
            chapter.UpdatedAt = DateTimeOffset.UtcNow;
        }
        session.Update(chapters.ToArray()); // Marten only support batch update if the it's an array

        // Soft delete entity
        story.IsDeleted = true;
        story.UpdatedAt = DateTimeOffset.UtcNow;
        session.Update(story);
        await session.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
