using EventsService.Infrastructure;
using EventsService.Infrastructure.Extensions;
using EventsService.Infrastructure.Filters;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}



builder.Services.AddDependencies();

builder.Services.AddRepresentation();
builder.Services.AddDb(builder.Configuration);
builder.Services.ConfigureRabbitMQ(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunJobs();

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthFilter() },
    });

app.Run();