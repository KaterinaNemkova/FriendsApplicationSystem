using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entities;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }

    public long? TelegramId { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}