using Microsoft.EntityFrameworkCore;

namespace QianShiChat.WebApi.Models
{
    public class ChatDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<UserRealtion> UserRealtions { get; set; }

        public DbSet<FriendApply> FriendApplies { get; set; }

        public ChatDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}
