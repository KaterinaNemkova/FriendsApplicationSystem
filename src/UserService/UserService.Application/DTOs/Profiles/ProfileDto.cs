namespace UserService.Application.DTOs.Profiles;

using UserService.Domain.Enums;

public class ProfileDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ActivityStatus ActivityStatus { get; set; }
}