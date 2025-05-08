namespace AuthService.Domain.Entities;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public long? TelegramId { get; set; }
}