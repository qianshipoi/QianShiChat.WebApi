using AutoMapper;

using QianShiChat.Common.Models;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Profiles
{
    public class FriendApplyProfile : Profile
    {
        public FriendApplyProfile()
        {
            CreateMap<FriendApply, FriendApplyDto>()
                .ReverseMap();
        }
    }
}
