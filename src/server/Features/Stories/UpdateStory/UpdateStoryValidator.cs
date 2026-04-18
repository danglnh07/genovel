using FluentValidation;

namespace Genovel.Features.Stories.UpdateStory;

public class UpdateStoryValidator : AbstractValidator<UpdateStoryCommand>
{
    public UpdateStoryValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);

        // Since this is a PATCH style request, a null value is actually valid,
        // but an empty value (empty string, empty list,...) is not
        RuleFor(x => x.Title)
            .Must(title => title is null || !string.IsNullOrEmpty(title))
            .WithMessage("Story title must not be empty");
        RuleFor(x => x.Description)
            .Must(description => description is null || !string.IsNullOrEmpty(description))
            .WithMessage("Story description must not be empty");
        RuleFor(x => x.Genres)
            .Must(genres => genres is null || !genres.Any())
            .WithMessage("Story genres must not be empty");
        RuleFor(x => x.Language)
            .Must(language => language is null || !string.IsNullOrEmpty(language))
            .WithMessage("Story language must not be empty");
        RuleFor(x => x.Image)
            .Must(image => image is null || !string.IsNullOrEmpty(image))
            .WithMessage("Story image must not be empty");
        RuleFor(x => x.Source)
            .Must(source => source is null || !string.IsNullOrEmpty(source))
            .WithMessage("Story source must not be empty");
    }

}
