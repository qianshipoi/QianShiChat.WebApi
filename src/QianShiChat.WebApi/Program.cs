// config distributed id.
var options = new IdGeneratorOptions(1);
YitIdHelper.SetIdGenerator(options);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// config mvc builder.
builder.Services
    .AddControllers(setup => {
        setup.Filters.Add<ClientAuthotizationFilter>();
        setup.Filters.Add<ResultWrapperFilter>();
        setup.Filters.Add<GlobalExceptionFilter>();
    })
    .AddJsonOptions(options => {
        //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    });

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
        options.AddPolicy(AppConsts.MyAllowSpecificOrigins, builder => {
            builder
            .WithOrigins("https://www.kuriyama.top", "http://127.0.0.1:5173", "http://127.0.0.1:3000")
            .WithHeaders("*")
            .WithMethods("*")
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
    .AddChatDbContext(builder.Configuration)
    .AddAutoDI()
    .AddSaveChatMessageJob()
    .AddImageConversion()
    .AddHttpContextAccessor()
    .AddDirectoryBrowser();

builder.Services.AddSingleton<TusDiskStorageOptionHelper>();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", async (context) => {
    await context.Response.WriteAsync(context.Request.GetBaseUrl());
});

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

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat");

app.Run();
