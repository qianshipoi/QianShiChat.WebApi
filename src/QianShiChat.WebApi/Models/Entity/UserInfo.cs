using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QianShiChat.WebApi.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string Account { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }
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
                .ValueGeneratedOnAdd()
                .HasComment("创建时间");
            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("是否已删除");

            builder.HasData(
                new UserInfo { Id = 1, Account = "admin", Password = "123456", NickName = "Admin" }
                );
        }
    }
}
