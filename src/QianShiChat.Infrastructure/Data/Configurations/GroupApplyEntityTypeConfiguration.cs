namespace QianShiChat.Infrastructure.Data.Configurations;

public class GroupApplyEntityTypeConfiguration : IEntityTypeConfiguration<GroupApply>
{
    public void Configure(EntityTypeBuilder<GroupApply> builder)
    {
        builder.ToTable(nameof(GroupApply));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.GroupId)
            .IsRequired();

        builder.HasOne(e => e.Group)
            .WithMany(e => e.GroupApplies)
            .IsRequired(false);

        builder.Property(p => p.UserId)
            .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.GroupApplys)
            .IsRequired(false);

        builder.Property(p => p.Remark)
            .HasMaxLength(255);
    }
}
