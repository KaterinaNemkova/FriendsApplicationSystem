using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application;

public class AuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> UserRegister(UserRegistrationDto userDto)
    {
        var user = new ApplicationUser
        {
            NickName = userDto.Name,
            Email = userDto.Email,
            NormalizedEmail = userDto.Email.ToUpper(),
            UserName = userDto.Name,
        };
        
       var result= await _userManager.CreateAsync(user, userDto.Password);

       try(result)
    }
}