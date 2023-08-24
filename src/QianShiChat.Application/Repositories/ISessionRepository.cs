namespace QianShiChat.Application.Repositories;

public interface ISessionRepository
{
    Task AddAsync(Session session, CancellationToken cancellationToken = default);
    Task<Session?> FindAsync(int fromId, string id, CancellationToken cancellationToken = default);
    IQueryable<Session> GetByUser(int userId);
    Task<bool> SessionExistsAsync(int fromId, string id, CancellationToken cancellationToken = default);
}
