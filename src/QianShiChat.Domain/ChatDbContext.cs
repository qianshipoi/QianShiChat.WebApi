namespace QianShiChat.Domain;

public class ChatDbContext : DbContext
{
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<UserAvatar> UserAvatars { get; set; }
    public DbSet<DefaultAvatar> DefaultAvatars { get; set; }
    public DbSet<UserRealtion> UserRealtions { get; set; }
    public DbSet<FriendApply> FriendApplies { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<MessageCursor> MessageCursors { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroupRealtion> UserGroupRealtions { get; set; }
    public DbSet<GroupApply> GroupApplies { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    public ChatDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        var nowTime = 1668583992424L;
        var password = "E10ADC3949BA59ABBE56E057F20F883E";

        var userInfos = new List<UserInfo>()
        {
            new UserInfo() {
                Id = 1,
                Account = "admin",
                Avatar = "/Raw/DefaultAvatar/1.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "Admin",
                Description = "炒鸡管理员！"
            },
            new UserInfo() {
                Id = 2,
                Account = "qianshi",
                Avatar="/Raw/DefaultAvatar/2.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "千矢",
                Description = "道歉的时候露出肚皮才是常识！"
            },
            new UserInfo() {
                Id = 3,
                Account = "kuriyama",
                Avatar="/Raw/DefaultAvatar/3.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "栗山未来",
                Description = "不愉快です"
            },
            new UserInfo() {
                Id = 4,
                Account = "admin1",
                Avatar="/Raw/DefaultAvatar/4.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "Admin1"
            },
            new UserInfo() {
                Id = 5,
                Account = "qianshi1",
                Avatar="/Raw/DefaultAvatar/5.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "千矢1"
            },
            new UserInfo() {
                Id = 6,
                Account = "kuriyama1",
                Avatar="/Raw/DefaultAvatar/6.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "栗山未来1"
            },
            new UserInfo() {
                Id = 7,
                Account = "admin2",
                Avatar="/Raw/DefaultAvatar/7.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "Admin2"
            },
            new UserInfo() {
                Id = 8,
                Account = "qianshi2",
                Avatar="/Raw/DefaultAvatar/8.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "千矢2"
            },
            new UserInfo() {
                Id = 9,
                Account = "kuriyama2",
                Avatar="/Raw/DefaultAvatar/9.jpg",
                CreateTime = nowTime,
                Password = password,
                NickName = "栗山未来2"
            },
        };

        modelBuilder.Entity<UserInfo>().HasData(userInfos);

        var userAvatars = new List<UserAvatar>()
        {
            new UserAvatar(){ Id = 1, UserId = 1, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/1.jpg", Size = 204*1024 },
            new UserAvatar(){ Id = 2, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/2.jpg", Size = 30*1024 },
            new UserAvatar(){ Id = 3, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/3.jpg", Size = (ulong)Math.Ceiling(85.7*1024) },
            new UserAvatar(){ Id = 4, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/4.jpg", Size = 156*1024 },
            new UserAvatar(){ Id = 5, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/5.jpg", Size = (ulong)Math.Ceiling(71.5*1024) },
            new UserAvatar(){ Id = 6, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/6.jpg", Size = 106*1024 },
            new UserAvatar(){ Id = 7, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/7.jpg", Size = (ulong)Math.Ceiling(91.6*1024) },
            new UserAvatar(){ Id = 8, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/8.jpg", Size = 110*1024 },
            new UserAvatar(){ Id = 9, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/9.jpg", Size = 106*1024 },
        };

        modelBuilder.Entity<UserAvatar>().HasData(userAvatars);

        var defaultAvatars = new List<DefaultAvatar>()
        {
            new DefaultAvatar(){ Id = 1, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/1.jpg", Size = 204*1024, },
            new DefaultAvatar(){ Id = 2, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/2.jpg", Size = 30*1024, },
            new DefaultAvatar(){ Id = 3, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/3.jpg", Size = (ulong)Math.Ceiling(85.7*1024), },
            new DefaultAvatar(){ Id = 4, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/4.jpg", Size = 156*1024,  },
            new DefaultAvatar(){ Id = 5, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/5.jpg", Size = (ulong)Math.Ceiling(71.5*1024), },
            new DefaultAvatar(){ Id = 6, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/6.jpg", Size = 106*1024, },
            new DefaultAvatar(){ Id = 7, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/7.jpg", Size = (ulong)Math.Ceiling(91.6*1024), },
            new DefaultAvatar(){ Id = 8, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/8.jpg", Size = 110*1024, },
            new DefaultAvatar(){ Id = 9, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/9.jpg", Size = 106*1024,  },
            new DefaultAvatar(){ Id = 10, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/10.jpg", Size = 111*1024,  },
            new DefaultAvatar(){ Id = 11, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/11.png", Size = 664*1024,  },
            new DefaultAvatar(){ Id = 12, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/12.gif", Size = 879*1024,  },
            new DefaultAvatar(){ Id = 13, CreateTime = nowTime, Path = "/Raw/DefaultAvatar/13.gif", Size = 731*1024,  },
        };

        modelBuilder.Entity<DefaultAvatar>().HasData(defaultAvatars);

        var friendApplys = new List<FriendApply>()
        {
            new(){ Id = 1, CreateTime = nowTime, UserId = 1, FriendId = 2, UpdateTime = nowTime, Remark = "很高兴认识你。", Status = ApplyStatus.Rejected },
            new(){ Id = 2, CreateTime = nowTime, UserId = 1, FriendId = 2, UpdateTime = nowTime, Remark = "Nice to meet you.", Status = ApplyStatus.Passed },
            new() {Id = 3, CreateTime = nowTime, UserId = 1, FriendId = 3, UpdateTime = nowTime, Remark = "hi!", Status = ApplyStatus.Applied }
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

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                // p => p.IsDeleted == false
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck = Expression.Lambda(Expression.Equal(Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)), Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
        }
    }
}