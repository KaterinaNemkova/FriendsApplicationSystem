namespace EventsService.Infrastructure.Repositories;

using System.Linq.Expressions;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class MeetingRepository : Repository<Meeting>, IMeetingRepository
{
    public MeetingRepository(IMongoCollection<Meeting> collection)
        : base(collection)
    {
    }

    public async Task<List<Meeting>> GetAllFutureAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.AnyEq("ParticipantIds", id);
        return await this._collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Meeting>> GetAllPastAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.AnyEq("ParticipantIds", id);
        return await this._collection.Find(filter).ToListAsync(cancellationToken);
    }

}