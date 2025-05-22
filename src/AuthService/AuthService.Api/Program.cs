using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Filters;
using AuthService.Infrastructure.MyIdentityApi;
using AuthService.Infrastructure.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

builder.Services.AddData();

builder.Services.AddPresentation();

builder.Services.AddEmailService(builder.Configuration);
builder.Services.ConfigureUserGrpcClient(builder.Configuration);
var app = builder.Build();
app.ApplyMigrations();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapMyIdentityApi<ApplicationUser>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
{
    Authorization = new[] { new HangfireAuthFilter() }
});

app.Run();