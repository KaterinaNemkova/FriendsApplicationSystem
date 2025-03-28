using AuthService.Domain.Entities;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AuthService.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        var envConnectionString = Environment.GetEnvironmentVariable("POSTGRE_DB_CONNECTION_STRING");
        services.AddDbContext<FriendsAppDbContext>(options =>
        {
            options.UseNpgsql(envConnectionString);
        });
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
       
        services.AddAuthentication();
        
        services.AddAuthorization();
        
        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>();
        
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });
        return services;

    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(options=>
        {
            options.Host = Environment.GetEnvironmentVariable("SMTP_HOST");
            options.Port = configuration.GetValue<int>("Smtp:Port");
            options.UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            options.Email = Environment.GetEnvironmentVariable("SMTP_EMAIL");
        });

        services.AddTransient<IEmailSender, EmailService>();
        return services;
    }

}