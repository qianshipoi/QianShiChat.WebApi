namespace QianShiChat.Infrastructure.Data.Configurations;

public class DefaultAvatarEntityTypeConfiguration : IEntityTypeConfiguration<DefaultAvatar>
{
    public void Configure(EntityTypeBuilder<DefaultAvatar> builder)
    {
        builder.ToTable(nameof(DefaultAvatar));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Path)
            .IsRequired()
            .HasMaxLength(255);
    }
}
