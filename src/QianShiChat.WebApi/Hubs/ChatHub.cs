using QianShiChat.Application.Contracts;

namespace QianShiChat.WebApi.Hubs;

/// <summary>
/// chat hub.
/// </summary>
public class ChatHub : Hub<IChatClient>
{
    private int CurrentUserId => int.Parse(Context.UserIdentifier!);

    private readonly IFriendService _friendService;
    private readonly IRedisCachingProvider _redisCachingProvider;

    /// <summary>
    /// chat hub.
    /// </summary>
    /// <param name="friendService"></param>
    /// <param name="redisCachingProvider"></param>
    public ChatHub(
        IFriendService friendService,
        IRedisCachingProvider redisCachingProvider)
    {
        _friendService = friendService;
        _redisCachingProvider = redisCachingProvider;
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
                CurrentUserId.ToString()));

        if (isOnline)
        {
            await _redisCachingProvider.HSetAsync(AppConsts.OnlineCacheKey, CurrentUserId.ToString(), "1");
        }
        else
        {
            await _redisCachingProvider.HDelAsync(AppConsts.OnlineCacheKey, new string[] { CurrentUserId.ToString() });
        }
    }
}