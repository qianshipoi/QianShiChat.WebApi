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
        CreateMap<UserRealtion, UserDto>()
             .ForMember(d => d.Id, m => m.MapFrom(s => s.Friend!.Id))
             .ForMember(d => d.Avatar, m => m.MapFrom(s => s.Friend!.Avatar))
             .ForMember(d => d.CreateTime, m => m.MapFrom(s => s.Friend!.CreateTime))
             .ForMember(d => d.NickName, m => m.MapFrom(s => s.Friend!.NickName))
             .ForMember(d => d.Account, m => m.MapFrom(s => s.Friend!.Account))
             .ForMember(d => d.Alias, m => m.MapFrom(s => s.Alias));
    }
}
