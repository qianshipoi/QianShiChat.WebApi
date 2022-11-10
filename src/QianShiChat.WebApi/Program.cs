using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using QianShiChat.WebApi;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.Parse("8.0.31"));
});

builder.Services.AddMediatR(x => x.AsScoped(), typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    context.Database.EnsureCreated();
}

app.MediateGet<ExampleRequest>("example/{name}");
app.MediatePost<ExampleRequest>("example");

app.Run();

