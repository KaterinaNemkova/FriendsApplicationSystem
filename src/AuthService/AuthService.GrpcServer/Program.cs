using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.GrpcServer.Services;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.HangfireJobs;
using AuthService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(50051, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
builder.Services.AddData();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IDeleteUncorfimedUserService, DeleteUnconfirmedUserJobService>();
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<FriendsAppDbContext>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthServiceImpl>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();