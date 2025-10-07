using System.Text;
using AuthService.Infrastructure.HangfireJobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Extensions;

using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Options;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services, IConfiguration configuration)
    {

        var envConnectionString = Environment.GetEnvironmentVariable("POSTGRES_DB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(envConnectionString))
        {
            throw new InvalidOperationException("POSTGRES_DB_CONNECTION_STRING environment variable is not set");
        }

        var hangfireConnectionString = Environment.GetEnvironmentVariable("HANGFIRE_CONNECTION");
        services.AddDbContext<FriendsAppDbContext>(
            options =>
        {
            options.UseNpgsql(envConnectionString);
        });

        services.AddHangfire(
            globalConfiguration =>
                globalConfiguration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(
                        hangfireConnectionString,
                        new PostgreSqlStorageOptions
                        {
                            PrepareSchemaIfNecessary = true,
                        }));
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IDeleteUncorfimedUserService, DeleteUnconfirmedUserJobService>();
        services.AddScoped<TokenService>();
        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings:SecurityKey").Value!)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
            .AddPolicy("User", policy => policy.RequireRole("User"));
        services.AddHangfireServer();

        return services;
    }

    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(
            options =>
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

    public static IServiceCollection ConfigureUserGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        var address = Environment.GetEnvironmentVariable("USER_GRPC_URL")
                      ?? configuration["UserGrpcUrl:GrpcUrl"];

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddGrpcClient<UserService.GrpcServer.UserProfileService.UserProfileServiceClient>(
                options =>
                {
                    options.Address = new Uri(address);
                })
            .ConfigurePrimaryHttpMessageHandler(
                () => new SocketsHttpHandler
                {
                    AllowAutoRedirect = true,
                });

        return services;
    }
}