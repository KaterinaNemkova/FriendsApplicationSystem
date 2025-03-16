using AuthService.Domain.Entities;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<FriendsAppDbContext>()
    .AddApiEndpoints();

builder.Services.AddData();
// builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<FriendsAppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.MapIdentityApi<ApplicationUser>();

app.Run();