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

    public async Task<List<SessionDto>> GetUserSessionAsync(int userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByUser(userId).ToListAsync(cancellationToken);
        var sessionDtos = new List<SessionDto>();
        await Parallel.ForEachAsync(sessions, async (session, ctx) => {
            var unreadCount = await _chatMessageRepository.GetUnreadCountAsync(session.Id, session.MessagePosition, ctx);
            if (unreadCount > 0)
            {
                sessionDtos.Add(new SessionDto
                {
                    Id = session.Id,
                    CreateTime = session.CreateTime,
                    FromId = session.FromId,
                    ToId = session.ToId,
                    Type = session.Type,
                    UnreadCount = unreadCount
                });
            }
        });

        return sessionDtos;
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
