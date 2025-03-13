using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AuthService.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddData(IServiceCollection services)
    {
        services.AddDbContext<FriendsAppDbContext>(x =>
        {
            x.UseNpgsql(
                "Host=db;Database=FriendsApp;Username=postgres;Password=1234"
            );
        });
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
    

}