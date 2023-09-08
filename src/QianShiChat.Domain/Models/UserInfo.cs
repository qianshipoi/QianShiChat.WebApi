namespace QianShiChat.Domain.Models;

public class UserInfo : IAuditable, ISoftDelete
{
    public UserInfo()
    {
        Realtions = new HashSet<UserRealtion>();
        Friends = new HashSet<UserRealtion>();
        FriendApplyUsers = new HashSet<FriendApply>();
        FriendApplyFriends = new HashSet<FriendApply>();
        GroupApplys = new HashSet<GroupApply>();
        UserAvatars = new HashSet<UserAvatar>();
        Groups = new HashSet<Group>();
        UserGroups = new HashSet<UserGroupRealtion>();
        Attachments = new HashSet<Attachment>();
        Sessions = new HashSet<Session>();
        Messages = new HashSet<ChatMessage>();
    }

    public int Id { get; set; }
    public string Account { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public long CreateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long UpdateTime { get; set; }
    public long DeleteTime { get; set; }

    public virtual ICollection<UserRealtion> Realtions { get; set; }
    public virtual ICollection<UserRealtion> Friends { get; set; }
    public virtual ICollection<FriendApply> FriendApplyUsers { get; set; }
    public virtual ICollection<FriendApply> FriendApplyFriends { get; set; }
    public virtual ICollection<GroupApply> GroupApplys { get; set; }
    public virtual ICollection<UserAvatar> UserAvatars { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<UserGroupRealtion> UserGroups { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; }
    public virtual ICollection<Session> Sessions { get; set; }
    public virtual ICollection<ChatMessage> Messages { get; set; }
}
