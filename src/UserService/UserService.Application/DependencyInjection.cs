namespace UserService.Application;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureNotificationGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["NotificationGrpcUrl:GrpcUrl"] = Environment.GetEnvironmentVariable("NOTIFICATION_GRPC_URL");
        services.AddGrpcClient<NotificationService.GrpcServer.NotificationService.NotificationServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                        configuration.GetSection("NotificationGrpcUrl").ToString() 
                                              ?? string.Empty);
                })
            .ConfigurePrimaryHttpMessageHandler(
                () =>
            {
                var handler = new HttpClientHandler();

                handler.ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true;

                return handler;
            });

        return services;
    }
}