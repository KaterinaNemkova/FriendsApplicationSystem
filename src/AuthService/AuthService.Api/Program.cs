using AuthService.Domain.Entities;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddControllers();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FriendsAppDbContext>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddIdentityCore<ApplicationUser>()
//     .AddEntityFrameworkStores<FriendsAppDbContext>()
//     .AddApiEndpoints();

builder.Services.AddData();
// builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<FriendsAppDbContext>();

var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}


app.MapIdentityApi<ApplicationUser>();

app.Run();