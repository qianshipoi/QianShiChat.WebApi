namespace QianShiChat.Infrastructure.Data.Configurations;

public class FriendApplyEntityTypeConfiguration : IEntityTypeConfiguration<FriendApply>
{
    public void Configure(EntityTypeBuilder<FriendApply> builder)
    {
        builder.ToTable(nameof(FriendApply));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.FriendId)
            .IsRequired();
        builder.HasOne(e => e.Friend)
            .WithMany(e => e.FriendApplyFriends)
            .IsRequired();

        builder.Property(p => p.UserId)
            .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.FriendApplyUsers)
            .IsRequired();

        builder.Property(p => p.Remark)
            .HasMaxLength(255);
    }
}
