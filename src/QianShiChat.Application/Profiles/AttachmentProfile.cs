namespace QianShiChat.Application.Profiles;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentDto>();
    }
}
