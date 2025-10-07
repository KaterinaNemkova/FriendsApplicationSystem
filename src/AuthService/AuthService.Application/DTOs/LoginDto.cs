using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs;

public record LoginDto
(
    [Required] string Email,
    [Required] string Password
);