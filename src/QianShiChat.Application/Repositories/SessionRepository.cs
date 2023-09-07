namespace QianShiChat.Application.Repositories;

public class SessionRepository : ISessionRepository, IScoped
{
    private readonly IApplicationDbContext _context;
    public SessionRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Session> GetByUser(int userId)
    {
        return _context.Sessions.AsNoTracking()
            .Where(x => x.FromId == userId);
    }

    public async Task<bool> SessionExistsAsync(int fromId, string id, CancellationToken cancellationToken = default)
        => await _context.Sessions.Where(x => x.FromId == fromId && x.Id == id).AnyAsync(cancellationToken);

    public async Task<Session?> FindAsync(int fromId, string id, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions.Where(x => x.FromId == fromId && x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Session session, CancellationToken cancellationToken = default) => await _context.Sessions.AddAsync(session, cancellationToken);
}
