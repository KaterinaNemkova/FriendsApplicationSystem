using AuthService.Api.Endpoints;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Filters;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// var envPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../.env");
//
// if (File.Exists(envPath))
// {
//     DotNetEnv.Env.Load(envPath);
// }

builder.Services.AddData(builder.Configuration);

builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddEmailService(builder.Configuration);
builder.Services.ConfigureUserGrpcClient(builder.Configuration);
var app = builder.Build();
//app.ApplyMigrations();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapAccountEndpoint();

app.UseRouting();
using (var scope = app.Services.CreateScope())
{
    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider);
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
{
    Authorization = new[] { new HangfireAuthFilter() },
});

app.Run();