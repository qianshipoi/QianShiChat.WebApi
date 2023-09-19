using Microsoft.EntityFrameworkCore.Storage;

namespace QianShiChat.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChatDbContext _context;

    public UnitOfWork(ChatDbContext context)
    {
        _context = context;
        _context.Database.BeginTransaction();
    }

    public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}

public class Transaction : ITransaction
{
    private readonly ChatDbContext _context;

    public Transaction(ChatDbContext context)
    {
        _context = context;
    }
    private IDbContextTransaction? _currentTransaction;

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;
        _currentTransaction = _context.Database.BeginTransaction();
        return Task.FromResult(_currentTransaction);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await _context.SaveChangesAsync();// 将当前所有的变更都保存到数据库
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                // 最终需要把当前事务进行释放，并且置为空
                // 这样就可以多次的开启事务和提交事务
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}