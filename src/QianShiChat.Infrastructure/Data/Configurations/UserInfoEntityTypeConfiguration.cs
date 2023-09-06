namespace QianShiChat.Infrastructure.Data.Configurations;

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
