namespace QianShiChat.WebApi.Profiles;

public class ChatMessageProfile : Profile
{
    public ChatMessageProfile()
    {
        CreateMap<ChatMessage, ChatMessageDto>();

        CreateMap<UpdateCursorRequest, MessageCursor>()
            .ForMember(d => d.LastUpdateTime, m => m.MapFrom(s => Timestamp.Now));
    }

    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>();
        }
    }
}