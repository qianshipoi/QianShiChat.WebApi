namespace QianShiChat.Infrastructure.Data.Repositories;

internal class UserRepository : IUserRepository, IScoped
{
    private readonly ChatDbContext _context;

    public UserRepository(ChatDbContext context)
    {
        _context = context;
    }

    public Task<UserInfo?> FindByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return _context.UserInfos
           .Where(x => x.Id == userId && !x.IsDeleted)
           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetNicknameAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos
            .Where(x => x.Id == userId)
            .Select(x => x.NickName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsFriendAsync(int id1, int id2, CancellationToken cancellationToken = default)
    {
        return await _context.UserRealtions.AnyAsync(x => x.UserId == id1 && x.FriendId == id2, cancellationToken);
    }

    public async Task<List<UserInfo>> FindByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public Task<int> SetAliasAsync(int userId, int friendId, string? name, CancellationToken cancellationToken = default)
    {
        return _context.UserRealtions
           .Where(x => x.UserId == userId && x.FriendId == friendId)
           .ExecuteUpdateAsync(x => x.SetProperty(a => a.Alias, b => name), cancellationToken);
    }
}
