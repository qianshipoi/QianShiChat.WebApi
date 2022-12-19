namespace QianShiChat.WebApi.Core.Interceptors
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var entityType = typeof(TEntity);

            if (typeof(ISoftDelete).IsAssignableFrom(entityType))
            {
                builder.HasQueryFilter(d => EF.Property<bool>(d, nameof(ISoftDelete.IsDeleted)) == false);
            }
        }
    }
}
