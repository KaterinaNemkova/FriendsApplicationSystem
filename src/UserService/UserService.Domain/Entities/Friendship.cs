using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class Friendship
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; }
    public Guid FriendProfileId { get; set; }
    public Profile FriendProfile { get; set; }
    public DateOnly BeginningOfInterrelations { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public RelationStatus RelationStatus { get; set; } = RelationStatus.Friend;
}
