using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.Contracts;

public interface IFriendshipRepository
{
    Task AddFriendAsync(Friendship friendship, CancellationToken token);
    Task<Friendship> FriendshipExistsByIdsAsync(Guid profileId, Guid friendId, CancellationToken cancellationToken);
    Task<Friendship> GetFriendshipByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Friendship> EstablishRelationStatusAsync(Friendship friendship, RelationStatus status,
        CancellationToken token);
    Task DeleteFriendAsync(Friendship friendship, CancellationToken token);
    Task<List<Profile>> GetAllFriendsAsync(Guid profileId, CancellationToken token);

    Task<Friendship> ChangeDataOfInterrelations(Friendship friendship, DateOnly date,
        CancellationToken token);

}