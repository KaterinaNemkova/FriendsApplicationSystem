using NotificationService.Infrastructure.Extensions;
using NotificationService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

builder.Services.AddDb(builder.Configuration);

builder.Services.AddControllers();

builder.Services.ConfigureAuthGrpcClient(builder.Configuration);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddSingleton<TelegramBotService>();

var app = builder.Build();

var telegramBotService = app.Services.GetRequiredService<TelegramBotService>();
telegramBotService.Start();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();