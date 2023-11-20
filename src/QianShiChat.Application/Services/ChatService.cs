using Microsoft.AspNetCore.SignalR;

namespace QianShiChat.Application.Services;

public class ChatService : IChatService, ISingleton
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IFileService _fileService;

    public ChatService(
        IMapper mapper,
        IServiceScopeFactory scopeFactory,
        IFileService fileService)
    {
        _mapper = mapper;
        _scopeFactory = scopeFactory;
        _fileService = fileService;
    }

    public async Task SendNewGroupNotifyByUsersAsync(int groupId, IEnumerable<int> userIds)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var groupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();

        var group = await GetGroupAsync(groupRepository, groupId);

        await hubContext
            .Clients
            .Users(userIds.Select(x => x.ToString()))
            .Notification(new NotificationMessage(NotificationType.NewGroup, group));
    }

    private async Task<GroupDto> GetGroupAsync(IGroupRepository groupRepository, int groupId)
    {
        var group = await groupRepository.FindByIdAsync(groupId);
        var dto = _mapper.Map<GroupDto>(group);
        _fileService.FormatAvatar(dto);
        return dto;
    }

    public async Task SendNewGroupNotifyAsync(int groupId)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var groupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();
        var group = await GetGroupAsync(groupRepository, groupId);

        await hubContext
            .Clients
            .Group(AppConsts.GetGroupChatRoomId(0, groupId))
            .Notification(new NotificationMessage(NotificationType.NewGroup, group));
    }

    public async Task SendGroupApplyNotifyAsync(int applyId)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var entity = await context.GroupApplies
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Group)
            .Where(x => x.Id == applyId)
            .SingleAsync();

        var apply = _mapper.Map<GroupApplyDto>(entity);

        await hubContext.Clients.User(apply.User!.Id.ToString()).Notification(new NotificationMessage(NotificationType.GroupApply, apply));
    }

    public async Task SendNewFriendNotifyAsync(IEnumerable<int> ids)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var users = (await userRepository
            .FindByIdsAsync(ids))
            .Select(x=> {
                var dto = _mapper.Map<UserDto>(x);
                _fileService.FormatAvatar(dto);
                return dto;
            });

        var tasks = users.Select(x => hubContext.Clients.User(x.Id.ToString()).Notification(new NotificationMessage(NotificationType.NewFriend, x)));

        await Task.WhenAll(tasks);
    }
}
