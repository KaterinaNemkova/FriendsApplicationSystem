namespace NotificationService.Infrastructure.BackgroundServices;

using System.Text.Json;
using Microsoft.Extensions.Hosting;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class MeetingRequestService : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly ITelegramBotClient _botClient;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;
    private readonly UserService.GrpcServer.UserProfileService.UserProfileServiceClient _userProfileServiceClient;

    public MeetingRequestService(IMessageConsumer consumer, AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient, ITelegramBotClient botClient)
    {
        this._consumer = consumer;
        this._botClient = botClient;
        this._authServiceClient = authServiceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this._consumer.InitializeAsync();

        await this._consumer.StartConsumingAsync(
            queueKey: "MeetingRequest",
            handleMessage: async (json) =>
            {
                var notification = JsonSerializer.Deserialize<MeetingRequestNotification>(json);

                var userIdResponse = new UserService.GrpcServer.GetUserIdRequest
                {
                    ProfileId = notification?.RecieverId.ToString(),
                };
                if (notification != null)
                {
                    var request = new AuthService.GrpcServer.GetTelegramIdRequest
                    {
                        UserId = userIdResponse.ProfileId,
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