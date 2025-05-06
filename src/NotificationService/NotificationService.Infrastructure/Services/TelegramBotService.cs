namespace NotificationService.Infrastructure.Services;

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;
    private readonly IMongoCollection<ProfileCreatedNotification> _profileNotificationsCollection;
    private readonly ILogger<TelegramBotService> _logger;

    public TelegramBotService(
        ITelegramBotClient botClient,
        AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient,
        IMongoDatabase database,
        ILogger<TelegramBotService> logger)
    {
        this._botClient = botClient;
        this._authServiceClient = authServiceClient;
        this._profileNotificationsCollection = database.GetCollection<ProfileCreatedNotification>("ProfileCreatedNotifications");
        this._logger = logger;
    }

    public void Start()
    {
        this._logger.LogInformation("Telegram bot is starting to listen...");

        this._botClient.StartReceiving(
            async (bot, update, token) =>
            {
                try
                {
                    var message = update.Message;
                    if (message?.Text == null)
                    {
                        return;
                    }

                    var telegramId = message.Chat.Id;
                    var messageParts = message.Text.Split(" ");

                    if (messageParts[0] != "/start")
                    {
                        return;
                    }

                    if (messageParts.Length <= 1)
                    {
                        await bot.SendTextMessageAsync(
                            chatId: telegramId,
                            text: "Hello! Please use the UserID link to register.",
                            cancellationToken: token);
                        this._logger.LogWarning("No UserId found in the /start command.");
                        return;
                    }

                    var userIdFromLink = messageParts[1];

                    var checkRequest = new AuthService.GrpcServer.GetTelegramIdRequest()
                    {
                        UserId = userIdFromLink,
                    };

                    var checkResponse = await this._authServiceClient.GetTelegramIdByUserIdAsync(checkRequest);

                    if (checkResponse.TelegramId == 0)
                    {
                        var saveTelegramIdRequest = new AuthService.GrpcServer.SaveTelegramIdRequest
                        {
                            UserId = userIdFromLink,
                            TelegramId = telegramId,
                        };

                        this._logger.LogWarning($"Sending request to AuthService: UserId={userIdFromLink}, TelegramId={telegramId}");

                        await this._authServiceClient.SaveTelegramIdAsync(saveTelegramIdRequest);

                        await bot.SendTextMessageAsync(
                            chatId: telegramId,
                            text: $"Thank you! Your Telegram ID is saved.",
                            cancellationToken: token);

                        this._logger.LogWarning($"Saved Telegram ID {telegramId} for user {userIdFromLink}");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(
                            chatId: telegramId,
                            text: "Your Telegram ID has already been saved!",
                            cancellationToken: token);

                        this._logger.LogWarning($"Telegram ID already exists for user");
                    }

                    var pendingNotifications = await this._profileNotificationsCollection
                        .Find(n => n.UserId == Guid.Parse(userIdFromLink))
                        .ToListAsync();

                    foreach (var notification in pendingNotifications)
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: telegramId,
                            text: notification.Message,
                            cancellationToken: token);

                        await this._profileNotificationsCollection.DeleteOneAsync(n => n.UserId == notification.UserId);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogWarning($"Error while processing update: {ex.Message}");
                }
            },
            (_, exception, _) =>
            {
                this._logger.LogWarning($"Error occurred: {exception.Message}");
                return Task.CompletedTask;
            });
    }
}
