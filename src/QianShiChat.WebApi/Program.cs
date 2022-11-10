using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using QianShiChat.WebApi;
using QianShiChat.WebApi.Hubs;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Requests;
using QianShiChat.WebApi.Services;

using System.Reflection;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("https://www.kuriyama.top");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddChatDbContext(builder.Configuration);

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSignalR();

builder.Services.AddMediatR(x => x.AsScoped(), typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.EnablePersistAuthorization();
    });

    //using var scope = app.Services.CreateScope();
    //var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    //context.Database.EnsureCreated();
}
app.UseCors("MyAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("user", async (ChatDbContext context)
    => await context.UserInfos.ToListAsync())
    .RequireAuthorization()
    .Produces<List<UserInfo>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .WithName("get users.")
    .WithTags("all");

app.MediatePost<AuthRequest>("user")
    .Produces<string>(StatusCodes.Status200OK)
    .Produces<string>(StatusCodes.Status400BadRequest)
    .WithName("auth user.")
    .WithTags("all");

app.MapHub<ChatHub>("/Hubs/Chat").RequireAuthorization();

app.Run();

