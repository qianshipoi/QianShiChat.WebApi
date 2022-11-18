using AutoMapper;

using EasyCaching.Core;

using Microsoft.AspNetCore.SignalR;

using QianShiChat.Common.Extensions;
using QianShiChat.Models;
using QianShiChat.WebApi.Models.Entity;
using QianShiChat.WebApi.Services;

using System.Collections.Generic;
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
    private readonly IMapper _mapper;

    public ChatHub(IFriendService friendService, IRedisCachingProvider redisCachingProvider, IMapper mapper)
    {
        _friendService = friendService;
        _redisCachingProvider = redisCachingProvider;
        _mapper = mapper;
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

    public async Task<ChatMessageDto> PrivateChatSend(PrivateChatMessageRequest request)
    {
        var now = Timestamp.Now;

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = request.Message,
            CreateTime = now,
            FromId = CurrentUserId,
            ToId = request.UserId,
            LastUpdateTime = now,
            MessageType = ChatMessageType.Text,
            SendType = ChatMessageSendType.Personal
        };
        var chatMessageDto = _mapper.Map<ChatMessageDto>(chatMessage);

        // send message.
        await Clients.User(request.UserId.ToString()).PrivateChat(chatMessageDto);

        // cache message max 30 times;
        var cacheKey = AppConsts.GetPrivateChatCacheKey(request.UserId, CurrentUserId);

        var cacheValue = await _redisCachingProvider.HGetAllAsync(cacheKey);
        if (cacheValue == null) cacheValue = new Dictionary<string, string>();

        cacheValue.Add(chatMessageDto.Id.ToString(), JsonSerializer.Serialize(chatMessageDto));

        if (cacheValue.Count > 30)
        {
            cacheValue.Remove(cacheValue.Keys.Min());
        }

        await _redisCachingProvider.HMSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        // cache save message queue.
        await _redisCachingProvider.HSetAsync(
            AppConsts.ChatMessageCacheKey,
            chatMessage.Id.ToString(),
            JsonSerializer.Serialize(chatMessage));

        // cache update message cursor
        await _redisCachingProvider.HSetAsync(
            AppConsts.MessageCursorCacheKey,
            AppConsts.GetMessageCursorCacheKey(CurrentUserId, request.UserId, ChatMessageSendType.Personal),
            chatMessage.Id.ToString());

        return chatMessageDto;
    }

    public async Task UpdateMesssageCursor(UpdateCursorRequest request)
    {
        await _redisCachingProvider.HSetAsync(
            AppConsts.MessageCursorCacheKey,
            AppConsts.GetMessageCursorCacheKey(CurrentUserId, request.ToId, request.Type),
            request.Position.ToString());
    }
}