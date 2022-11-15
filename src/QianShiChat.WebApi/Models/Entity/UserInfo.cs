using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using QianShiChat.Common.Extensions;

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace QianShiChat.WebApi.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string Account { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Avatar { get; set; }

        public long CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        [InverseProperty(nameof(UserRealtion.User))]
        public List<UserRealtion> Realtions { get; set; } = new();

        [InverseProperty(nameof(UserRealtion.Friend))]
        public List<UserRealtion> Friends { get; set; } = new();

        [InverseProperty(nameof(FriendApply.User))]
        public List<FriendApply> FriendApplyUsers { get; set; } = new();

        [InverseProperty(nameof(FriendApply.Friend))]
        public List<FriendApply> FriendApplyFriends { get; set; } = new();

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
            builder.Property(x => x.CreateTime)
                .IsRequired()
                .HasComment("创建时间");
            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("是否已删除");

            builder.HasData(
                new UserInfo { Id = 1, Account = "admin", CreateTime = Timestamp.Now, Password = "E10ADC3949BA59ABBE56E057F20F883E", NickName = "Admin" },
                    new UserInfo { Id = 2, Account = "qianshi", CreateTime = Timestamp.Now, Password = "E10ADC3949BA59ABBE56E057F20F883E", NickName = "千矢" },
                    new UserInfo { Id = 3, Account = "kuriyama", CreateTime = Timestamp.Now, Password = "E10ADC3949BA59ABBE56E057F20F883E", NickName = "栗山未来" }
                );
        }
    }
}
