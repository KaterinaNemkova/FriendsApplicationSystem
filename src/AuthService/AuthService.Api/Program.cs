using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.MyIdentityApi;
using AuthService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

builder.Services.AddData(builder.Configuration);
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddPresentation();

builder.Services.AddEmailService(builder.Configuration);
builder.Services.ConfigureUserGrpcClient(builder.Configuration);
var app = builder.Build();
app.ApplyMigrations();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapMyIdentityApi<ApplicationUser>();

app.Run();