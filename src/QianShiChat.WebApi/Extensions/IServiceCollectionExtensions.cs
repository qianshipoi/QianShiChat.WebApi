using EasyCaching.Serialization.SystemTextJson.Configurations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using QianShiChat.WebApi;
using QianShiChat.WebApi.BackgroundHost;
using QianShiChat.WebApi.Models;

using Quartz;

using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service Extension
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加JWT认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtOptions.OptionsKey);

            var jwtOptions = new JwtOptions();
            section.Bind(jwtOptions);
            services.AddOptions();
            services.Configure<JwtOptions>(section);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ClockSkew = TimeSpan.FromSeconds(5),
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });
            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// 添加ChatDbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddChatDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("Default"), ServerVersion.Parse("8.0.31"));
            });
            return services;
        }

        /// <summary>
        /// 添加缓存服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddEasyCaching(setup =>
            {
                setup.UseRedis(configuration);
                setup.WithSystemTextJson("mymsgpack");
            });

            return services;
        }

        /// <summary>
        /// 后台持久化消息服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSaveChatMessageJob(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                // base Quartz scheduler, job and trigger configuration
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = nameof(SaveChatMessageJob);
                q.AddJob<SaveChatMessageJob>(options => options.WithIdentity(jobKey));
                q.AddTrigger(options => options
                    .ForJob(jobKey)
                    .WithIdentity(jobKey + "_trigger")
                    .WithDailyTimeIntervalSchedule(opt => opt.WithIntervalInMinutes(5))
                );

                var messageCursorJobKey = nameof(SaveMessageCursorJob);
                q.AddJob<SaveMessageCursorJob>(options => options.WithIdentity(messageCursorJobKey));
                q.AddTrigger(options => options
                    .ForJob(messageCursorJobKey)
                    .WithIdentity(jobKey + "_trigger")
                    .WithDailyTimeIntervalSchedule(opt => opt.WithIntervalInMinutes(2)));
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
