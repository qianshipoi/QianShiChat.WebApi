using QianShiChat.Application.Common.IRepositories;

namespace QianShiChat.Application.Services;

public class SessionService : ISessionService, ITransient
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SessionService(
        ISessionRepository sessionRepository,
        IChatMessageRepository chatMessageRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _chatMessageRepository = chatMessageRepository;
        _unitOfWork = unitOfWork;
    }

    private object? FormatMessageContnt(string content, ChatMessageType type)
    {
        if (type == ChatMessageType.Text)
        {
            return content;
        }

        var classType = type switch
        {
            ChatMessageType.Image => typeof(Attachment),
            ChatMessageType.Video => typeof(Attachment),
            ChatMessageType.OtherFile => typeof(Attachment),
            _ => throw new NotSupportedException()
        };

        return JsonSerializer.Deserialize(content, classType);
    }

    public async Task<List<SessionDto>> GetUserSessionAsync(int userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByUser(userId).ToListAsync(cancellationToken);
        var sessionDtos = new List<SessionDto>();

        foreach (var session in sessions)
        {
            var unreadCount = await _chatMessageRepository.GetUnreadCountAsync(session.Id, session.MessagePosition, cancellationToken);
            if (unreadCount > 0)
            {
                var messageDto = new SessionDto
                {
                    Id = session.Id,
                    FromId = session.FromId,
                    ToId = session.ToId,
                    Type = session.Type,
                    UnreadCount = unreadCount
                };

                // get last message content and time;
                var lastMessage = await _chatMessageRepository.GetLastMessageAsync(session.Id, cancellationToken);
                if (lastMessage is not null)
                {
                    messageDto.LastMessageTime = lastMessage.UpdateTime;
                    messageDto.LastMessageContent = FormatMessageContnt(lastMessage.Content, lastMessage.MessageType);
                }

                sessionDtos.Add(messageDto);
            }
        }

        return sessionDtos;
    }

    public async IAsyncEnumerable<SessionDto> GetRoomsAsync(int userId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByUser(userId).ToListAsync(cancellationToken);

        foreach (var session in sessions)
        {
            var unreadCount = await _chatMessageRepository.GetUnreadCountAsync(session.Id, session.MessagePosition, cancellationToken);
            if (unreadCount > 0)
            {
                var messageDto = new SessionDto
                {
                    Id = session.Id,
                    FromId = session.FromId,
                    ToId = session.ToId,
                    Type = session.Type,
                    UnreadCount = unreadCount
                };

                // get last message content and time;
                var lastMessage = await _chatMessageRepository.GetLastMessageAsync(session.Id, cancellationToken);
                if (lastMessage is not null)
                {
                    messageDto.LastMessageTime = lastMessage.UpdateTime;
                    messageDto.LastMessageContent = FormatMessageContnt(lastMessage.Content, lastMessage.MessageType);
                }
                yield return messageDto;
            }
        }
    }

    public async Task<SessionDto?> GetRoomAsync(int userId, string roomId, CancellationToken cancellationToken = default)
    {
        var room = await _sessionRepository.FindAsync(userId, roomId, cancellationToken);
        if (room is null)
        {
            return null;
        }

        var message = await _chatMessageRepository.GetLastMessageAsync(roomId, cancellationToken);

        var roomDto = new SessionDto
        {
            Id = room.Id,
            FromId = room.FromId,
            ToId = room.ToId,
            Type = room.Type,
            UnreadCount = 0
        };

        if (message is not null)
        {
            roomDto.LastMessageTime = message.UpdateTime;
            roomDto.LastMessageContent = FormatMessageContnt(message.Content, message.MessageType);
        }

        return roomDto;
    }

    public async Task UpdateSessionPositionAsync(int userId, string sessionId, long position, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(userId, sessionId, cancellationToken);

        if (session is null) return;

        session.MessagePosition = position;

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

    public async Task AddOrUpdatePositionAsync(
        int fromId,
        int toId,
        ChatMessageSendType type,
        long position = 0,
        CancellationToken cancellationToken = default)
    {
        var sessionId = string.Empty;
        if (type == ChatMessageSendType.Personal)
        {
            sessionId = AppConsts.GetPrivateChatSessionId(fromId, toId);
        }
        else if (type == ChatMessageSendType.Group)
        {
            sessionId = AppConsts.GetGroupChatSessionId(fromId, toId);
        }

        var session = await _sessionRepository.FindAsync(fromId, sessionId, cancellationToken);
        if (session is null)
        {
            session = new Session
            {
                Id = sessionId,
                FromId = fromId,
                ToId = toId,
                MessagePosition = position,
            };
            await _sessionRepository.AddAsync(session, cancellationToken);
        }
        else
        {
            session.MessagePosition = position;
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
