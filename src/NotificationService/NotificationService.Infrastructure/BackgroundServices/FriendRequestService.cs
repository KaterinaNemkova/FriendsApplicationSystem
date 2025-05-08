namespace NotificationService.Infrastructure.BackgroundServices;

using System.Text.Json;
using Microsoft.Extensions.Hosting;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class FriendRequestService : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly ITelegramBotClient _botClient;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;

    public FriendRequestService(IMessageConsumer consumer, AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient, ITelegramBotClient botClient)
    {
        this._consumer = consumer;
        this._botClient = botClient;
        this._authServiceClient = authServiceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this._consumer.InitializeAsync();

        await this._consumer.StartConsumingAsync(
            queueKey: "FriendRequest",
            handleMessage: async (json) =>
            {
                var notification = JsonSerializer.Deserialize<FriendRequestNotification>(json);

                if (notification != null)
                {
                    var request = new AuthService.GrpcServer.GetTelegramIdRequest
                    {
                        UserId = notification.ReceiverUserId.ToString(),
                    };

                    var response = await this._authServiceClient.GetTelegramIdByUserIdAsync(request);

                    if (response.TelegramId != 0)
                    {
                        await this._botClient.SendTextMessageAsync(
                            chatId: response.TelegramId,
                            text: notification.Message,
                            cancellationToken: stoppingToken);
                    }
                }
            },
            stoppingToken);
    }
}