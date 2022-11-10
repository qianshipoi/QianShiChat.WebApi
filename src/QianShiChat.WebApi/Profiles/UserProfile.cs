using AutoMapper;

using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, UserInfo>()
                .ForMember(d=>d.CreateTime,m => m.MapFrom(s => DateTime.Now));
            CreateMap<UserInfo, UserDto>()
                .ReverseMap();
        }
    }
}
