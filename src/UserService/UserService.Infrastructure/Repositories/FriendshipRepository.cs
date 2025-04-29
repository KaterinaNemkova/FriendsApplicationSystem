namespace UserService.Infrastructure.Repositories;

using MongoDB.Driver;
using UserService.Application.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

public class FriendshipRepository : IFriendshipRepository
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
        await this._friendshipCollection.InsertOneAsync(friendship, token);
    }

    public async Task<Friendship> FriendshipExistsByIdsAsync(Guid profileId, Guid friendId, CancellationToken cancellationToken)
    {
        var filter = Builders<Friendship>.Filter.Or(
            Builders<Friendship>.Filter.And(
                Builders<Friendship>.Filter.Eq(f => f.ProfileId, profileId),
                Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, friendId)),
            Builders<Friendship>.Filter.And(
                Builders<Friendship>.Filter.Eq(f => f.ProfileId, friendId),
                Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, profileId)));

        return await this._friendshipCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Friendship> GetFriendshipByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<Friendship>.Filter.Eq(f => f.Id, id);
        return await this._friendshipCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteFriendAsync(Friendship friendship, CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.And(
            Builders<Friendship>.Filter.Eq(f => f.ProfileId, friendship.ProfileId),
            Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, friendship.FriendProfileId));

        await this._friendshipCollection.DeleteOneAsync(friendshipFilter, token);

        var profileFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.ProfileId);
        var profileUpdate = Builders<Profile>.Update.Pull(p => p.FriendIds, friendship.FriendProfileId);
        await this._profilesCollection.UpdateOneAsync(profileFilter, profileUpdate, cancellationToken: token);

        var friendFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.FriendProfileId);
        var friendUpdate = Builders<Profile>.Update.Pull(p => p.FriendIds, friendship.ProfileId);
        await this._profilesCollection.UpdateOneAsync(friendFilter, friendUpdate, cancellationToken: token);
    }

    public async Task<Friendship> EstablishRelationStatusAsync(Friendship friendship, RelationStatus status, CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.Eq(p => p.Id, friendship.Id);
        var friendshipUpdate = Builders<Friendship>.Update.Set(f => f.RelationStatus, status);

        await this._friendshipCollection.UpdateOneAsync(friendshipFilter, friendshipUpdate, cancellationToken: token);

        return await this._friendshipCollection.Find(friendshipFilter).FirstOrDefaultAsync(token);
    }

    public async Task<List<Profile>> GetAllFriendsAsync(Guid profileId, CancellationToken token)
    {
        var friendships = await _friendshipCollection
            .Find(f => f.ProfileId == profileId || f.FriendProfileId == profileId)
            .ToListAsync(token);

        var friendIds = friendships
            .Select(f => f.ProfileId == profileId ? f.FriendProfileId : f.ProfileId)
            .Distinct()
            .ToList();
        var filter = Builders<Profile>.Filter.In(p => p.Id, friendIds);
        return await this._profilesCollection.Find(filter).ToListAsync(token);
    }

    public async Task<List<Friendship>> GetAllMyFriendsRequestsAsync(
        Guid profileId,
        CancellationToken token)
    {
        var filter = Builders<Friendship>.Filter.And(
            Builders<Friendship>.Filter.Eq(f => f.RequestStatus, RequestStatus.Pending),
            Builders<Friendship>.Filter.Eq(f => f.FriendProfileId, profileId));

        var pendingFriendships = await this._friendshipCollection
            .Find(filter)
            .ToListAsync(token);

        return pendingFriendships.ToList();
    }

    public async Task<Friendship> ChangeDataOfInterrelations(
        Friendship friendship,
        DateOnly date,
        CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.Eq(p => p.Id, friendship.Id);
        var friendshipUpdate = Builders<Friendship>.Update.Set(f => f.BeginningOfInterrelations, date);

        await this._friendshipCollection.UpdateOneAsync(friendshipFilter, friendshipUpdate, cancellationToken: token);

        return await this._friendshipCollection.Find(friendshipFilter).FirstOrDefaultAsync(token);
    }

    public async Task<Friendship> AcceptFriendRequestAsync(Friendship friendship, CancellationToken token)
    {
        var filter = Builders<Friendship>.Filter.Eq(f => f.Id, friendship.Id);

        await this._friendshipCollection.ReplaceOneAsync(
            filter,
            friendship,
            new ReplaceOptions { IsUpsert = false },
            token);

        var profileFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.ProfileId);
        var profileUpdate = Builders<Profile>.Update.Push(p => p.FriendIds, friendship.FriendProfileId);
        await this._profilesCollection.UpdateOneAsync(profileFilter, profileUpdate, cancellationToken: token);

        var friendFilter = Builders<Profile>.Filter.Eq(p => p.Id, friendship.FriendProfileId);
        var friendUpdate = Builders<Profile>.Update.Push(p => p.FriendIds, friendship.ProfileId);
        await this._profilesCollection.UpdateOneAsync(friendFilter, friendUpdate, cancellationToken: token);

        return this._friendshipCollection.Find(filter).FirstOrDefault(token);
    }

    public async Task RejectFriendRequestAsync(Guid friendshipId, CancellationToken token)
    {
        var friendshipFilter = Builders<Friendship>.Filter.Eq(p => p.Id, friendshipId);
        await this._friendshipCollection.DeleteOneAsync(friendshipFilter, token);
    }

}