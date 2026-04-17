using FluentValidation;

namespace Genovel.Features.Stories.CreateStory;

public class CreateStoryValidator : AbstractValidator<CreateStoryCommand>
{
    public CreateStoryValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Story title must not be empty");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Story description must not be empty");
        RuleFor(x => x.Genres).NotEmpty().WithMessage("Story genres must not be empty");
        RuleFor(x => x.Language).NotEmpty().WithMessage("Story language must not be empty");
        RuleFor(x => x.Image).NotEmpty().WithMessage("Story image must not be empty");
        RuleFor(x => x.Source).NotEmpty().WithMessage("Story source must not be empty");
    }
}
