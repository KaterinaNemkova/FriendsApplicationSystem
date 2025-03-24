using System.Linq.Expressions;
using MongoDB.Driver;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.Repositories;

public class ProfileRepository:IProfileRepository
{
    private readonly IMongoCollection<Profile> _profilesCollection;

    public async Task CreateAsync(Profile profile, CancellationToken token)
    {
        await _profilesCollection.InsertOneAsync(profile, token);
    }

    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _profilesCollection.Find(p => p.Id == id).FirstOrDefaultAsync(token);
    }

    public async Task<IEnumerable<Profile>> GetAllAsync(Expression<Func<Profile,bool>> filter, CancellationToken token)
    {
        return await _profilesCollection.Find(filter).ToListAsync(token);
    }

    public async Task UpdateAsync(Profile profile, CancellationToken token)
    {
        await _profilesCollection.ReplaceOneAsync(p => p.Id == profile.Id, profile);
    }

    public async Task DeleteAsync(Profile profile, CancellationToken token)
    {
        await _profilesCollection.DeleteOneAsync(p => p.Id == profile.Id, token);
    }

    public async Task EstablishStatus(Guid Id, ActivityStatus status, CancellationToken token)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.Id, Id);
        var update = Builders<Profile>.Update.Set(p => p.ActivityStatus, status);

        await _profilesCollection.UpdateOneAsync(filter, update, cancellationToken: token);
        
    }

    // public async Task UploadImage(Guid Id, Stream stream, CancellationToken token)
    // {
    //     
    // }
    
}

