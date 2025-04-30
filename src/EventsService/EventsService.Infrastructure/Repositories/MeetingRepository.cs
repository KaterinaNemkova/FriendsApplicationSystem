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
    //ToDo: if reject on meeting => delete ProfileId from list of participants, if accept - it's ok, stay all like was
    //public async Task<Meeting?> AcceptMeetingAsync()
    //public async Task<Meeting> RejectMeetingAsync()
}