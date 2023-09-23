using Microsoft.EntityFrameworkCore.Storage;

namespace QianShiChat.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    bool HasActiveTransaction { get; }

    IDbContextTransaction? GetCurrentTransaction { get; }

    Task<IDbContextTransaction?> BeginTransactionAsync();

    Task CommitAsync(IDbContextTransaction transaction);

    void RollbackTransaction();
}
