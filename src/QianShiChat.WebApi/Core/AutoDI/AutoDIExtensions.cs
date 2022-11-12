using QianShiChat.WebApi.Core.AutoDI;

namespace Microsoft.Extensions.DependencyInjection
{
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
                var interfaces = type.GetInterfaces();
                interfaces.ToList().ForEach(x =>
                {
                    if (lifetimeType == typeof(ISingleton)) services.AddSingleton(x, type);
                    if (lifetimeType == typeof(IScoped)) services.AddScoped(x, type);
                    if (lifetimeType == typeof(ITransient)) services.AddTransient(x, type);
                });
            }
        }
    }
}
