namespace AuthService.Domain.Contracts;

public interface IAuthRepository
{
    Task SaveTelegramIdAsync(string userId, long telegramId);

    Task<long?> GetTelegramIdByUserIdAsync(string userId);
}