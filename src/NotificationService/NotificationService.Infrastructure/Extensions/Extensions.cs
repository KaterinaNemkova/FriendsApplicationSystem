namespace NotificationService.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs;
using NotificationService.Infrastructure.BackgroundServices;
using NotificationService.Infrastructure.Options;
using NotificationService.Infrastructure.Services;
using Telegram.Bot;

public static class Extensions
{
    private static void RegisterMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(ProfileCreatedNotification)))
        {
            BsonClassMap.RegisterClassMap<ProfileCreatedNotification>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(c => c.UserId)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    cm.MapMember(c => c.ProfileId)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB3_CONNECTION_STRING") ?? string.Empty;
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));
        services.AddSingleton<IMongoDatabase>(
            sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("NotificationDatabase");
            });
        RegisterMappings();
        return services;
    }

    public static IServiceCollection ConfigureAuthGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["AuthGrpcUrl:GrpcUrl"] = Environment.GetEnvironmentVariable("AUTH_GRPC_URL");

        var address = configuration["AuthGrpcUrl:GrpcUrl"]
                      ?? throw new InvalidOperationException("AuthGrpcUrl:GrpcUrl is not configured!");

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddGrpcClient<AuthService.GrpcServer.AuthService.AuthServiceClient>(
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

    public static void ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var messageBrokerSection = configuration.GetSection("MessageBroker");

        messageBrokerSection["HostName"] =
            Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? messageBrokerSection["HostName"];
        messageBrokerSection["Username"] =
            Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? messageBrokerSection["Username"];
        messageBrokerSection["Password"] =
            Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? messageBrokerSection["Password"];
        messageBrokerSection["Port"] =
            Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? messageBrokerSection["Port"];

        services.AddOptions<RabbitMQOptions>()
            .Bind(messageBrokerSection)
            .ValidateDataAnnotations()
            .Validate(config => true, "Queues section is required")
            .ValidateOnStart();

        services.AddSingleton<IMessageConsumer, RabbitMQConsumer>();
        services.AddHostedService<FriendRequestService>();
        services.AddHostedService<ProfileCreatedService>();
    }

    public static IServiceCollection ConfigureTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["TelegramBot:AccessToken"] = Environment.GetEnvironmentVariable("TELEGRAM_BOT");
        services.AddSingleton<ITelegramBotClient>(
            new TelegramBotClient(configuration["TelegramBot:AccessToken"] ?? string.Empty));
        services.AddSingleton<TelegramBotService>();
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