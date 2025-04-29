namespace NotificationService.Infrastructure.BackgroundServices;

using System.Text.Json;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using NotificationService.Application.Contracts;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class ProfileCreatedService : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly IMongoCollection<ProfileCreatedNotification> _profileNotificationsCollection;

    public ProfileCreatedService(
        IMessageConsumer consumer,
        AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient,
        ITelegramBotClient botClient,
        IMongoDatabase database)
    {
        this._consumer = consumer;
        this._profileNotificationsCollection = database.GetCollection<ProfileCreatedNotification>("ProfileCreatedNotifications");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this._consumer.InitializeAsync();
        await this._consumer.StartConsumingAsync(
            queueKey: "ProfileCreated",
            handleMessage: async (json) =>
            {
                var notification = JsonSerializer.Deserialize<ProfileCreatedNotification>(json);
                if (notification != null)
                {
                    await this._profileNotificationsCollection.InsertOneAsync(
                        new ProfileCreatedNotification
                    {
                        Id = notification.Id,
                        UserId = notification.UserId,
                        ProfileId = notification.ProfileId,
                        Message = notification.Message,
                        UserName = notification.UserName,
                    });
                }
            },
            stoppingToken);
    }
}