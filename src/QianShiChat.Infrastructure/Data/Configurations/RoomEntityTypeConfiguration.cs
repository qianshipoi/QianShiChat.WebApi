﻿namespace QianShiChat.Infrastructure.Data.Configurations;

public class RoomEntityTypeConfiguration : EntityTypeConfiguration<Session>
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
