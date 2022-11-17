using EasyCaching.Core;

using Microsoft.AspNetCore.SignalR;

using QianShiChat.Common.Extensions;
using QianShiChat.Models;
using QianShiChat.WebApi.Services;

using System.Text.Json;
using System.Text.Json.Serialization;

using Yitter.IdGenerator;

namespace QianShiChat.WebApi.Hubs;

public class ChatHub : Hub<IChatClient>
{
    public const string OnlineCacheKey = "OnlineList";
    private int CurrentUserId => int.Parse(Context.UserIdentifier!);

    private readonly IFriendService _friendService;
    private readonly IRedisCachingProvider _redisCachingProvider;

    public ChatHub(IFriendService friendService, IRedisCachingProvider redisCachingProvider)
    {
        _friendService = friendService;
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
        var ids = await _friendService.GetFriendIdsAsync(CurrentUserId);
        await Clients.Users(ids.Select(x => x.ToString())).Notification(new NotificationMessage(default, default)
        {
            Type = isOnline ? NotificationType.FriendOnline : NotificationType.FriendOffline,
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

    public async Task<PrivateChatMessage> PrivateChatSend(PrivateChatMessageRequest request)
    {
        var message = new PrivateChatMessage(
            YitIdHelper.NextId(),
            request.UserId,
            request.Message);

        await Clients.User(request.UserId.ToString()).PrivateChat(message);
        var cacheKey = GetPrivateCacheKey(request.UserId);
        await _redisCachingProvider.HSetAsync(cacheKey, Timestamp.Now.ToString(), JsonSerializer.Serialize(message));

        return message;
    }

    private string GetPrivateCacheKey(int userId)
    {
        return CurrentUserId > userId ? $"{userId}-{CurrentUserId}" : $"{CurrentUserId}-{userId}";
    }

}