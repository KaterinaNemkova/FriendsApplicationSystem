using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UserService.Domain.Contracts;
using UserService.Infrastructure.Helpers;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Extensions;

public static class Extensions
{
    public static IServiceCollection AddDb(this IServiceCollection services,IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING") ?? "";
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));
        
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("UserDatabase");
        });
       return services; 
    }

    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
         
        configuration["CloudinarySettings:CloudName"] = Environment.GetEnvironmentVariable("CLOUD_NAME") ?? "";
        configuration["CloudinarySettings:ApiKey"] = Environment.GetEnvironmentVariable("CLOUD_API_KEY") ?? "";
        configuration["CloudinarySettings:ApiSecretKey"] = Environment.GetEnvironmentVariable("CLOUD_SECRET_API_KEY") ?? "";
        
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        return services;
    }

    public static IServiceCollection AddRepresentation(this IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();


        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations(); 
        });
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        return services;
    }


}