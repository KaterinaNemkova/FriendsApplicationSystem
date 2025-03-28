using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.DTOs;

public class ProfileDto
{
    public Guid ProfileId { get; set; }
    public string Name { get; set; }
}