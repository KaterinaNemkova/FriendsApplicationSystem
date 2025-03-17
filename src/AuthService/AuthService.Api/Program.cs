using AuthService.Domain.Entities;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.MyIdentityApi;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddData(builder.Configuration);

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
app.MapMyIdentityApi<ApplicationUser>();

app.Run();