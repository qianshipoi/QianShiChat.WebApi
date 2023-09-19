using Microsoft.EntityFrameworkCore.Storage;

namespace QianShiChat.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}

public interface ITransaction
{
    // 获取当前事务
    IDbContextTransaction GetCurrentTransaction();

    // 判断当前事务是否开启
    bool HasActiveTransaction { get; }

    // 开启事务
    Task<IDbContextTransaction> BeginTransactionAsync();

    // 提交事务
    Task CommitTransactionAsync(IDbContextTransaction transaction);

    // 事务回滚
    void RollbackTransaction();
}