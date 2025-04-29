using UserService.Infrastructure;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

builder.Services.AddDb(builder.Configuration);

builder.Services.AddCloudinary(builder.Configuration);

builder.Services.AddRepresentation();

builder.Services.AddDependencies();

builder.Services.ConfigureRabbitMQ(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();