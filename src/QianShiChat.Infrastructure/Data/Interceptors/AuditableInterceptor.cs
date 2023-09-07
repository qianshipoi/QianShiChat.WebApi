using QianShiChat.Domain.Extensions;

namespace QianShiChat.Infrastructure.Data.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                var now = Timestamp.Now;
                entry.Property(x => x.CreateTime).CurrentValue = now;
                entry.Property(x => x.UpdateTime).CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.UpdateTime).CurrentValue = Timestamp.Now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}