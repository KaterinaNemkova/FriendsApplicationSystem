using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs;

public record RegisterDto
{
    [Required] public string? UserName { get; set; } = string.Empty;

    [Required] public string FullName { get; set; } = string.Empty;

    [Required] public string Email { get; set; } = string.Empty;

    [Required] public string? Password { get; set; } = string.Empty;
}