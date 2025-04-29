namespace NotificationService.Infrastructure.Services;

using MongoDB.Driver;
using NotificationService.Application.DTOs;
using Telegram.Bot;

public class TelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly AuthService.GrpcServer.AuthService.AuthServiceClient _authServiceClient;
    private readonly IMongoCollection<ProfileCreatedNotification> _profileNotificationsCollection;

    public TelegramBotService(
        ITelegramBotClient botClient,
        AuthService.GrpcServer.AuthService.AuthServiceClient authServiceClient,
        IMongoDatabase database)
    {
        this._botClient = botClient;
        this._authServiceClient = authServiceClient;
        this._profileNotificationsCollection = database.GetCollection<ProfileCreatedNotification>("ProfileCreatedNotifications");
    }

    public void Start()
    {
        Console.WriteLine("Telegram bot is starting to listen...");

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
                            text: "Привет! Пожалуйста, используйте ссылку с UserID для регистрации.",
                            cancellationToken: token);
                        Console.WriteLine("No UserId found in the /start command.");
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

                        Console.WriteLine($"Sending request to AuthService: UserId={userIdFromLink}, TelegramId={telegramId}");

                        await this._authServiceClient.SaveTelegramIdAsync(saveTelegramIdRequest);

                        await bot.SendTextMessageAsync(
                            chatId: telegramId,
                            text: $"Спасибо! Ваш Telegram ID сохранён для пользователя {userIdFromLink}.",
                            cancellationToken: token);

                        Console.WriteLine($"Saved Telegram ID {telegramId} for user {userIdFromLink}");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(
                            chatId: telegramId,
                            text: "Ваш Telegram ID уже был сохранён ранее!",
                            cancellationToken: token);

                        Console.WriteLine($"Telegram ID уже существует для пользователя {userIdFromLink}");
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
                    Console.WriteLine($"Error while processing update: {ex.Message}");
                }
            },
            (_, exception, _) =>
            {
                Console.WriteLine($"Error occurred: {exception.Message}");
                return Task.CompletedTask;
            });
    }
}
