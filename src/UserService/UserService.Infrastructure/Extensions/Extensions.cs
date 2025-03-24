using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace UserService.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddDb(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString=configuration.GetConnectionString("MongoDb");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("UserDatabase");

        services.AddSingleton<IMongoDatabase>(database);
        return services; 
    }
    
}