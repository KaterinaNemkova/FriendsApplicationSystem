namespace AuthService.Infrastructure.Repositories;

using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FriendsAppDbContext _dbContext;

    public AuthRepository(UserManager<ApplicationUser> userManager, FriendsAppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task SaveTelegramIdAsync(string userId, long telegramId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.TelegramId = telegramId;
        await this._userManager.UpdateAsync(user);
    }

    public async Task<long?> GetTelegramIdByUserIdAsync(string userId)
    {
        var user = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.TelegramId;
    }


}