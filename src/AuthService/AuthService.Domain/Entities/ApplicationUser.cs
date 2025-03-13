using Microsoft.AspNetCore.Identity;
namespace AuthService.Domain.Entities;

public class ApplicationUser:IdentityUser
{
    public Guid Id { get; set; }
    
    public string NickName { get; set; }
    
    public string EmailAddress { get; set; } 
    
    public string PasswordHash { get; set; }
}