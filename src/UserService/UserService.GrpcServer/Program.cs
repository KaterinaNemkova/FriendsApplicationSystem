using Microsoft.AspNetCore.Server.Kestrel.Core;
using UserService.Application.Contracts;
using UserService.GrpcServer.Services;
using UserService.Infrastructure.Extensions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(50052, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
builder.Services.AddDb(builder.Configuration);

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.ConfigureRabbitMQ(builder.Configuration);
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserProfileServiceImpl>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();