namespace QianShiChat.Application.Repositories;

public class UnitOfWork : IUnitOfWork, IScoped
{
    private readonly ChatDbContext _context;

    public UnitOfWork(ChatDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}