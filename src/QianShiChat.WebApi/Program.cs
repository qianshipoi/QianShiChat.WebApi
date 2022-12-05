

// config distributed id.
using SixLabors.ImageSharp.Web.DependencyInjection;

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
    .AddSaveChatMessageJob()
    .AddImageConversion();

builder.Services.AddDirectoryBrowser();


var app = builder.Build();

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

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat").RequireAuthorization();

app.Run();