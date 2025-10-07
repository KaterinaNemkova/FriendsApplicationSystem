using ApiGateway.AuthenticationExtension;
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

builder.Services.AddHttpContextAccessor();

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Configuration.AddOcelotWithSwaggerSupport((options) =>
{
    options.Folder = "OcelotConfigurations";
    options.FileOfSwaggerEndPoints = "ocelot.swagger";
});

builder.Services.AddTransient<CookieToHeaderHandler>();

builder.Services.AddOcelot().AddDelegatingHandler<CookieToHeaderHandler>();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.MapControllers();

app.Run();

