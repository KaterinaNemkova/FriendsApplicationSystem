using AuthService.Domain.Entities;
using AuthService.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();

builder.Services.AddData();

builder.Services.AddEmailService(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapIdentityApi<ApplicationUser>();

app.Run();