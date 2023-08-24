namespace QianShiChat.Application.Repositories;

public class ChatMessageRepository : IChatMessageRepository, IScoped
{
    private readonly ChatDbContext _context;

    public ChatMessageRepository(ChatDbContext context)
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
}