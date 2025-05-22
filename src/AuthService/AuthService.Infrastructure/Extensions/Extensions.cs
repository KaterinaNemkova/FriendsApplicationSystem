using AuthService.Infrastructure.HangfireJobs;

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
        this IServiceCollection services)
    {
        var envConnectionString = Environment.GetEnvironmentVariable("POSTGRES_DB_CONNECTION_STRING");
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
        return services;
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using FriendsAppDbContext context = scope.ServiceProvider.GetRequiredService<FriendsAppDbContext>();

        context.Database.Migrate();
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddAuthentication();

        services.AddAuthorization();

        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FriendsAppDbContext>();

        services.Configure<IdentityOptions>(
            options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });
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
        configuration["UserGrpcUrl:GrpcUrl"] = Environment.GetEnvironmentVariable("USER_GRPC_URL");

        var address = configuration["UserGrpcUrl:GrpcUrl"]
                      ?? throw new InvalidOperationException("UserGrpcUrl:GrpcUrl is not configured!");

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