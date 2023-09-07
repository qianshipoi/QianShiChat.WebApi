namespace QianShiChat.Infrastructure.Data.Configurations;

public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable(nameof(Group));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
            .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.Groups)
            .IsRequired(false);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(p => p.Avatar)
            .HasMaxLength(512);
    }
}
