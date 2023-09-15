namespace QianShiChat.Infrastructure.Data.Configurations;

public class ChatMessageEntityTypeConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable(nameof(ChatMessage));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(p => p.FromId)
            .IsRequired();
        builder.HasOne(e => e.FromUser)
            .WithMany(e => e.Messages)
            .IsRequired(false);
        builder.Property(p => p.ToId)
            .IsRequired();
        builder.Property(p => p.RoomId)
            .IsRequired()
            .HasMaxLength(32);
        builder.Property(p => p.SendType)
            .IsRequired();
        builder.Property(p => p.MessageType)
            .IsRequired();
        builder.Property(p => p.Content)
            .IsRequired();

        builder.HasMany(p => p.Attachments)
            .WithMany(a => a.Messages)
            .UsingEntity<MessageAttachment>(
                l => l.HasOne(ma => ma.Attachment).WithMany().HasForeignKey(ma => ma.AttachmentId).IsRequired(false),
                r => r.HasOne(ma => ma.Message).WithMany().HasForeignKey(ma => ma.MessageId).IsRequired(false));
    }
}
