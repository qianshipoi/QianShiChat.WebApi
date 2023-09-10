namespace QianShiChat.Infrastructure.Data.Repositories;

public class ChatMessageRepository : IChatMessageRepository, IScoped
{
    private readonly IApplicationDbContext _context;

    public ChatMessageRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetUnreadCountAsync(string roomId, long position, CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .Where(x => x.RoomId == roomId)
            .Where(x => x.Id > position)
            .CountAsync(cancellationToken);
    }

    public async Task<ChatMessage?> GetLastMessageAsync(string roomId, CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages.AsNoTracking()
            .Where(x => x.RoomId == roomId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}