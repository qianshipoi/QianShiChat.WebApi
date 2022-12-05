using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers;

namespace Microsoft.Extensions.DependencyInjection;

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
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
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
                .WithIdentity(messageCursorJobKey + "_trigger")
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

    /// <summary>
    /// Add open api
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "QianShiChat API",
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            //添加授权
            var schemeName = "Bearer";
            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "请输入不带有Bearer的Token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = schemeName.ToLowerInvariant(),
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = schemeName
                        }
                    },
                    new string[0]
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Add image conversion.
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddImageConversion(this IServiceCollection services)
    {
        services.AddImageSharp(options =>
               {
                   options.Configuration = SixLabors.ImageSharp.Configuration.Default;
                   options.BrowserMaxAge = TimeSpan.FromDays(7);
                   options.CacheMaxAge = TimeSpan.FromDays(365);
                   options.CacheHashLength = 8;
                   options.OnParseCommandsAsync = _ => Task.CompletedTask;
                   options.OnBeforeSaveAsync = _ => Task.CompletedTask;
                   options.OnProcessedAsync = _ => Task.CompletedTask;
                   options.OnPrepareResponseAsync = _ => Task.CompletedTask;
               })
               .SetRequestParser<QueryCollectionRequestParser>()
               .Configure<PhysicalFileSystemCacheOptions>(options =>
               {
                   options.CacheRootPath = null;
                   options.CacheFolder = "is-cache";
                   options.CacheFolderDepth = 8;
               })
               .SetCache<PhysicalFileSystemCache>()
               .SetCacheKey<UriRelativeLowerInvariantCacheKey>()
               .SetCacheHash<SHA256CacheHash>()
               .Configure<PhysicalFileSystemProviderOptions>(options =>
               {
                   options.ProviderRootPath = null;
               })
               .AddProvider<PhysicalFileSystemProvider>()
               .AddProcessor<ResizeWebProcessor>()
               .AddProcessor<FormatWebProcessor>()
               .AddProcessor<BackgroundColorWebProcessor>()
               .AddProcessor<QualityWebProcessor>()
               .AddProcessor<AutoOrientWebProcessor>();

        return services;
    }
}