using QianShiChat.Application.Contracts;

namespace QianShiChat.WebApi.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserInfo>()
            .ForMember(d => d.CreateTime, m => m.MapFrom(s => Timestamp.Now));

        CreateMap<UserInfo, UserDto>()
            .ReverseMap();

        CreateMap<UserInfo, UserWithMessage>()
             .ReverseMap();

        CreateMap<UserDto, UserWithMessage>()
            .ReverseMap();
    }
}

public class AvatarProfile : Profile
{
    public AvatarProfile()
    {
        CreateMap<DefaultAvatar, AvatarDto>();
        CreateMap<UserAvatar, AvatarDto>();
    }
}