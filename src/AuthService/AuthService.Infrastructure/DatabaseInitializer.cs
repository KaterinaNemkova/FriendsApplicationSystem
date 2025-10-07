using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

public class DatabaseInitializer
{
     public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FriendsAppDbContext>();

            await context.Database.MigrateAsync();

            await SeedRolesAsync(serviceProvider);
            await SeedUsersAsync(serviceProvider);
        }

     public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = ["Admin", "User"];
            foreach (var role in roles)
            {
                if (!await roleManager.Roles.AnyAsync(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

     public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var users = new[]
            {
                new { FullName = "Admin", UserName = "Admin", Email = "admin@example.com", Password = "Admin123@", Role = "Admin" },
                new { FullName = "User", UserName = "User", Email = "user@example.com", Password = "User123@", Role = "User" },
            };

            foreach (var userData in users)
            {
                if (await userManager.FindByEmailAsync(userData.Email) is null)
                {
                    var user = new AppUser
                    {
                        FullName = userData.FullName,
                        UserName = userData.UserName,
                        Email = userData.Email,
                        EmailConfirmed = true,
                    };

                    var result = await userManager.CreateAsync(user, userData.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userData.Role);
                    }
                }
            }
        }
}