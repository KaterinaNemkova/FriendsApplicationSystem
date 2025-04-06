namespace EventsService.Infrastructure.Repositories;

using System.Linq.Expressions;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class MeetingRepository : Repository<Meeting>, IMeetingRepository
{
    public MeetingRepository(IMongoCollection<Meeting> collection) : base(collection)
    {
    }

    public async Task<List<Meeting>> GetAllFutureAsync(Expression<Func<Meeting, bool>> predicate,CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.Where(predicate);
        var options = new FindOptions<Meeting, Meeting>();
        var cursor = await this._collection.FindAsync(filter, options, cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task<List<Meeting>> GetAllPastAsync(Expression<Func<Meeting, bool>> predicate, CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.Where(predicate);
        var options = new FindOptions<Meeting, Meeting>();
        var cursor = await this._collection.FindAsync(filter, options, cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

}