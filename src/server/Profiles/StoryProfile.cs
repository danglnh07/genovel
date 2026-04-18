using AutoMapper;
using Genovel.Features.Stories.CreateStory;
using Genovel.Features.Stories.GetStories;
using Genovel.Features.Stories.GetStoryById;
using Genovel.Features.Stories.UpdateStory;
using Genovel.Models;
using Genovel.Shared.Patterns;
using Genovel.Shared.Utils;

namespace Genovel.Profiles;

public class StoryProfile : Profile
{
    public StoryProfile()
    {

        CreateMap<CreateStoryRequest, CreateStoryCommand>();
        CreateMap<CreateStoryCommand, Story>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => Normalizer.NormalizeGenres(src.Genres)));
        CreateMap<CreateStoryResult, CreateStoryResponse>();

        CreateMap<UpdateStoryRequest, UpdateStoryCommand>();
        // Do not map ID, and only update field that is not null or empty
        CreateMap<UpdateStoryCommand, Story>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest => dest.Genres, opt =>
            {
                opt.Condition(src => src.Genres != null && src.Genres.Count > 0);
                opt.MapFrom(src => Normalizer.NormalizeGenres(src.Genres!));
            })
            .ForMember(dest => dest.Language, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Language)))
            .ForMember(dest => dest.Image, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Image)))
            .ForMember(dest => dest.Source, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Source)))
            .ForMember(dest => dest.IsOriginalCompleted, opt => opt.Condition(src => src.IsOriginalCompleted.HasValue));
        CreateMap<Story, UpdateStoryResult>();
        CreateMap<UpdateStoryResult, UpdateStoryResponse>();

        CreateMap<Story, GetStoryByIdResult>();
        CreateMap<GetStoryByIdResult, GetStoryByIdResponse>();

        CreateMap<GetStoriesParams, GetStoriesQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page == null || src.Page < 1 ? 1 : src.Page.Value))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size == null || src.Size < 1 ? 10 : src.Size.Value));
        CreateMap<GetStoriesResult, GetStoriesResponse>();
        CreateMap<GetStoriesResult, GetStoriesResponse>()
            .ConstructUsing((src, ctx) => new GetStoriesResponse(
                        src.Stories,
                        new Pagination(
                            src.Stories.HasNextPage,
                            src.Stories.HasPreviousPage,
                            src.Stories.TotalItemCount,
                            src.Stories.PageSize,
                            src.Stories.PageNumber)));


    }
}
