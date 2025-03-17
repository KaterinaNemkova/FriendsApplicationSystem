namespace AuthService.Infrastructure.MyIdentityApi;

public class MyLoginRequest
{
    public required string Email { get; init; }
    
    public required string Password { get; init; }

}