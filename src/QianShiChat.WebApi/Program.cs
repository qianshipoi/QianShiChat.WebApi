// config distributed id.
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Processors;
using SixLabors.ImageSharp.Web.Providers;

var options = new IdGeneratorOptions(1);
YitIdHelper.SetIdGenerator(options);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// config mvc builder.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    });

// config signalr server builder.
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!);

// config infrastructure.
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy(AppConsts.MyAllowSpecificOrigins, builder =>
        {
            builder.WithOrigins("https://www.kuriyama.top");
        });
    })
    .AddMediatR(x => x.AsScoped(), typeof(Program))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddOpenApi()
    .AddJwtAuthentication(builder.Configuration)
    .AddCache(builder.Configuration)
    .AddChatDbContext(builder.Configuration)
    .AddAutoDI()
    .AddSaveChatMessageJob();

builder.Services.AddDirectoryBrowser();
builder.Services.AddImageSharp(options =>
{
    options.Configuration = Configuration.Default;
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

var app = builder.Build();

app.UseOpenApi();

app.UseCors(AppConsts.MyAllowSpecificOrigins);

app.UseDefaultFiles();
app.UseImageSharp();
app.UseStaticFiles();
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Raw")),
    RequestPath = new PathString("/Raw")
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat").RequireAuthorization();

app.Run();