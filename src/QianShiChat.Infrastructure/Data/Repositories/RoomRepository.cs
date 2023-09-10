namespace QianShiChat.Infrastructure.Data.Repositories;

public class RoomRepository : IRoomRepository, IScoped
{
    private readonly IApplicationDbContext _context;
    public RoomRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Room> GetByUser(int userId)
    {
        return _context.Rooms.AsNoTracking()
            .Where(x => x.FromId == userId);
    }

    public async Task<bool> RoomExistsAsync(int fromId, string id, CancellationToken cancellationToken = default)
        => await _context.Rooms.Where(x => x.FromId == fromId && x.Id == id).AnyAsync(cancellationToken);

    public async Task<Room?> FindAsync(int fromId, string id, CancellationToken cancellationToken = default)
    {
        return await _context.Rooms.Where(x => x.FromId == fromId && x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Room room, CancellationToken cancellationToken = default) => await _context.Rooms.AddAsync(room, cancellationToken);
}
