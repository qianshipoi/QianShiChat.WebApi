var builder = WebApplication.CreateBuilder(args).Inject();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
