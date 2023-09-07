namespace QianShiChat.Infrastructure.Data.Configurations;

public class UserGroupRealtionEntityTypeConfiguration : IEntityTypeConfiguration<UserGroupRealtion>
{
    public void Configure(EntityTypeBuilder<UserGroupRealtion> builder)
    {
        builder.ToTable(nameof(UserGroupRealtion));
        builder.HasKey(x => new { x.UserId, x.GroupId });
        builder.HasOne(e => e.User)
            .WithMany(e => e.UserGroups)
            .IsRequired(false);
        builder.HasOne(e => e.Group)
            .WithMany(e => e.UserGroupRealtions)
            .IsRequired(false);
    }
}
