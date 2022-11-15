using AutoMapper;

using QianShiChat.Common.Extensions;
using QianShiChat.Models;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, UserInfo>()
                .ForMember(d => d.CreateTime, m => m.MapFrom(s => Timestamp.Now));

            CreateMap<UserInfo, UserDto>()
                .ReverseMap();
        }
    }
}
