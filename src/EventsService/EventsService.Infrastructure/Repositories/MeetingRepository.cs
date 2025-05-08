namespace EventsService.Infrastructure.Repositories;

using System.Linq.Expressions;
using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class MeetingRepository : Repository<Meeting>, IMeetingRepository
{
    public MeetingRepository(IMongoCollection<Meeting> collection)
        : base(collection)
    {
    }

    public async Task RejectMeetingAsync(Guid meetingId, Guid profileId, CancellationToken token)
    {
        var filter = Builders<Meeting>.Filter.Eq(p => p.Id, meetingId);
        var update = Builders<Meeting>.Update.Pull(p => p.ParticipantIds, profileId);
        await this._collection.UpdateOneAsync(filter, update, cancellationToken: token);
    }
}