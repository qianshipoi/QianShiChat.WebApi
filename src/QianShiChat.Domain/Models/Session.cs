namespace QianShiChat.Domain.Models;

public class Session : IAuditable, ISoftDelete
{
    public string Id { get; set; } = default!;
    public ChatMessageSendType Type { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public long MessagePosition { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }

    public virtual UserInfo? FromUser { get; set; }
}

public class SessionInfoEntityTypeConfiguration : EntityTypeConfiguration<Session>
{
    public override void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(p => new { p.Id, p.FromId });
        builder.HasIndex(p => p.Id);
        builder.HasIndex(p => p.ToId);
        builder.HasIndex(p => p.FromId);
        builder.HasIndex(p => p.Type);
        builder.HasIndex(p => p.IsDeleted);

        builder.HasOne(e => e.FromUser)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.FromId)
            .IsRequired();
        builder.Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(64);
        builder.Property(p => p.ToId)
            .IsRequired();
        builder.Property(p => p.Type)
            .IsRequired();
        builder.Property(p => p.MessagePosition)
            .IsRequired();
        builder.Property(p => p.CreateTime)
            .IsRequired();
        builder.Property(p => p.UpdateTime)
            .IsRequired();
        builder.Property(p => p.IsDeleted)
            .IsRequired();

        base.Configure(builder);
    }
}
