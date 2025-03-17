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
    public static IServiceCollection AddData(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<FriendsAppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        // services.AddDataProtection();
        // services.AddSingleton<TimeProvider>(TimeProvider.System);
        return services;
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using FriendsAppDbContext context = scope.ServiceProvider.GetRequiredService<FriendsAppDbContext>();

        context.Database.Migrate();
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen();
        
        services.AddAuthorization();
        
        services.AddAuthentication();
        
        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>();
        
        //unique email, confirm email 
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            //options.SignIn.RequireConfirmedEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });
        return services;

    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailSender, EmailService>();
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Smtp));
        return services;
    }

}