namespace Microsoft.Extensions.DependencyInjection;

public static class DomainDependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(AppSettingsOptions.OptionsKey);
        services.Configure<AppSettingsOptions>(section);

        return services;
    }
}
