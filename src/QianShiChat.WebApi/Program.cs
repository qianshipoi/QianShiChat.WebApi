// config distributed id.

using QianShiChat.Infrastructure.Data;

var options = new IdGeneratorOptions(1);
YitIdHelper.SetIdGenerator(options);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// config mvc builder.
builder.Services
    .AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services
    .AddControllers(setup => {
        setup.Filters.Add<ClientAuthotizationFilter>();
        setup.Filters.Add<ResultWrapperFilter>();
        setup.Filters.Add<GlobalExceptionFilter>();
    })
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "zh-CN", "en-US" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
    options.ApplyCurrentCultureToResponseHeaders = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// config signalr server builder.
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!);

// config infrastructure.
builder.Services
    .AddCors(options => {
        var corsSection = builder.Configuration.GetSection("Cors");
        var corsOptions = new CorsOptions();
        corsSection.Bind(corsOptions);

        options.AddPolicy(AppConsts.MyAllowSpecificOrigins, builder => {
            builder
            .WithOrigins(corsOptions.WithOrigins)
            .WithHeaders("*")
            .WithMethods("*")
            .AllowCredentials()
            .WithExposedHeaders(CustomResponseHeader.AccessToken)
            .WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders());
        });
    })
    .AddMediatR(configure => configure.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddOpenApi()
    .AddJwtAuthentication(builder.Configuration)
    .AddCache(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddAutoDI()
    .AddSaveChatMessageJob()
    .AddImageConversion()
    .AddHttpContextAccessor()
    .AddDirectoryBrowser();

builder.Services.AddCarter();

builder.Services.AddSingleton<TusDiskStorageOptionHelper>();
builder.Services.AddSingleton(serviceProvider => serviceProvider.CreateTusConfigurationForCleanupService());
builder.Services.AddHostedService<ExpiredFilesCleanupService>();

var app = builder.Build();

await app.InitialiseDatabaseAsync();

app.UseRequestLocalization();

app.UseForwardedHeaders();
app.UseOpenApi();

app.UseCors(AppConsts.MyAllowSpecificOrigins);

app.UseDefaultFiles();
app.UseImageSharp();
app.UseStaticFiles();
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Raw")),
    RequestPath = new PathString("/Raw")
});

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (context) => context.Response.WriteAsync(context.Request.GetBaseUrl()));

app.MapTus("/api/tusfiles", HttpContextExtensions.TusConfigurationFactory)
    .RequireAuthorization()
    .AddEndpointFilter(async (context, next) => {
        var isAuthorization = await context.HttpContext.ExistsLegalTokenAsync();
        if (!isAuthorization)
        {
            return Results.Unauthorized();
        }
        return await next(context);
    });
app.MapCarter();
app.MapControllers();
app.MapHub<ChatHub>("/Hubs/Chat");
app.Run();
