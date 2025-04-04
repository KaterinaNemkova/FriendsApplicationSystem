using System.Linq.Expressions;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

namespace EventsService.Infrastructure.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly IMongoCollection<Meeting> _meetingsCollection;

    public MeetingRepository(IMongoDatabase database)
    {
        _meetingsCollection = database.GetCollection<Meeting>("Meetings");
    }

    public async Task CreateAsync(Meeting meeting, CancellationToken cancellationToken)
    {
        await _meetingsCollection.InsertOneAsync(meeting, cancellationToken: cancellationToken);
    }

    public async Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _meetingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Meeting>> GetAllFutureAsync(Expression<Func<Meeting, bool>> predicate,CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.Where(predicate);
        var options = new FindOptions<Meeting, Meeting>();
        var cursor = await _meetingsCollection.FindAsync(filter, options, cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task<List<Meeting>> GetAllPastAsync(Expression<Func<Meeting, bool>> predicate, CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.Where(predicate);
        var options = new FindOptions<Meeting, Meeting>();
        var cursor = await this._meetingsCollection.FindAsync(filter, options, cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Meeting meeting, CancellationToken cancellationToken)
    {
        var filter = Builders<Meeting>.Filter.Where(m => m.Id == meeting.Id);
        await _meetingsCollection.ReplaceOneAsync(filter, meeting, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _meetingsCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}