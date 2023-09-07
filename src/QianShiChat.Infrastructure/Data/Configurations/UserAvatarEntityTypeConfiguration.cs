namespace QianShiChat.Infrastructure.Data.Configurations;

public class UserAvatarEntityTypeConfiguration : IEntityTypeConfiguration<UserAvatar>
{
    public void Configure(EntityTypeBuilder<UserAvatar> builder)
    {
        builder.ToTable(nameof(UserAvatar));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
           .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.UserAvatars)
            .IsRequired(false);

        builder.Property(p => p.Path)
            .IsRequired()
            .HasMaxLength(255);
    }
}
