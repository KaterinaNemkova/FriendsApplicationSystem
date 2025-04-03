using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class Profile
{
    public Guid Id { get; set; } = Guid.NewGuid();     
    public Guid UserId { get; set; }
    public string Name { get; set; }    
    public Photo Photo { get; set; }
    public ActivityStatus ActivityStatus { get; set; } = ActivityStatus.Busy;
    public List<Guid>? FriendIds { get; set; } = [];

}