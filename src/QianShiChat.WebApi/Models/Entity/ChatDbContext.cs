namespace QianShiChat.WebApi.Models
{
    public class ChatDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<UserRealtion> UserRealtions { get; set; }

        public DbSet<FriendApply> FriendApplies { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<MessageCursor> MessageCursors { get; set; }

        public ChatDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

            var nowTime = 1668583992424L;
            var password = "E10ADC3949BA59ABBE56E057F20F883E";

            var userInfos = new List<UserInfo>()
            {
                new UserInfo() { Id = 1, Account = "admin", CreateTime = nowTime, Password = password, NickName = "Admin" },
                new UserInfo() { Id = 2, Account = "qianshi", CreateTime = nowTime, Password = password, NickName = "千矢" },
                new UserInfo() { Id = 3, Account = "kuriyama", CreateTime = nowTime, Password = password, NickName = "栗山未来" },
                new UserInfo() { Id = 4, Account = "admin1", CreateTime = nowTime, Password = password, NickName = "Admin1" },
                new UserInfo() { Id = 5, Account = "qianshi1", CreateTime = nowTime, Password = password, NickName = "千矢1" },
                new UserInfo() { Id = 6, Account = "kuriyama1", CreateTime = nowTime, Password = password, NickName = "栗山未来1" },
                new UserInfo() { Id = 7, Account = "admin2", CreateTime = nowTime, Password = password, NickName = "Admin2" },
                new UserInfo() { Id = 8, Account = "qianshi2", CreateTime = nowTime, Password = password, NickName = "千矢2" },
                new UserInfo() { Id = 9, Account = "kuriyama2", CreateTime = nowTime, Password = password, NickName = "栗山未来2" },
            };

            modelBuilder.Entity<UserInfo>().HasData(userInfos);

            var friendApplys = new List<FriendApply>()
            {
                new(){ Id = 1, CreateTime = nowTime, UserId = 1, FriendId = 2, LaseUpdateTime = nowTime, Remark = "很高兴认识你。", Status = QianShiChat.Models.ApplyStatus.Rejected },
                new(){ Id = 2, CreateTime = nowTime, UserId = 1, FriendId = 2, LaseUpdateTime = nowTime, Remark = "Nice to meet you.", Status = QianShiChat.Models.ApplyStatus.Passed },
                new() {Id = 3, CreateTime = nowTime, UserId = 1, FriendId = 3, LaseUpdateTime = nowTime, Remark = "hi!", Status = QianShiChat.Models.ApplyStatus.Applied }
            };

            modelBuilder.Entity<FriendApply>().HasData(friendApplys);

            var urs = new List<UserRealtion>()
            {
                new () { Id = 1, CreateTime = nowTime, UserId = 1, FriendId = 2 },
                new () { Id = 2, CreateTime = nowTime, UserId = 2, FriendId = 1 },
                new () { Id = 3, CreateTime = nowTime, UserId = 2, FriendId = 3 },
                new () { Id = 4, CreateTime = nowTime, UserId = 3, FriendId = 2 },
            };

            modelBuilder.Entity<UserRealtion>().HasData(urs);
        }
    }
}