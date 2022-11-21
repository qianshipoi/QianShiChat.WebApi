﻿using AutoMapper;

using QianShiChat.Common.Extensions;
using QianShiChat.Models;
using QianShiChat.WebApi.Models.Entity;

namespace QianShiChat.WebApi.Profiles
{
    public class ChatMessageProfile : Profile
    {
        public ChatMessageProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>();

            CreateMap<UpdateCursorRequest, MessageCursor>()
                .ForMember(d => d.LastUpdateTime, m => m.MapFrom(s => Timestamp.Now));
        }
    }
}
