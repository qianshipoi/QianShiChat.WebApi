namespace QianShiChat.Application.Profiles;

public class AvatarProfile : Profile
{
    public AvatarProfile()
    {
        CreateMap<DefaultAvatar, AvatarDto>();
        CreateMap<UserAvatar, AvatarDto>();
    }
}