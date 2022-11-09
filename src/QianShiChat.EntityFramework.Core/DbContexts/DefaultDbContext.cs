using Furion.DatabaseAccessor;

using Microsoft.EntityFrameworkCore;

namespace QianShiChat.EntityFramework.Core.DbContexts
{
    [AppDbContext("DefaultDbContext", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}
