using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Authentication;

public static class AppAuthentication
{
    public static void AddAppAuthentication(this IServiceCollection services, IConfigurationBuilder configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = configuration["Jwt:Issuer"],
                    //ValidAudience = configuration["Jwt:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(
                        //Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireUserRole", policy => 
                policy.RequireRole("User"));
            options.AddPolicy("RequireAdminRole", policy => 
                policy.RequireRole("Admin"));
        });
    }
}