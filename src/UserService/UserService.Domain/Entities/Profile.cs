using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class Profile
{
    public Guid Id { get; set; }       
    public Guid UserId { get; set; }    // Связь с User (из AuthService)
    public string Name { get; set; }    
    public string PhotoUrl { get; set; }
    public ActivityStatus ActivityStatus { get; set; } = ActivityStatus.Busy;
    public ICollection<Friendship> Friends { get; set; }

}