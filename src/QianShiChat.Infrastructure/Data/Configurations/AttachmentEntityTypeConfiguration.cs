namespace QianShiChat.Infrastructure.Data.Configurations;

public class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Hash);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(p => p.RawPath)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(p => p.PreviewPath)
            .HasMaxLength(512);

        builder.Property(p => p.ContentType)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Hash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(p => p.Size)
            .IsRequired();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(e => e.Attachments)
            .IsRequired(false);

        builder.Property(x => x.CreateTime)
            .IsRequired();

        builder.Property(x => x.UpdateTime)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DeleteTime)
            .IsRequired();
    }
}
