namespace QianShiChat.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, UserInfo>()
            .ForMember(d => d.CreateTime, m => m.MapFrom(s => Timestamp.Now));

        CreateMap<UserInfo, UserDto>()
            .ReverseMap();

        CreateMap<UserInfo, UserWithMessage>()
             .ReverseMap();

        CreateMap<UserDto, UserWithMessage>()
            .ReverseMap();
    }
}
