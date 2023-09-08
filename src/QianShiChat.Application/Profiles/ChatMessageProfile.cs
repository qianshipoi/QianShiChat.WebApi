namespace QianShiChat.Application.Profiles;

public class ChatMessageProfile : Profile
{
    public ChatMessageProfile()
    {
        CreateMap<ChatMessage, ChatMessageDto>();
    }
}
