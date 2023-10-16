namespace QianShiChat.Infrastructure.Data.Configurations;

public class FriendGroupEntityTypeConfigration : IEntityTypeConfiguration<FriendGroup>
{
    public void Configure(EntityTypeBuilder<FriendGroup> builder)
    {
        builder.ToTable(nameof(FriendGroup));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
           .IsRequired()
           .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
         .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.FriendGroups)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(32);
    }
}
