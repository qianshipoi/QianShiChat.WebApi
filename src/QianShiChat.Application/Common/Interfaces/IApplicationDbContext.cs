namespace QianShiChat.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserInfo> UserInfos { get; set; }
    DbSet<UserAvatar> UserAvatars { get; set; }
    DbSet<DefaultAvatar> DefaultAvatars { get; set; }
    DbSet<UserRealtion> UserRealtions { get; set; }
    DbSet<FriendApply> FriendApplies { get; set; }
    DbSet<ChatMessage> ChatMessages { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<UserGroupRealtion> UserGroupRealtions { get; set; }
    DbSet<GroupApply> GroupApplies { get; set; }
    DbSet<Attachment> Attachments { get; set; }
    DbSet<Room> Rooms { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
