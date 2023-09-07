namespace QianShiChat.Application.Repositories;

public class UnitOfWork : IUnitOfWork, IScoped
{
    private readonly IApplicationDbContext _context;

    public UnitOfWork(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}