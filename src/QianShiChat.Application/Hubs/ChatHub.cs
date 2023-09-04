using System.Threading.Channels;

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
    /// <param name="redisCachingProvider"></param>
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

    public async Task UploadStream(ChannelReader<string> stream, string id)
    {
        Console.WriteLine(id);
        while (await stream.WaitToReadAsync())
        {
            while (stream.TryRead(out var item))
            {
                Console.WriteLine(item);
            }
        }
        Console.WriteLine(id);
    }

    public Task<string> OnlineFileStreamConfirm(int toid, FileBaseInfo fileInfo)
    {
        return _onlineDataTransmission.CreateChannel(CurrentUserId, toid, fileInfo);
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
    }

    public ChannelReader<string>? OnlineReceiveFileStream(string id)
    {
        var reader = _onlineDataTransmission.GetChannelReader(id, CurrentUserId);
        return reader;
    }
}