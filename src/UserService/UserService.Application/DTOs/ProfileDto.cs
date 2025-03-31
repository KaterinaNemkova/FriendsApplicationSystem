using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.DTOs;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ActivityStatus ActivityStatus { get; set; }
}