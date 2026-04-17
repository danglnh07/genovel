using AutoMapper;
using Genovel.Features.Stories.CreateStory;
using Genovel.Models;

namespace Genovel.Profiles;

public class StoryProfile : Profile
{
    public StoryProfile()
    {

        CreateMap<CreateStoryRequest, CreateStoryCommand>();
        CreateMap<CreateStoryCommand, Story>();
        CreateMap<CreateStoryResult, CreateStoryResponse>();
    }
}
