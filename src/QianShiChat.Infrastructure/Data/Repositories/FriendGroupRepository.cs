namespace QianShiChat.Infrastructure.Data.Repositories;

internal class FriendGroupRepository : IFriendGroupRepository, IScoped
{
    private readonly ChatDbContext _context;

    public FriendGroupRepository(ChatDbContext context)
    {
        _context = context;
    }

    public Task<FriendGroup?> FindUserDefaultGroupByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return _context.FriendGroups.Where(x => x.UserId == userId && x.IsDeleted && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
    }
}
