namespace QianShiChat.Application.Services;

public class ChatService : IChatService, ISingleton
{
    private readonly ILogger<ChatService> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOnlineManager _onlineManager;

    public ChatService(
        ILogger<ChatService> logger,
        IMapper mapper,
        IServiceScopeFactory scopeFactory,
        IOnlineManager onlineManager)
    {
        _logger = logger;
        _mapper = mapper;
        _scopeFactory = scopeFactory;
        _onlineManager = onlineManager;
    }

    public async Task SendNewGroupNotifyByUsersAsync(int groupId, IEnumerable<int> userIds)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var groupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();
        var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();

        var group = await GetGroupAsync(fileService, groupRepository, groupId);

        await hubContext
            .Clients
            .Users(userIds.Select(x => x.ToString()))
            .Notification(new NotificationMessage(NotificationType.NewGroup, group));
    }

    private async Task<GroupDto> GetGroupAsync(IFileService fileService, IGroupRepository groupRepository, int groupId)
    {
        var group = await groupRepository.FindByIdAsync(groupId);
        var dto = _mapper.Map<GroupDto>(group);
        if (string.IsNullOrWhiteSpace(dto.Avatar))
        {
            dto.Avatar = fileService.GetDefaultGroupAvatar();
        }
        else
        {
            dto.Avatar = fileService.FormatPublicFile(dto.Avatar);
        }
        return dto;
    }

    public async Task SendNewGroupNotifyAsync(int groupId)
    {
        using var scope = _scopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
        var groupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();
        var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
        var group = await GetGroupAsync(fileService, groupRepository, groupId);

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
}
