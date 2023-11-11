namespace QianShiChat.Infrastructure.Data.Configurations;

public class UserRealtionEntityTypeConfiguration : IEntityTypeConfiguration<UserRealtion>
{
    public void Configure(EntityTypeBuilder<UserRealtion> builder)
    {
        builder.ToTable(nameof(UserRealtion));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
           .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.Realtions)
            .IsRequired(false);

        builder.Property(p => p.FriendId)
           .IsRequired();
        builder.HasOne(e => e.Friend)
            .WithMany(e => e.Friends)
            .IsRequired(false);

        builder.Property(p => p.FriendGroupId)
            .HasDefaultValue(0)
            .IsRequired();
    }
}
