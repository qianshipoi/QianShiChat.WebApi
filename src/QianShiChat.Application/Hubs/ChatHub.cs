using QianShiChat.Application.Services.IServices;

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
    private readonly IRoomService _roomService;
    private readonly IOnlineManager _onlineManager;
    private readonly OnlineDataTransmission _onlineDataTransmission;
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;

    /// <summary>
    /// chat hub.
    /// </summary>
    /// <param name="friendService"></param>
    /// <param name="roomService"></param>
    /// <param name="onlineManager"></param>
    /// <param name="onlineDataTransmission"></param>
    public ChatHub(
        IFriendService friendService,
        IRoomService roomService,
        IOnlineManager onlineManager,
        OnlineDataTransmission onlineDataTransmission,
        IUserService userService,
        IGroupService groupService)
    {
        _friendService = friendService;
        _roomService = roomService;
        _onlineManager = onlineManager;
        _onlineDataTransmission = onlineDataTransmission;
        _userService = userService;
        _groupService = groupService;
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

        var roomIds = await _roomService.GetRoomIdsAsync(CurrentUserId);

        var cacheKey = FormatOnlineUserKey(CurrentUserId.ToString(), CurrentClientType!);
        if (isOnline)
        {
            await _onlineManager.ConnectedAsync(CurrentUserId, Context.ConnectionId, CurrentClientType);
            foreach (var item in roomIds)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, item);
            }
        }
        else
        {
            await _onlineManager.DisconnectedAsync(CurrentUserId, Context.ConnectionId, CurrentClientType);
            foreach (var item in roomIds)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, item);
            }
        }
    }

    private static string FormatOnlineUserKey(string userId, string clientType) => $"{userId}_{clientType}";

    public IAsyncEnumerable<RoomDto> GetRoomsAsync(CancellationToken cancellationToken)
    {
        return _roomService.GetRoomsAsync(CurrentUserId, cancellationToken);
    }

    public async Task ReadPositionAsync(string roomId, long position)
    {
        await _roomService.UpdateRoomPositionAsync(CurrentUserId, roomId, position);
    }

    public async Task<RoomDto?> GetRoomAsync(int toId, ChatMessageSendType type)
    {
        var roomId = type switch
        {
            ChatMessageSendType.Personal => AppConsts.GetPrivateChatRoomId(CurrentUserId, toId),
            ChatMessageSendType.Group => AppConsts.GetGroupChatRoomId(CurrentUserId, toId),
            _ => throw new NotSupportedException()
        };

        var room = await _roomService.GetRoomAsync(CurrentUserId, roomId);
        if (room is null)
        {
            var user = await _userService.GetUserByIdAsync(CurrentUserId);

            IRoomToObject? toObject = type switch
            {
                ChatMessageSendType.Personal => await _userService.GetUserByIdAsync(toId),
                ChatMessageSendType.Group => await _groupService.FindByIdAsync(toId),
                _ => throw new NotSupportedException()
            };
            if (toObject is null)
            {
                return null;
            }

            room = RoomDto.CreateEmpty(roomId, user!, toObject);
        }
        return room;
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