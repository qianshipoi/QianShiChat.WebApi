using EasyCaching.Core;

using Microsoft.AspNetCore.SignalR;

using QianShiChat.Models;
using QianShiChat.WebApi.Services;

namespace QianShiChat.WebApi.Hubs;

public class ChatHub : Hub<IChatClient>
{
    public const string OnlineCacheKey = "OnlineList";
    private int CurrentUserId => int.Parse(Context.UserIdentifier!);

    private readonly IFirendService _firendService;
    private readonly IRedisCachingProvider _redisCachingProvider;

    public ChatHub(IFirendService firendService, IRedisCachingProvider redisCachingProvider)
    {
        _firendService = firendService;
        _redisCachingProvider = redisCachingProvider;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await UserOnlineOffline();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        await UserOnlineOffline(false);
    }

    /// <summary>
    /// user online offline notification
    /// </summary>
    /// <param name="isOnline"></param>
    /// <returns></returns>
    private async Task UserOnlineOffline(bool isOnline = true)
    {
        var ids = await _firendService.GetFirendIdsAsync(CurrentUserId);
        await Clients.Users(ids.Select(x => x.ToString())).Notification(new NotificationMessage
        {
            Type = isOnline ? NotificationType.FirendOnline : NotificationType.FirendOffline,
            Message = CurrentUserId.ToString()
        });

        if (isOnline)
        {
            await _redisCachingProvider.HSetAsync(OnlineCacheKey, CurrentUserId.ToString(), "1");
        }
        else
        {
            await _redisCachingProvider.HDelAsync(OnlineCacheKey, new string[] { CurrentUserId.ToString() });
        }
    }

    public async Task SendMessage(string user, string message)
      => await Clients.All.ReceiveMessage(user, message);
}