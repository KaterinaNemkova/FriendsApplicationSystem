namespace UserService.Application.Contracts;

using UserService.Domain.Entities;
using UserService.Domain.Enums;

public interface IFriendshipRepository
{
    Task AddFriendAsync(Friendship friendship, CancellationToken token);

    Task<Friendship> FriendshipExistsByIdsAsync(Guid profileId, Guid friendId, CancellationToken cancellationToken);

    Task<Friendship> GetFriendshipByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Friendship> EstablishRelationStatusAsync(Friendship friendship, RelationStatus status,
        CancellationToken token);

    Task DeleteFriendAsync(Friendship friendship, CancellationToken token);

    Task<List<Profile>> GetAllFriendsAsync(Guid profileId, CancellationToken token);

    Task<Friendship> ChangeDataOfInterrelations(
        Friendship friendship,
        DateOnly date,
        CancellationToken token);

    Task<Friendship> AcceptFriendRequestAsync(
        Friendship friendship,
        CancellationToken token);

    Task RejectFriendRequestAsync(Guid friendshipId, CancellationToken token);

    Task<List<Friendship>> GetAllMyFriendsRequestsAsync(
        Guid profileId,
        CancellationToken token);
}