namespace QianShiChat.Infrastructure.Data.Configurations;

public class MessageCursorEntityTypeConfiguration : IEntityTypeConfiguration<MessageCursor>
{
    public void Configure(EntityTypeBuilder<MessageCursor> builder)
    {
        builder.ToTable(nameof(MessageCursor));
        builder.HasKey(x => x.UserId);

        builder.Property(p => p.Postiton)
            .HasDefaultValue(0L);
    
        builder.HasOne(e => e.User)
            .WithOne(e => e.Cursor)
            .IsRequired(false);
    }
}
