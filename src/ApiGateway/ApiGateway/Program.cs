using ApiGateway.AuthenticationExtension;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
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

// 1. CORS всегда в самом начале
app.UseCors("AllowAngular");

// 2. ДОРОГА №1: Если запрос начинается на /api — это ТОЛЬКО Ocelot
app.MapWhen(context => context.Request.Path.StartsWithSegments("/api"), apiApp =>
{
    // В этой ветке нет FallbackToFile, поэтому HTML не вернется никогда
    apiApp.UseOcelot().Wait();
});

// 3. ДОРОГА №2: Все остальные запросы (Фронтенд)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwaggerForOcelotUI(opt => {
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Если файл не найден в wwwroot (например, /login), отдаем index.html
app.MapFallbackToFile("index.html");

app.Run();