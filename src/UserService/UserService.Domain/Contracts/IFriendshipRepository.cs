using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.Contracts;

public interface IFriendshipRepository
{
    Task AddFriendAsync(Friendship friendship, CancellationToken token);
    Task<bool> FriendshipExistsAsync(Guid profileId, Guid friendId, CancellationToken cancellationToken);
    Task<Friendship> EstablishRelationStatusAsync(Friendship friendship, RelationStatus status,
        CancellationToken token);
    Task DeleteFriendAsync(Friendship friendship, CancellationToken token);

}