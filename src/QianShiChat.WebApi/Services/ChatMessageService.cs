using AutoMapper;

using EasyCaching.Core;

using Microsoft.EntityFrameworkCore;

using QianShiChat.Common.Extensions;
using QianShiChat.Models;
using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Models.Entity;

using System.Text.Json;

namespace QianShiChat.WebApi.Services
{
    public class ChatMessageService : IChatMessageService, ITransient
    {
        private readonly ILogger<ChatMessageService> _logger;
        private readonly ChatDbContext _context;
        private readonly IRedisCachingProvider _redisCachingProvider;
        private readonly IMapper _mapper;

        public ChatMessageService(
            IRedisCachingProvider redisCachingProvider, 
            ChatDbContext context, 
            ILogger<ChatMessageService> logger, 
            IMapper mapper)
        {
            _redisCachingProvider = redisCachingProvider;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(
            int userId1, 
            int userId2, 
            CancellationToken cancellationToken = default)
        {
            var cacheKey = AppConsts.GetPrivateChatCacheKey(userId1, userId2);

            var cacheVal = await _redisCachingProvider.HGetAllAsync(cacheKey);
            if (cacheVal is null) cacheVal = new Dictionary<string, string>();

            var messages = new List<ChatMessage>();
            foreach (var item in cacheVal)
            {
                var message = JsonSerializer.Deserialize<ChatMessage>(item.Value);
                if (message is null)
                {
                    await _redisCachingProvider.HDelAsync(cacheKey, new List<string> { item.Key });
                    continue;
                }
                messages.Add(message);
            }

            if (messages.Count < 10)
            {
                var minId = messages.Min(x => x.Id);

                var data = await _context.ChatMessages.AsNoTracking()
                     .Where(x => x.SendType == ChatMessageSendType.Personal)
                     .Where(x => (x.ToId == userId1 && x.FromId == userId2) || (x.ToId == userId2 && x.FromId == userId1))
                     .Where(x => x.Id < minId)
                     .OrderByDescending(x => x.Id)
                     .Take(10 - messages.Count)
                     .ToListAsync(cancellationToken);
                if (data.Count > 0)
                {
                    messages.AddRange(data);

                    foreach (var item in data)
                    {
                        await _redisCachingProvider.HSetAsync(cacheKey, item.Id.ToString(), JsonSerializer.Serialize(item));
                    }
                }
            }

            return _mapper.Map<List<ChatMessageDto>>(messages);
        }

        public async Task UpdateMessageCursor(
            int userId,
            UpdateCursorRequest request,
            CancellationToken cancellationToken = default)
        {
            var cursorInfo = await _context.MessageCursors
                .FindAsync(new object[] { userId, request.ToId, request.Type }, cancellationToken);

            if (cursorInfo == null)
            {
                cursorInfo = _mapper.Map<MessageCursor>(request);
                cursorInfo.UserId = userId;
            }
            else
            {
                _mapper.Map(request, cursorInfo);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
