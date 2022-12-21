using QianShiChat.Application.Contracts;

namespace QianShiChat.WebApi.Profiles;

public class FriendApplyProfile : Profile
{
    public FriendApplyProfile()
    {
        CreateMap<FriendApply, FriendApplyDto>()
            .ReverseMap();

        CreateMap<CreateFriendApplyDto, FriendApply>()
            .ForMember(d => d.CreateTime, m => m.MapFrom(s => Timestamp.Now))
            .ForMember(d => d.UpdateTime, m => m.MapFrom((s, d) => d.CreateTime))
            .ForMember(d => d.FriendId, m => m.MapFrom(s => s.UserId))
            .ForMember(d => d.Status, m => m.MapFrom(s => ApplyStatus.Applied));
    }
}