using MongoDB.Driver;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.Repositories;

public class FriendshipRepository:IFriendshipRepository
{
    private readonly IMongoCollection<Friendship> _friendshipCollection;
    private readonly IMongoCollection<Profile> _profilesCollection;

    public FriendshipRepository(IMongoDatabase database)
    {
        _friendshipCollection = database.GetCollection<Friendship>("Friendships");
        _profilesCollection = database.GetCollection<Profile>("Profiles");
    }

    public async Task AddFriendAsync(Friendship friendship, CancellationToken token)
    {
        
        await _friendshipCollection.InsertOneAsync(friendship, token);
        
        var profileFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.ProfileId);
        var profileUpdate = Builders<Profile>.Update.Push(p => p.FriendIds, friendship.FriendProfileId);
        await _profilesCollection.UpdateOneAsync(profileFilter, profileUpdate, cancellationToken: token);

        var friendFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.FriendProfileId);
        var friendUpdate = Builders<Profile>.Update.Push(p => p.FriendIds, friendship.ProfileId);
        await _profilesCollection.UpdateOneAsync(friendFilter, friendUpdate, cancellationToken: token);
    
    }
    
    public async Task<bool> FriendshipExistsAsync(Guid profileId, Guid friendId, CancellationToken cancellationToken)
    {
        var filter = Builders<Friendship>.Filter.Or(
            Builders<Friendship>.Filter.And(
                Builders<Friendship>.Filter.Eq(f => f.ProfileId, profileId),
                Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, friendId)
            ),
            Builders<Friendship>.Filter.And(
                Builders<Friendship>.Filter.Eq(f => f.ProfileId, friendId),
                Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, profileId)
            )
        );

        return await _friendshipCollection.Find(filter).AnyAsync(cancellationToken);
    }

    public async Task DeleteFriendAsync(Friendship friendship, CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.And(
            Builders<Friendship>.Filter.Eq(f => f.ProfileId, friendship.ProfileId),
            Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, friendship.FriendProfileId)
        );

        await _friendshipCollection.DeleteOneAsync(friendshipFilter, token);

        var profileFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.ProfileId);
        var profileUpdate = Builders<Profile>.Update.Pull(p => p.FriendIds, friendship.FriendProfileId);
        await _profilesCollection.UpdateOneAsync(profileFilter, profileUpdate, cancellationToken: token);
        
        var friendFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.FriendProfileId);
        var friendUpdate = Builders<Profile>.Update.Pull(p => p.FriendIds, friendship.ProfileId);
        await _profilesCollection.UpdateOneAsync(friendFilter, friendUpdate, cancellationToken: token);
    }
    
    
    public async Task<Friendship> EstablishRelationStatusAsync(Friendship friendship, RelationStatus status, CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.Eq(p=>p.Id, friendship.Id);
        var friendshipUpdate= Builders<Friendship>.Update.Set(f => f.RelationStatus, status);
        
        await _friendshipCollection.UpdateOneAsync(friendshipFilter,friendshipUpdate, cancellationToken: token);
        
        return await _friendshipCollection.Find(friendshipFilter).FirstOrDefaultAsync(token);
        
    }

    //get all my friends
}