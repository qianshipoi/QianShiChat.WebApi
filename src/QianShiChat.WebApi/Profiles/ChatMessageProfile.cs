using AutoMapper;

using QianShiChat.Models;
using QianShiChat.WebApi.Models.Entity;

namespace QianShiChat.WebApi.Profiles
{
    public class ChatMessageProfile : Profile
    {
        public ChatMessageProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>();
        }
    }
}
