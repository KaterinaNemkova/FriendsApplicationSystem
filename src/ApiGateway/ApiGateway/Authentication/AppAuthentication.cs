using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Authentication;

public static class AppAuthentication
{
    public static void AddAppAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://authservice_api:5001";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "api" 
                };
            });
    }
}