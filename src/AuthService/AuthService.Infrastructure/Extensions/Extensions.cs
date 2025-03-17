using AuthService.Domain.Entities;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        services.AddDataProtection();
        services.AddSingleton<TimeProvider>(TimeProvider.System);
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

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen();
        
        services.AddAuthorization();
        
        services.AddAuthentication();
        
        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>();
        
        return services;

    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailSender, EmailService>();
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Smtp));
        return services;
    }

}