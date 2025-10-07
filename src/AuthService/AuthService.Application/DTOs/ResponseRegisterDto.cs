namespace AuthService.Application.DTOs;

public record ResponseRegisterDto
(
    string? Id,

    string? UserName,

    string? FullName,

    string? Email,

    long? TelegramId
);