using AutoMapper;

using QianShiChat.Common.Models;
using QianShiChat.Models;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Profiles
{
    public class FriendApplyProfile : Profile
    {
        public FriendApplyProfile()
        {
            CreateMap<FriendApply, FriendApplyDto>()
                .ReverseMap();
            CreateMap<CreateFriendApplyDto, FriendApply>()
                .ForMember(d => d.CreateTime, m => m.MapFrom(s => DateTime.Now))
                .ForMember(d => d.LaseUpdateTime, m => m.MapFrom(s => DateTime.Now))
                .ForMember(d => d.FriendId, m => m.MapFrom(s => s.UserId))
                .ForMember(d => d.Status, m => m.MapFrom(s =>  ApplyStatus.Applied));
        }
    }
}
