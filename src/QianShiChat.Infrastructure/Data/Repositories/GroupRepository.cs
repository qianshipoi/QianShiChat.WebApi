namespace QianShiChat.Infrastructure.Data.Repositories;

internal class GroupRepository : IGroupRepository, IScoped
{
    private readonly ChatDbContext _context;

    public GroupRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task TotalUserDecrementAsync(int groupId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE {nameof(Group)} SET TotalUser=TotalUser-1 WHERE Id = ${groupId}");
    }

    public async Task TotalUserIncrementAsync(int groupId, CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE {nameof(Group)} SET TotalUser=TotalUser+1 WHERE Id = ${groupId}");
    }

    public Task<Group?> FindByIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return _context.Groups
            .AsNoTracking()
            .Where(x => x.Id == groupId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> UpdateGroupNameAsync(int groupId, string name, CancellationToken cancellationToken = default)
    {
        return _context.Groups
            .Where(x => x.Id == groupId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.Name, b => name), cancellationToken);
    }

    public Task<int> UpdateGroupAvatarAsync(int groupId, string avatarPath, CancellationToken cancellationToken = default)
    {
        return _context.Groups
            .Where(x => x.Id == groupId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.Avatar, b => avatarPath), cancellationToken);
    }

    public Task<bool> IsCreatorAsync(int userId, int groupId, CancellationToken cancellationToken = default)
    {
        return _context.Groups
            .AnyAsync(x => x.Id == groupId && x.UserId == userId, cancellationToken);
    }

    public Task<bool> IsJoinGroupAsync(int userId, int groupId, CancellationToken cancellationToken = default)
    {
        return _context.UserGroupRealtions
            .AnyAsync(x => x.GroupId == groupId && x.UserId == userId, cancellationToken);
    }

    public Task<int> SetAliasAsync(int userId, int groupId, string? name, CancellationToken cancellationToken = default)
    {
        return _context.UserGroupRealtions
            .Where(x => x.UserId == userId && x.GroupId == groupId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.Alias, b => name), cancellationToken);
    }
}
