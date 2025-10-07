namespace AuthService.Application.DTOs;

public record ResponseLoginDto(string Token, DateTime Expires, string UserId, string UserName, IList<string> Roles);