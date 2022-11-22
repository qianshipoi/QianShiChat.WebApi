using FluentValidation;

using MediatR;

using QianShiChat.WebApi.Hubs;

using System.Reflection;

using Yitter.IdGenerator;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var options = new IdGeneratorOptions(1);
YitIdHelper.SetIdGenerator(options);

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// config mvc builder
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
    });

// config signalr server builder
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!);

// config infrastructure
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins, builder =>
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

var app = builder.Build();

app.UseOpenApi();

app.UseCors("MyAllowSpecificOrigins");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Hubs/Chat").RequireAuthorization();

app.Run();

