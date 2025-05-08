namespace UserService.Application.DTOs.Friendships;

using UserService.Domain.Enums;

public class FriendshipDto
{
    public Guid Id { get; init; }

    public Guid ProfileId { get; init; }

    public Guid FriendProfileId { get; init; }

    public RelationStatus RelationStatus { get; init; }

    public DateOnly BeginningOfInterrelations { get; init; }

    public RequestStatus RequestStatus { get; init; }
}