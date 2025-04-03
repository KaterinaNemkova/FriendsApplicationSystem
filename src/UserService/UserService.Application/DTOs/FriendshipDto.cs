using UserService.Domain.Enums;

namespace UserService.Application.DTOs;

public class FriendshipDto
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public Guid FriendProfileId { get; set; }
    public RelationStatus RelationStatus { get; set; }
    public DateOnly BeginningOfInterrelations { get; set; }
}