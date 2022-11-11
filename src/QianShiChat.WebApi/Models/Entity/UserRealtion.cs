using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QianShiChat.WebApi.Models
{
    /// <summary>
    /// 用户关系表
    /// </summary>
    public class UserRealtion
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 朋友编号
        /// </summary>
        public int FriendId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public UserInfo User { get; set; }

        public UserInfo Friend { get; set; }
    }

    public class UserRealtionEntityTypeConfiguration : IEntityTypeConfiguration<UserRealtion>
    {
        public void Configure(EntityTypeBuilder<UserRealtion> builder)
        {
            builder.ToTable(nameof(UserRealtion));
            builder.HasIndex(x => x.Id);

            builder.HasOne(p => p.User)
                .WithMany(p => p.Realtions);
            builder.HasOne(p => p.Friend)
                .WithMany(p => p.Friends);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasComment("用户关系表主键");
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("用户编号");
            builder.Property(x => x.FriendId)
                .IsRequired()
                .HasComment("朋友编号");
            builder.Property(x => x.CreateTime)
                .IsRequired()
                .HasComment("创建时间");
        }
    }
}
