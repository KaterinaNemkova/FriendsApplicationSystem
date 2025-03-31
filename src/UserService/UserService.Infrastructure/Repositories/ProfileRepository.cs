using System.Linq.Expressions;
using MongoDB.Driver;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.Repositories;

public class ProfileRepository:IProfileRepository
{
    private readonly IMongoCollection<Profile> _profilesCollection;
    
    public ProfileRepository(IMongoDatabase database)
    {
        _profilesCollection = database.GetCollection<Profile>("Profiles");
    }

    public async Task CreateAsync(Profile profile, CancellationToken token)
    {
        await _profilesCollection.InsertOneAsync(profile, token);
    }

    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _profilesCollection.Find(p => p.Id == id).FirstOrDefaultAsync(token);
    }
    
    public async Task<Profile> GetByNameAsync(string name, CancellationToken token)
    {
        return await _profilesCollection.Find(p => p.Name==name).FirstOrDefaultAsync(token);
    }

    public async Task<List<Profile>> GetAllAsync(CancellationToken token)
    {
        return await _profilesCollection
            .Find(_ => true)
            .ToListAsync(token);
    }

    public async Task<long> GetTotalCountAsync()
    {
        return await _profilesCollection.CountDocumentsAsync(_ => true);
    }
    
    public async Task EstablishStatus(Guid Id, ActivityStatus status, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, Id);
        var update = Builders<Profile>.Update.Set(p => p.ActivityStatus, status);

        await _profilesCollection.UpdateOneAsync(filter, update, cancellationToken: token);
    }

    public async Task UpdatePhotoAsync(Guid profileId, Photo photo, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, profileId);
        var update = Builders<Profile>.Update.Set(p => p.Photo, photo);

        await _profilesCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeletePhotoAsync(Guid profileId, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, profileId);
        var update = Builders<Profile>.Update.Unset(p => p.Photo);

        await _profilesCollection.UpdateOneAsync(filter, update, cancellationToken: token);
    }

    public async Task AddFriendAsync(Guid profileId, Guid friendId, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, profileId);
        
    }

}

