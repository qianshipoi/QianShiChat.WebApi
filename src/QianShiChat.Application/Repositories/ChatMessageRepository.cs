namespace QianShiChat.Application.Repositories;

public class ChatMessageRepository : IChatMessageRepository, IScoped
{
    private readonly IApplicationDbContext _context;

    public ChatMessageRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetUnreadCountAsync(string sessionId, long position, CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .Where(x => x.SessionId == sessionId)
            .Where(x => x.Id > position)
            .CountAsync(cancellationToken);
    }

    public async Task<ChatMessage?> GetLastMessageAsync(string sessionId,CancellationToken cancellationToken= default)
    {
        return await _context.ChatMessages.AsNoTracking()
            .Where(x => x.SessionId == sessionId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}