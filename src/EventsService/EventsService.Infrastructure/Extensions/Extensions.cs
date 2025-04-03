namespace EventsService.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class Extensions
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB2_CONNECTION_STRING") ?? string.Empty;
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("EventsDatabase");
        });
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