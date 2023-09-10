namespace QianShiChat.Application.Common.IRepositories;

public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken cancellationToken = default);
    Task<Room?> FindAsync(int fromId, string id, CancellationToken cancellationToken = default);
    IQueryable<Room> GetByUser(int userId);
    Task<bool> RoomExistsAsync(int fromId, string id, CancellationToken cancellationToken = default);
}
