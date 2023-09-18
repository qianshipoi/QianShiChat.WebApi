namespace QianShiChat.Application.Profiles;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDto>();

        CreateMap<GroupApply, GroupApplyDto>();
    }
}