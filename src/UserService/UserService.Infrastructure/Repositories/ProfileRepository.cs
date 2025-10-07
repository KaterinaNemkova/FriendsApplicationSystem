namespace UserService.Infrastructure.Repositories;

using MongoDB.Driver;
using UserService.Application.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

public class ProfileRepository : IProfileRepository
{
    private readonly IMongoCollection<Profile> _profilesCollection;

    public ProfileRepository(IMongoDatabase database)
    {
        this._profilesCollection = database.GetCollection<Profile>("Profiles");
    }

    public async Task CreateAsync(Profile profile, CancellationToken token)
    {
        await this._profilesCollection.InsertOneAsync(profile, token);
    }

    public async Task DeleteAsync(Guid profileId, CancellationToken token)
    {
        await this._profilesCollection.DeleteOneAsync(p => p.Id == profileId, token);
    }

    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await this._profilesCollection.Find(p => p.Id == id).FirstOrDefaultAsync(token);
    }

    public async Task<List<Profile>> GetAllAsync(CancellationToken token)
    {
        return await this._profilesCollection
            .Find(_ => true)
            .ToListAsync(token);
    }

    public async Task EstablishStatus(Guid id, ActivityStatus status, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, id);
        var update = Builders<Profile>.Update.Set(p => p.ActivityStatus, status);

        await this._profilesCollection.UpdateOneAsync(filter, update, cancellationToken: token);
    }

    public async Task UpdatePhotoAsync(Guid profileId, Photo photo, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, profileId);
        var update = Builders<Profile>.Update.Set(p => p.Photo, photo);

        await this._profilesCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeletePhotoAsync(Guid profileId, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, profileId);
        var update = Builders<Profile>.Update.Unset(p => p.Photo);

        await this._profilesCollection.UpdateOneAsync(filter, update, cancellationToken: token);
    }
}

