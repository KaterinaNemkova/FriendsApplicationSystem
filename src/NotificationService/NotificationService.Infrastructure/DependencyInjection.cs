namespace NotificationService.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["TelegramBot:AccessToken"] = Environment.GetEnvironmentVariable("TELEGRAM_BOT");
        services.AddSingleton<ITelegramBotClient>(
            new TelegramBotClient(configuration["TelegramBot:AccessToken"] ?? string.Empty));
        return services;
    }

}