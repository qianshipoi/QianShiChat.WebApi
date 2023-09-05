namespace QianShiChat.Application.Hubs;

/// <summary>
/// chat hub.
/// </summary>
[Authorize]
[ServiceFilter(typeof(ClientTypeAuthorize))]
public class ChatHub : Hub<IChatClient>
{
    private int CurrentUserId => int.Parse(Context.UserIdentifier!);
    private string CurrentClientType => Context.User!.FindFirstValue(CustomClaim.ClientType)!;

    private readonly IFriendService _friendService;
    private readonly ISessionService _sessionService;
    private readonly IOnlineManager _onlineManager;
    private readonly OnlineDataTransmission _onlineDataTransmission;

    /// <summary>
    /// chat hub.
    /// </summary>
    /// <param name="friendService"></param>
    /// <param name="sessionService"></param>
    /// <param name="onlineManager"></param>
    /// <param name="onlineDataTransmission"></param>
    public ChatHub(
        IFriendService friendService,
        ISessionService sessionService,
        IOnlineManager onlineManager,
        OnlineDataTransmission onlineDataTransmission)
    {
        _friendService = friendService;
        _sessionService = sessionService;
        _onlineManager = onlineManager;
        _onlineDataTransmission = onlineDataTransmission;
    }

    /// <summary>
    /// connected.
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await UserOnlineOffline();
    }

    /// <summary>
    /// disconnected.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        await UserOnlineOffline(false);
    }

    /// <summary>
    /// user online offline notification
    /// </summary>
    /// <param name="isOnline">is online</param>
    /// <returns></returns>
    private async Task UserOnlineOffline(bool isOnline = true)
    {
        var ids = await _friendService.GetFriendIdsAsync(CurrentUserId);
        await Clients.Users(ids.Select(x => x.ToString()))
            .Notification(
            new NotificationMessage(
                isOnline ? NotificationType.FriendOnline : NotificationType.FriendOffline,
                CurrentUserId));

        var cacheKey = FormatOnlineUserKey(CurrentUserId.ToString(), CurrentClientType!);
        if (isOnline)
        {
            await _onlineManager.ConnectedAsync(CurrentUserId, Context.ConnectionId, CurrentClientType);
        }
        else
        {
            await _onlineManager.DisconnectedAsync(CurrentUserId, Context.ConnectionId, CurrentClientType);
        }
    }

    private static string FormatOnlineUserKey(string userId, string clientType) => $"{userId}_{clientType}";

    public IAsyncEnumerable<SessionDto> GetSessionsAsync(CancellationToken cancellationToken)
    {
        return _sessionService.GetRoomsAsync(CurrentUserId, cancellationToken);
    }

    public async Task ReadPositionAsync(string sessionId, long position)
    {
        await _sessionService.UpdateSessionPositionAsync(CurrentUserId, sessionId, position);
    }

    public async IAsyncEnumerable<int> Counter(int count, int delay, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (int i = 0; i < count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return i;

            await Task.Delay(delay);
        }
    }

    public Task<SessionDto?> GetRoomAsync(int toId, ChatMessageSendType type)
    {
        return _sessionService.GetRoomAsync(CurrentUserId, type switch
        {
            ChatMessageSendType.Personal => AppConsts.GetPrivateChatSessionId(CurrentUserId, toId),
            ChatMessageSendType.Group => AppConsts.GetGroupChatSessionId(CurrentUserId, toId),
            _ => throw new NotSupportedException()
        });
    }

    public bool UserIsOnline(int id) => _onlineManager.CheckOnline(id);

    public async Task<string?> OnlineFileStreamConfirm(int toid, FileBaseInfo fileInfo)
    {
        var connections = _onlineManager.GetConnections(toid);
        if (!connections.Any())
        {
            return null;
        }

        var channel = await _onlineDataTransmission.CreateChannel(CurrentUserId, toid, fileInfo);
        var confirm = await Clients.Client(connections.First())
            .ConfirmOnlineFile(channel);
        if (!confirm)
        {
            await _onlineDataTransmission.Cancel(channel.Id, CurrentUserId);
            return null;
        }
        channel.PassedClient(CurrentClientType);
        await Clients.Client(connections.First())
            .OnlineFileReceive(channel);
        return channel.Id;
    }

    public async Task<bool> RejectOnlineFile(string id)
    {
        return await _onlineDataTransmission.Cancel(id, CurrentUserId);
    }

    public async Task OnlineUploadFileStream(ChannelReader<string> stream, string id)
    {
        var writer = _onlineDataTransmission.GetChannelWriter(id, CurrentUserId);
        if (writer is null) return;
        while (await stream.WaitToReadAsync())
        {
            while (stream.TryRead(out var item))
            {
                await writer.WriteAsync(item);
            }
        }

        await stream.Completion.ContinueWith((_) => {
            writer.Complete();
        });
    }

    public ChannelReader<string> OnlineReceiveFileStream(string id)
    {
        return _onlineDataTransmission.GetChannelReader(id, CurrentUserId)!;
    }
}