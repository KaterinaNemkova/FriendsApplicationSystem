using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.GrpcServer.Services;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.HangfireJobs;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.DataProtection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(50051, o =>
    {
        o.Protocols = HttpProtocols.Http2;
    });
});
builder.Services.AddData(builder.Configuration);

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IDeleteUncorfimedUserService, DeleteUnconfirmedUserJobService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<FriendsAppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthServiceImpl>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();