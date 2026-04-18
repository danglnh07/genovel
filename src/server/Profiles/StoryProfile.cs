using AutoMapper;
using Genovel.Features.Stories.CreateStory;
using Genovel.Features.Stories.GetStoryById;
using Genovel.Features.Stories.UpdateStory;
using Genovel.Models;

namespace Genovel.Profiles;

public class StoryProfile : Profile
{
    public StoryProfile()
    {

        CreateMap<CreateStoryRequest, CreateStoryCommand>();
        CreateMap<CreateStoryCommand, Story>();
        CreateMap<CreateStoryResult, CreateStoryResponse>();

        CreateMap<UpdateStoryRequest, UpdateStoryCommand>();
        // Do not map ID, and only update field that is not null or empty
        CreateMap<UpdateStoryCommand, Story>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest => dest.Genres, opt => opt.Condition(src => src.Genres != null && src.Genres.Count > 0))
            .ForMember(dest => dest.Language, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Language)))
            .ForMember(dest => dest.Image, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Image)))
            .ForMember(dest => dest.Source, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Source)))
            .ForMember(dest => dest.IsOriginalCompleted, opt => opt.Condition(src => src.IsOriginalCompleted.HasValue));
        CreateMap<Story, UpdateStoryResult>();
        CreateMap<UpdateStoryResult, UpdateStoryResponse>();

        CreateMap<Story, GetStoryByIdResult>();
        CreateMap<GetStoryByIdResult, GetStoryByIdResponse>();

    }
}
