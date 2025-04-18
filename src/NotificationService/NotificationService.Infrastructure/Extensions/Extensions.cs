namespace NotificationService.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class Extensions
{
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

}