namespace EventsService.Infrastructure.Extensions;

using EventsService.Domain.Entities;
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
        if (!BsonClassMap.IsClassMapRegistered(typeof(Event)))
        {
            BsonClassMap.RegisterClassMap<Event>(
                cm =>
            {
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.ParticipantIds)
                    .SetSerializer(
                        new EnumerableInterfaceImplementerSerializer<List<Guid>>(
                        new GuidSerializer(GuidRepresentation.Standard)));
                cm.MapMember(c => c.Title);
                cm.MapMember(c => c.Description);
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
}