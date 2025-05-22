
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Configuration.AddOcelotWithSwaggerSupport((options) =>
{
    options.Folder = "OcelotConfigurations";
    options.FileOfSwaggerEndPoints = "ocelot.swagger";
});

builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowAll");

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();