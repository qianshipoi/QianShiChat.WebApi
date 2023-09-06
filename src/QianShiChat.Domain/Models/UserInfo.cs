namespace QianShiChat.Domain.Models;

public class UserInfo : IAuditable, ISoftDelete
{
    public UserInfo()
    {
        Realtions = new HashSet<UserRealtion>();
        Friends = new HashSet<UserRealtion>();
        FriendApplyUsers = new HashSet<FriendApply>();
        FriendApplyFriends = new HashSet<FriendApply>();
        UserAvatars = new HashSet<UserAvatar>();
        Groups = new HashSet<Group>();
        UserGroups = new HashSet<UserGroupRealtion>();
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

    [InverseProperty(nameof(UserRealtion.User))]
    public virtual ICollection<UserRealtion> Realtions { get; set; }

    [InverseProperty(nameof(UserRealtion.Friend))]
    public virtual ICollection<UserRealtion> Friends { get; set; }

    [InverseProperty(nameof(FriendApply.User))]
    public virtual ICollection<FriendApply> FriendApplyUsers { get; set; }

    [InverseProperty(nameof(FriendApply.Friend))]
    public virtual ICollection<FriendApply> FriendApplyFriends { get; set; }

    [InverseProperty(nameof(UserAvatar.User))]
    public virtual ICollection<UserAvatar> UserAvatars { get; set; }

    public MessageCursor Cursor { get; set; } = default!;

    public virtual ICollection<Group> Groups { get; set; } = null!;

    public virtual ICollection<UserGroupRealtion> UserGroups { get; set; } = null!;

    public virtual ICollection<Attachment> Attachments { get; set; } = null!;

    public virtual ICollection<Session>? Sessions { get; set; }
}
