namespace UserService.Infrastructure.Extensions;

using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UserService.Application.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Infrastructure.Helpers;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Services;

public static class Extensions
{
    private static void RegisterMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Profile)))
        {
            BsonClassMap.RegisterClassMap<Profile>(
                cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.ActivityStatus)
                    .SetSerializer(new EnumSerializer<ActivityStatus>(BsonType.String));
                cm.MapMember(c => c.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.UserId)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.FriendIds)
                    .SetSerializer(
                        new EnumerableInterfaceImplementerSerializer<List<Guid>>(
                            new GuidSerializer(GuidRepresentation.Standard)));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Friendship)))
        {
            BsonClassMap.RegisterClassMap<Friendship>(
                cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.ProfileId)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.FriendProfileId)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.RelationStatus)
                    .SetSerializer(new EnumSerializer<RelationStatus>(BsonType.String));
                cm.MapMember(c => c.RequestStatus)
                    .SetSerializer(new EnumSerializer<RequestStatus>(BsonType.String));
            });
        }
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING") ?? string.Empty;
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("UserDatabase");
        });
        RegisterMappings();
        return services;
    }

    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUD_NAME") ?? string.Empty;
        configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUD_API_KEY") ?? string.Empty;
        configuration["CloudinarySettings:ApiSecretKey"] = Environment.GetEnvironmentVariable("CLOUD_SECRET_API_KEY") ?? string.Empty;

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        return services;
    }

    public static IServiceCollection AddRepresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(
            c =>
        {
            c.EnableAnnotations();
        });
        services
            .AddControllers()
            .AddJsonOptions(
                options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
            .Validate(
                config =>
                    config.Queues != null &&
                    !string.IsNullOrEmpty(config.Queues.ProfileCreated) &&
                    !string.IsNullOrEmpty(config.Queues.FriendRequest),
                "Both 'ProfileCreated' and 'FriendRequest' queue names must be configured in RabbitMQ options.")
            .ValidateOnStart();

        services.AddSingleton<IMessageService, RabbitMQService>();
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
}