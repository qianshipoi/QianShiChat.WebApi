using QianShiChat.WebApi.Core.Interceptors;

namespace QianShiChat.WebApi.Models;

public class UserInfo : IAuditable, ISoftDelete
{
    public UserInfo()
    {
        Realtions = new HashSet<UserRealtion>();
        Friends = new HashSet<UserRealtion>();
        FriendApplyUsers = new HashSet<FriendApply>();
        FriendApplyFriends = new HashSet<FriendApply>();
        UserAvatars = new HashSet<UserAvatar>();
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

    public MessageCursor Cursor { get; set; }
}

public class UserInfoEntityTypeConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable(nameof(UserInfo));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasComment("用户表主键");

        builder.Property(x => x.Account)
            .IsRequired()
            .HasMaxLength(32)
            .HasComment("账号");

        builder.Property(x => x.NickName)
            .IsRequired()
            .HasMaxLength(32)
            .HasComment("昵称");
        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(32)
            .HasComment("密码");

        builder.Property(x => x.Description)
            .HasMaxLength(128)
            .HasComment("描述");

        builder.Property(x => x.CreateTime)
            .IsRequired()
            .HasComment("创建时间");
        builder.Property(x => x.UpdateTime)
            .IsRequired()
            .HasComment("修改时间");
        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否已删除");
        builder.Property(x => x.DeleteTime)
            .IsRequired()
            .HasComment("删除时间");
    }
}