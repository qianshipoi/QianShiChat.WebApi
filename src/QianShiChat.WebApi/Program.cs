// config distributed id.
using Microsoft.AspNetCore.HttpOverrides;

var options = new IdGeneratorOptions(1);
YitIdHelper.SetIdGenerator(options);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// config mvc builder.
builder.Services
    .AddControllers(setup =>
    {
        setup.Filters.Add<ClientAuthotizationFilter>();
        setup.Filters.Add<ResultWrapperFilter>();
        setup.Filters.Add<GlobalExceptionFilter>();
    })
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    });

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
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
    .AddCors(options =>
    {
        options.AddPolicy(AppConsts.MyAllowSpecificOrigins, builder =>
        {
            builder.WithOrigins("https://www.kuriyama.top", "http://127.0.0.1:5173")
            .WithHeaders("*")
            .WithMethods("*");
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
    .AddSaveChatMessageJob()
    .AddImageConversion()
    .AddHttpContextAccessor();

builder.Services.AddDirectoryBrowser();

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

app.MapGet("/", async (context) =>
{
    await context.Response.WriteAsync(context.Request.GetBaseUrl());
});

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat");

app.Run();