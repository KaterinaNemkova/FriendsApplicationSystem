namespace AuthService.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; }
    
    public UserInfoDto UserInfo { get; set; }
}