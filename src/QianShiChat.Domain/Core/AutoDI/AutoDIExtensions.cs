﻿namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// auto di extensions.
/// </summary>
public static class AutoDIExtensions
{
    /// <summary>
    /// auto di
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutoDI(this IServiceCollection services)
    {
        services.RegisterLifetimesByInhreit(typeof(ISingleton));
        services.RegisterLifetimesByInhreit(typeof(IScoped));
        services.RegisterLifetimesByInhreit(typeof(ITransient));

        return services;
    }

    private static void RegisterLifetimesByInhreit(this IServiceCollection services, Type lifetimeType)
    {
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => lifetimeType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
            .ToList();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces().Where(x=>x != lifetimeType).ToList();

            if (interfaces.Any())
            {
                foreach (var x in interfaces)
                {
                    if (lifetimeType == typeof(ISingleton)) services.AddSingleton(x, type);
                    if (lifetimeType == typeof(IScoped)) services.AddScoped(x, type);
                    if (lifetimeType == typeof(ITransient)) services.AddTransient(x, type);
                }
            }
            else
            {
                if (lifetimeType == typeof(ISingleton)) services.AddSingleton(type);
                if (lifetimeType == typeof(IScoped)) services.AddScoped(type);
                if (lifetimeType == typeof(ITransient)) services.AddTransient(type);
            }
        }
    }
}