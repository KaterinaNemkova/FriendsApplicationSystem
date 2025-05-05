namespace EventsService.Infrastructure.Extensions;

using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using EventsService.Infrastructure.Options;
using EventsService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

public static class Extensions
{
    private static void RegisterMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
        {
            BsonClassMap.RegisterClassMap<Entity>(
                cm =>
            {
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Event)))
        {
            BsonClassMap.RegisterClassMap<Event>(
                cm =>
                {
                    cm.MapMember(c => c.ParticipantIds)
                        .SetSerializer(
                            new EnumerableInterfaceImplementerSerializer<List<Guid>>(
                                new GuidSerializer(GuidRepresentation.Standard)));
                    cm.MapMember(c => c.Title);
                    cm.MapMember(c => c.Description);
                });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Meeting)))
        {
            BsonClassMap.RegisterClassMap<Meeting>(
                cm =>
                {
                    cm.AutoMap();

                    cm.MapMember(c => c.Author)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Goal)))
        {
            BsonClassMap.RegisterClassMap<Goal>(
                cm =>
                {
                    cm.AutoMap();

                    cm.MapMember(c => c.Author)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB2_CONNECTION_STRING") ?? string.Empty;
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));
        services.AddSingleton<IMongoDatabase>(
            sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("EventsDatabase");
        });
        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Date>("Dates"));

        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Meeting>("Meetings"));

        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Goal>("Goals"));
        RegisterMappings();
        return services;
    }

    public static IServiceCollection AddRepresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(
            c =>
        {
            c.EnableAnnotations();
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
                    !string.IsNullOrEmpty(config.Queues.MeetingRequest),
                "Queue names must be configured in RabbitMQ options.")
            .ValidateOnStart();

        services.AddSingleton<IMessageService, RabbitMQService>();
    }
}