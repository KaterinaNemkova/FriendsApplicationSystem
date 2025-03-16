using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AuthService.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        services.AddDbContext<FriendsAppDbContext>(options =>
        {
            options.UseNpgsql(
                "Host=postgre_db;Database=FriendsApp;Username=postgres;Password=1234"
            );
        });
        services.AddDataProtection(); // Добавление DataProtection
        services.AddSingleton<TimeProvider>(TimeProvider.System); // Регистрация TimeProvider
        return services;
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<FriendsAppDbContext>();
            
            dbContext.Database.Migrate();
        }
    }

}