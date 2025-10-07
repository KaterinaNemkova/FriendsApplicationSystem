using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.AuthenticationExtension;

public static class AuthConfiguration
{
    public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                var securityKey = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY") 
                                  ?? configuration["JWTSettings:SecurityKey"];
        
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                    ValidateIssuer = true,
                    ValidIssuer = "authservice_api",
                    ValidateAudience = true,
                    ValidAudience = "microservices",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
        
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("accessToken", out var token))
                        {
                            context.Token = token;
                        }
                        else if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                        {
                            var header = authHeader.FirstOrDefault();
                            if (!string.IsNullOrEmpty(header) && header.StartsWith("Bearer "))
                            {
                                context.Token = header.Substring("Bearer ".Length);
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
}

public class CookieToHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieToHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        
        if (context != null)
        {
            if (context.Request.Cookies.TryGetValue("accessToken", out var token))
            {
                request.Headers.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"CookieToHeaderHandler: Token added to Authorization header");
            }
            else
            {
                Console.WriteLine($"CookieToHeaderHandler: accessToken cookie not found");
            }
        }
        else
        {
            Console.WriteLine($"CookieToHeaderHandler: HttpContext is null");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}