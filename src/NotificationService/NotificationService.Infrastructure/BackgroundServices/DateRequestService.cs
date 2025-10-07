// <copyright file="DateRequestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NotificationService.Infrastructure.BackgroundServices;

using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class DateRequestService : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly ITelegramBotClient _botClient;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;
    private readonly UserService.GrpcServer.UserProfileService.UserProfileServiceClient _userProfileServiceClient;
    private readonly ILogger<DateRequestService> _logger;

    public DateRequestService(
        IMessageConsumer consumer,
        AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient,
        UserService.GrpcServer.UserProfileService.UserProfileServiceClient userProfileServiceClient,
        ITelegramBotClient botClient)
    {
        this._consumer = consumer;
        this._botClient = botClient;
        this._authServiceClient = authServiceClient;
        this._userProfileServiceClient = userProfileServiceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this._consumer.InitializeAsync();

        await this._consumer.StartConsumingAsync(
            queueKey: "DateRequest",
            handleMessage: async (json) =>
            {
                var notification = JsonSerializer.Deserialize<EventsRequestNotification>(json);

                if (notification != null)
                {
                    var userIdRequest = new UserService.GrpcServer.GetUserIdRequest
                    {
                        ProfileId = notification?.ReceiverId.ToString(),
                    };
                    var userIdResponse = await this._userProfileServiceClient.GetUserIdAsync(userIdRequest);
                    if (string.IsNullOrEmpty(userIdResponse.UserId))
                    {
                        this._logger.LogWarning("User not found for profile {ProfileId}", notification.ReceiverId);
                        return;
                    }

                    var request = new AuthService.GrpcServer.GetTelegramIdRequest
                    {
                        UserId = userIdResponse.UserId,
                    };

                    var response = await this._authServiceClient.GetTelegramIdByUserIdAsync(request);

                    if (response.TelegramId == null)
                    {
                        _logger.LogWarning("User not found for profile {ProfileId}", notification.ReceiverId);
                        return;
                    }

                    try
                    {
                        var sentMessage = await _botClient.SendTextMessageAsync(
                            chatId: response.TelegramId,
                            text: notification.Message,
                            cancellationToken: stoppingToken);

                        this._logger.LogInformation(
                            "Message sent successfully. MessageId: {MessageId}",
                            sentMessage.MessageId);
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, "Failed to send Telegram message");
                    }
                }
            },
            stoppingToken);
    }
}