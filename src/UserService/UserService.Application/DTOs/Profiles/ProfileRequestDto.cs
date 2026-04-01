using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.DTOs.Profiles;

public class ProfileRequestDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public Photo Photo { get; set; }
}