using QianShiChat.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        Guard.IsNotNullOrWhiteSpace(connectionString,nameof(connectionString));

        services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();

        services.AddDbContext<ChatDbContext>((sp, options) => {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseMySql(connectionString, ServerVersion.Parse("8.0.31"), builder => builder.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName));
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ChatDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        //services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

